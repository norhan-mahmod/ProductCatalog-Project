using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductTask.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
