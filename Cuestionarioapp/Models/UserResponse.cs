using System.ComponentModel.DataAnnotations;

namespace Cuestionarioapp.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }  

        public string Response { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public virtual Question Question { get; set; } = null!;
    }
}