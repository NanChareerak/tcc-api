using System;
using System.Collections.Generic;
using System.Linq;

namespace Tcc.Core.Constants
{
    public class ApprovalStatus : IEquatable<ApprovalStatus>
    {
        public string Code { get; }
        public string Name { get; }

        private ApprovalStatus(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly ApprovalStatus Pending =
            new ApprovalStatus("PENDING", "รออนุมัติ");

        public static readonly ApprovalStatus Approved =
            new ApprovalStatus("APPROVED", "อนุมัติ");

        public static readonly ApprovalStatus Rejected =
            new ApprovalStatus("REJECTED", "ไม่อนุมัติ");

        public static readonly IReadOnlyList<ApprovalStatus> All =
            new[] { Pending, Approved, Rejected };

        public static ApprovalStatus FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code is required");

            var result = All.FirstOrDefault(x =>
                x.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));

            return result ?? throw new ArgumentException($"Invalid code: {code}");
        }

        public static ApprovalStatus FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            var result = All.FirstOrDefault(x =>
                x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

            return result ?? throw new ArgumentException($"Invalid name: {name}");
        }

        public bool Equals(ApprovalStatus? other)
        {
            if (other is null) return false;
            return Code == other.Code;
        }

        public override bool Equals(object? obj) => Equals(obj as ApprovalStatus);

        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString() => Code;
    }
}