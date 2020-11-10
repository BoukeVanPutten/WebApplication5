using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Product
    {
        // Attributes kunnen gebruikt worden voor modelvalidation
        // Er kan ook een eigen validator gebruikt worden, die zou dan als losse service kunnen bestaan

        [Key]
        [Required]
        public string ProductCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }        

        [Required]
        public float PotSizeInCentimeters { get; set; }

        [Required]
        public float PlantHeightInCentimeters { get; set; }

        public Colour Colour { get; set; }

        [Required]
        public ProductGroup ProductGroup { get; set; }
    }
}
