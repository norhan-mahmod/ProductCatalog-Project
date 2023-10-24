using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CreatedByUserId { get; set; }
    }
}
