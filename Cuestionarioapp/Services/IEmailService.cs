using Cuestionarioapp.Models;

namespace Cuestionarioapp.Services
{
    public interface IEmailService
    {
        Task SendResponseEmailAsync(string to, UserResponse response);
    }
}
