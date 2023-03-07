 
using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage ="isbn is required")]
        public string Isbn { get; set; }
      
     
        [Required(ErrorMessage = "from date is required")]
        public DateTime From { get; set; }

        [DateGreaterThanToValidator]
        public DateTime To { get; set; }
    }
}
