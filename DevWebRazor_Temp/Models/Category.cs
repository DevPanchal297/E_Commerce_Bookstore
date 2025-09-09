using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevWebRazor_Temp.Models
{
    public class Category
    {
        [Key]  // Data Annotation to specify primary key   
        public int Id { get; set; }
        [Required]  // Data Annotation to specify not null
        [MaxLength(100, ErrorMessage = "Length Cannot be greater than 100")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Disply Order should be In between 1 to 100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
