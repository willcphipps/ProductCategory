using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models {
    public class Category {
        [Key]
        public int CatId { get; set; }
        public string CatName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Collection> CategorySelections { get; set; }
    }
}