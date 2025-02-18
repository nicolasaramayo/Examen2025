using System.ComponentModel.DataAnnotations;

namespace Cuestionarioapp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }

   
}
