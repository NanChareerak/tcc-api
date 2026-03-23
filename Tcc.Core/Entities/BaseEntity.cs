using System;

namespace Tcc.Core.Entities
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; } = "SYSTEM";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}