using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public List<Product> products { get; set; }
    }
}
