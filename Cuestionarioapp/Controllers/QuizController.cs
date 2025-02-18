using Azure;
using Cuestionarioapp.Models;
using Cuestionarioapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Cuestionarioapp.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IEmailService _emailService;
        private readonly ILogger<QuizController> _logger;
        private readonly UserManager<IdentityUser> _userManager;


        public QuizController(
            IQuizService quizService,
            IEmailService emailService,
            ILogger<QuizController> logger,
            UserManager<IdentityUser> userManager)
        {
            _quizService = quizService;
            _emailService = emailService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Solo usuarios autenticados pueden acceder
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            
            var questions = await _quizService.GetQuestionsWithUserResponses(userId);
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResponse(int questionId, string response, string responseType)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                UserResponse userResponse;
                int responseId;

                // Buscar si ya existe una respuesta para esta pregunta y usuario
                var existingResponse = await _quizService.GetResponseByQuestionIdAndUserId(questionId, userId);

                if (existingResponse != null)
                {
                    // Actualizar la respuesta existente
                    existingResponse.Response = response;
                    existingResponse.ResponseType = responseType;
                    existingResponse.ModifiedAt = DateTime.UtcNow;
                    await _quizService.UpdateResponseAsync(existingResponse);

                    userResponse = existingResponse;
                    responseId = existingResponse.Id;
                }
                else
                {
                    // Si no existe, crear una nueva
                    var newResponse = new UserResponse
                    {
                        QuestionId = questionId,
                        UserId = userId,
                        Response = response,
                        ResponseType = responseType,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _quizService.SaveResponseAsync(newResponse);
                    userResponse = newResponse;
                    responseId = newResponse.Id;
                }

                // El resto del código según el tipo de respuesta
                switch (responseType.ToLower())
                {
                    case "popup":
                        return Json(new { success = true, message = response });
                    case "newwindow":
                        return Json(new { success = true, responseId = responseId });
                    case "email":
                        var responseWithQuestion = await _quizService.GetResponseByQuestionIdAndUserId(questionId, userId);
                        await _emailService.SendResponseEmailAsync(User.Identity.Name, responseWithQuestion);
                        return Json(new { success = true, message = "Response sent via email" });
                    default: // samewindow
                        return PartialView("_ResponseNote", userResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting response");
                return Json(new { success = false, message = "Error saving response" });
            }
        }

        public async Task<IActionResult> ShowResponse(int id)
        {
            var response = await _quizService.GetResponseByIdAsync(id);
            if (response == null)
                return NotFound();

            return View(response);
        }

        public async Task<IActionResult> ViewResponses()
        {
            var responses = await _quizService.GetUserResponsesAsync();
            return View(responses);
        }

        [HttpPost]
        public async Task<IActionResult> ExportResponses()
        {
            try
            {
                var fileName = $"quiz_responses_{DateTime.Now:yyyyMMddHHmmss}.txt";
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                await _quizService.ExportResponsesToFile(filePath);

                // Leemos el archivo
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Limpiamos el archivo temporal
                System.IO.File.Delete(filePath);

                // Configuramos la respuesta para forzar la descarga
                return File(
                    fileContents: fileBytes,
                    contentType: "text/plain",
                    fileDownloadName: fileName,
                    enableRangeProcessing: true
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting responses");
                return Json(new { success = false, message = "Error exporting responses" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateResponse(int questionId, string response)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userResponse = await _quizService.GetResponseByQuestionIdAndUserId(questionId, userId);

                if (userResponse != null)
                {
                    userResponse.Response = response;
                    userResponse.ModifiedAt = DateTime.UtcNow;
                    await _quizService.UpdateResponseAsync(userResponse);
                    return Json(new { success = true });
                }
                else
                {
                    // Si no existe, crear nueva respuesta
                    var newResponse = new UserResponse
                    {
                        QuestionId = questionId,
                        UserId = userId,
                        Response = response,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _quizService.SaveResponseAsync(newResponse);
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating response");
                return Json(new { success = false, message = "Error al actualizar la respuesta" });
            }
        }

    }
}

