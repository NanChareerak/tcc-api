namespace Tcc.Application.Dtos.ModelRequest;

public class ApproveApprovalRequest
{
    public List<int> Ids { get; set; } = new();
    public string Reason { get; set; } = string.Empty;
}