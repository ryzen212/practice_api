using System.ComponentModel.DataAnnotations;

namespace practice_api.Data
{
    public class Blogs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty ;
        
        public string Author { get; set; } = string.Empty;


    }
}
