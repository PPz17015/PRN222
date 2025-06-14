using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects;

public partial class AccountMember
{
    [Key]
    [StringLength(50)]
    public string MemberId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MemberPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    public int MemberRole { get; set; }
}
