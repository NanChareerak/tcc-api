using Tcc.Application.Dtos.ModelRequest;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IApprovalCommandRepository
{
    Task<int> ApproveAsync(ApproveApprovalRequest request, CancellationToken cancellationToken);
    Task<int> RejectAsync(RejectApprovalRequest request, CancellationToken cancellationToken);
}