using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.Common;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Constants;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class ApprovalCommandRepository : IApprovalCommandRepository
{
    private readonly AppDbContext _context;

    public ApprovalCommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> ApproveAsync(ApproveApprovalRequest request, CancellationToken cancellationToken)
    {
        if (request.Ids == null || request.Ids.Count == 0)
            throw new BusinessException(ResultCode.ValidationError, "Please select at least one item.");

        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new BusinessException(ResultCode.ValidationError, "Reason is required.");

        var items = await _context.Approvals
            .Where(x => request.Ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            throw new BusinessException(ResultCode.DataNotFound, "Approval items not found.");

        var invalidItems = items
            .Where(x => x.StatusCode != ApprovalStatus.Pending.Code)
            .ToList();

        if (invalidItems.Count > 0)
            throw new BusinessException(ResultCode.BusinessError, "Only pending items can be approved.");

        foreach (var item in items)
        {
            item.StatusCode = ApprovalStatus.Approved.Code;
            item.ApprovedReason = request.Reason.Trim();
            item.RejectedReason = null;
            item.ActionBy = "SYSTEM";
            item.ActionDate = DateTime.UtcNow;
            item.UpdatedBy = "SYSTEM";
            item.UpdatedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return items.Count;
    }

    public async Task<int> RejectAsync(RejectApprovalRequest request, CancellationToken cancellationToken)
    {
        if (request.Ids == null || request.Ids.Count == 0)
            throw new BusinessException(ResultCode.ValidationError, "Please select at least one item.");

        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new BusinessException(ResultCode.ValidationError, "Reason is required.");

        var items = await _context.Approvals
            .Where(x => request.Ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            throw new BusinessException(ResultCode.DataNotFound, "Approval items not found.");

        var invalidItems = items
            .Where(x => x.StatusCode != ApprovalStatus.Pending.Code)
            .ToList();

        if (invalidItems.Count > 0)
            throw new BusinessException(ResultCode.BusinessError, "Only pending items can be rejected.");

        foreach (var item in items)
        {
            item.StatusCode = ApprovalStatus.Rejected.Code;
            item.RejectedReason = request.Reason.Trim();
            item.ApprovedReason = null;
            item.ActionBy = "SYSTEM";
            item.ActionDate = DateTime.UtcNow;
            item.UpdatedBy = "SYSTEM";
            item.UpdatedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return items.Count;
    }
}