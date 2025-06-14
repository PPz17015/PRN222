using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(40)]
    public string ProductName { get; set; }

    public int CategoryId { get; set; }

    public int? UnitStock { get; set; }

    [Column(TypeName = "money")]
    public decimal? UnitPrice { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}
