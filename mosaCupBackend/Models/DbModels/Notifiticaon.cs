using System.ComponentModel.DataAnnotations;

namespace mosaCupBackend.Models.DbModels
{
    public class notification
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public string AffectUid { get; set; }
        // Follow -> 0 / Like -> 1
        [Required]
        public int TypeCode { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Guid? Pid { get; set; }
    }
}
