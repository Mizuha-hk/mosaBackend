using System.ComponentModel.DataAnnotations;

namespace mosaCupBackend.Models.DbModels
{
    public class post
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }

        public int? ReplyId { get; set; } = null;
        [Required]
        public int JoyLevel { get; set; }
    }
}
