using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book
    {
        [Key]
        public string Isbn { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Path { get; set; }

        //public string SupplierAddress { get; set; }

        //public double OriginalPrice { get; set; }
    }
}
