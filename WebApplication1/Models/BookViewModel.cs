

using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class BookViewModel
    {
        [Required(ErrorMessage ="Input Isbn")]
        [RegularExpression("(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)", ErrorMessage ="Isbn is not valid")]
        public string Isbn { get; set; }

        [Required(ErrorMessage = "Input Name")]
        [StringLength(150, ErrorMessage ="Maximum 150 characters in length")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input Year")]
        [YearCustomValidator]
        public int Year { get; set; }

        public string Path { get; set; }
    }
}
