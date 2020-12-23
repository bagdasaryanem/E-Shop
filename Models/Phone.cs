using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop.Models
{
    public class Phone
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        public string DisplaySize { get; set; }
        [Required]
        public string Processor { get; set; }
        [Required]
        public int Memory { get; set; }
        [Required]
        public string OperatingSystem { get; set; }
        [Required(ErrorMessage = "Provide Price")]
        [Range(1, 999999999, ErrorMessage = "Enter valid price")]
        public int Price { get; set; }
        [Required]
        public string Photos { get; set; }
    }
}
