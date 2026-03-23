using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class ApprovalQueryRepository : IApprovalQueryRepository
{
    private readonly AppDbContext _context;

    public ApprovalQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ApprovalEntity> Items, int TotalCount)> GetPagedAsync(
        GetApprovalListRequest request,
        CancellationToken cancellationToken)
    {
        var query = _context.Approvals.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim();

            query = query.Where(x =>
                x.ItemName.Contains(keyword) ||
                (x.RequestReason != null && x.RequestReason.Contains(keyword)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Id)
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}