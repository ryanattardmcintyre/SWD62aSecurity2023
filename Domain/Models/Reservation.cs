using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Domain.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
       
        [ForeignKey("Book")]
        public string Isbn_Fk { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey("User")]
        public string User_Fk { get; set; }
        public IdentityUser User { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
