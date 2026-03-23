using Tcc.Application.Dtos.ModelRequest;
using Tcc.Core.Entities;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IApprovalQueryRepository
{
    Task<(List<ApprovalEntity> Items, int TotalCount)> GetPagedAsync(
        GetApprovalListRequest request,
        CancellationToken cancellationToken);
}