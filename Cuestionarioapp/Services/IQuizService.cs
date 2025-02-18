using Cuestionarioapp.Models;

namespace Cuestionarioapp.Services
{
    public interface IQuizService
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<UserResponse> SaveResponseAsync(UserResponse response);
        Task<List<UserResponse>> GetUserResponsesAsync();
        Task<UserResponse?> GetResponseByIdAsync(int id);

        Task ExportResponsesToFile(string filePath);

        Task<UserResponse> GetResponseByQuestionIdAndUserId(int questionId, string userId);

        Task UpdateResponseAsync(UserResponse response);


        Task<List<Question>> GetQuestionsWithUserResponses(string userId);



    }
}
