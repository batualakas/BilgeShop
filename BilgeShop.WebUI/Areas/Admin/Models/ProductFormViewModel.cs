using System.ComponentModel.DataAnnotations;

namespace BilgeShop.WebUI.Areas.Admin.Models
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ürün Adı girmek zorunlu.")]
        [Display(Name="Ürün Adı")] 
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitInStock { get; set; }

        [Display(Name="Ürün Görseli")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Bir kategori seçmek zorunlu.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

    }
}
