using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LegalDocument
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; }
        public string Name { get; set; }

        public string Signature { get; set; }

        public string User_Id_Fk { get; set; } //this is very important, because using this fk,
                                               //later when the file will be requested to download you need
                                               //to retrieve the public key belonging to the user having this user_id

    }
}
