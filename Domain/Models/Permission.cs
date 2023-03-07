using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string User_Fk { get; set; }
        public IdentityUser User { get; set; } //navigational property

        public Book Book { get; set; } //navigational property
        
        [ForeignKey("Book")]
        public string Isbn_Fk { get; set; }
    }
}
