using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API9.Authenticate
{
    public class Course_Details
    {
        [Key]
        public int Course_Id { get; set; }
        [Required]
        public string Coursecategory { get; set; }


        public string Description { get; set; }
        [Required]
        public DateTime Coursestartdate { get; set; }
        [Required]
        public string Format { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
