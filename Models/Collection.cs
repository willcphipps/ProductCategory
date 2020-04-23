using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models {
    public class Collection {
        [Key]
        public int ColId { get; set; }
        public int ProdId { get; set; }
        public int CatId { get; set; }
        public Product ColProd { get; set; }
        public Category ColCat { get; set; }
    }
}