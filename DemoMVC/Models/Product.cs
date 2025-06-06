using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string Name { get; set; } 
        [Range(0.01, 1000000, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; } 
        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public string Category { get; set; } 
    }
}
