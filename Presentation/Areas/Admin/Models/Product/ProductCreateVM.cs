using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Models.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal StockQuantity { get; set; }
        [Required]
        public string PhotoName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
