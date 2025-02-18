using Cuestionarioapp.Data;
using Cuestionarioapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuestionarioapp.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
            _httpContextAccessor = new HttpContextAccessor();
        }
        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.UserResponses)
                .ToListAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.UserResponses)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<UserResponse> SaveResponseAsync(UserResponse response)
        {
            // Si es una nueva respuesta, cargamos la pregunta
            if (response.Question == null)
            {
                response.Question = await _context.Questions.FindAsync(response.QuestionId);
            }

            response.UserId = GetCurrentUserId();
            response.CreatedAt = DateTime.UtcNow;
            _context.UserResponses.Add(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<List<UserResponse>> GetUserResponsesAsync()
        {
            var userId = GetCurrentUserId();
            return await _context.UserResponses
                .Where(r => r.UserId == userId)
                .Include(r => r.Question)
                .OrderBy(r => r.QuestionId)
                .ToListAsync();
        }

        public async Task<UserResponse?> GetResponseByIdAsync(int id)
        {
            return await _context.UserResponses
                .Include(r => r.Question)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task ExportResponsesToFile(string filePath)
        {
            var userId = GetCurrentUserId();
            var responses = await _context.UserResponses
                .Where(r => r.UserId == userId)
                .Include(r => r.Question)
                .OrderBy(r => r.QuestionId)
                .ToListAsync();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("RESPUESTAS DEL QUIZ TÉCNICO");
                await writer.WriteLineAsync("==========================");
                await writer.WriteLineAsync();

                foreach (var response in responses)
                {
                    await writer.WriteLineAsync($"Pregunta {response.QuestionId}:");
                    await writer.WriteLineAsync($"{response.Question.Text}");
                    await writer.WriteLineAsync("\nRespuesta:");
                    await writer.WriteLineAsync($"{response.Response}");
                    await writer.WriteLineAsync($"\nFecha: {response.CreatedAt:dd/MM/yyyy HH:mm:ss}");
                    if (response.ModifiedAt.HasValue)
                    {
                        await writer.WriteLineAsync($"Última modificación: {response.ModifiedAt.Value:dd/MM/yyyy HH:mm:ss}");
                    }
                    await writer.WriteLineAsync("\n" + new string('-', 50) + "\n");
                }
            }
        }

        
        public async Task<UserResponse> GetResponseByQuestionIdAndUserId(int questionId, string userId)
        {
            return await _context.UserResponses
                .Include(r => r.Question)  
                .FirstOrDefaultAsync(r => r.QuestionId == questionId && r.UserId == userId);
        }

        public async Task UpdateResponseAsync(UserResponse response)
        {
            // Si estamos actualizando, aseguramos que la pregunta esté cargada
            if (response.Question == null)
            {
                response.Question = await _context.Questions.FindAsync(response.QuestionId);
            }

            _context.UserResponses.Update(response);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Question>> GetQuestionsWithUserResponses(string userId)
        {
            return await _context.Questions
                .Include(q => q.UserResponses.Where(r => r.UserId == userId))
                .ToListAsync();
        }

    }
}

