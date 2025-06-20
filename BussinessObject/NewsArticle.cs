using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject;

public partial class NewsArticle
{
    public int NewsArticleId { get; set; }

    [Required(ErrorMessage = "Tiêu đề bài viết là bắt buộc")]
    [StringLength(500, ErrorMessage = "Tiêu đề không được vượt quá 500 ký tự")]
    public string Headline { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    [StringLength(4000, ErrorMessage = "Nội dung không được vượt quá 4000 ký tự")]
    public string? NewsContent { get; set; }

    public bool NewsStatus { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục hợp lệ")]
    public int CategoryId { get; set; }
    public int AccountId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual SystemAccount Account { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
