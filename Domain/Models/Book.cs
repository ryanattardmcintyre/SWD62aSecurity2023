using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{

    //What is the difference between declaring classes (with almost same names) in the Domain vs ViewModels (normally declared in website/businesslogic)
    //Take the example of a digest (hash) of any data...this is a property which one have declared in the Domain class but not in the ViewModel
    //reason: the end user on the View never needs to see the Digest of any data
    public class Book
    {
        [Key]
        public string Isbn { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Path { get; set; }

        public string Digest { get; set; }

        //public string SupplierAddress { get; set; }

        //public double OriginalPrice { get; set; }
    }
}
