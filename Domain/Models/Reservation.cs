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

    //why would i choose the built-in authentication system over a custom one?

    //1. that system has been tested thousand of times by thousands of developers
    //2. it saves you a lot of time
    //3. can be easily integrated with an Oauth login system (e.g. Google login)

    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
       
        [ForeignKey("Book")]
        public string Isbn_Fk { get; set; }
        public virtual Book Book { get; set; } //navigational property

        [ForeignKey("User")]
        public string User_Fk { get; set; }
        public IdentityUser User { get; set; } //navigational property

        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
