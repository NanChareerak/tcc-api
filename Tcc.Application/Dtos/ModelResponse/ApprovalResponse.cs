namespace Tcc.Application.Dtos.ModelResponse;

public class ApprovalResponse
{
    public int Id { get; set; }

    public string ItemName { get; set; } = string.Empty;
    public string? RequestReason { get; set; }

    public string StatusCode { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;

    public string? ApprovedReason { get; set; }
    public string? RejectedReason { get; set; }

    public string? ActionBy { get; set; }
    public DateTime? ActionDate { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }
}