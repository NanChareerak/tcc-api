using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tcc.Core.Constants;

namespace Tcc.Core.Entities
{
    [Table("Approval")]
    public class ApprovalEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? RequestReason { get; set; }

        [Required]
        [MaxLength(20)]
        public string StatusCode { get; set; } = ApprovalStatus.Pending.Code;

        [MaxLength(1000)]
        public string? ApprovedReason { get; set; }

        [MaxLength(1000)]
        public string? RejectedReason { get; set; }

        [MaxLength(100)]
        public string? ActionBy { get; set; }

        public DateTime? ActionDate { get; set; }
    }
}