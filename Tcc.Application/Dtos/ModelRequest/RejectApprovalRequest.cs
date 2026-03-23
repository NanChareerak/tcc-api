namespace Tcc.Application.Dtos.ModelRequest;

public class RejectApprovalRequest
{
    public List<int> Ids { get; set; } = new();
    public string Reason { get; set; } = string.Empty;
}