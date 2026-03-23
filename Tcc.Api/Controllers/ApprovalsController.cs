using Microsoft.AspNetCore.Mvc;
using Tcc.Application.Dtos.Common;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Dtos.ModelResponse;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Constants;
using Tcc.Core.Entities;

namespace Tcc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalsController : ControllerBase
{
    private readonly IApprovalQueryRepository _approvalQueryRepository;
    private readonly IApprovalCommandRepository _approvalCommandRepository;

    public ApprovalsController(
        IApprovalQueryRepository approvalQueryRepository,
        IApprovalCommandRepository approvalCommandRepository)
    {
        _approvalQueryRepository = approvalQueryRepository;
        _approvalCommandRepository = approvalCommandRepository;
    }

    [HttpPost("list")]
    [ProducesResponseType(typeof(ApiPagedResponse<ApprovalResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromBody] GetApprovalListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _approvalQueryRepository.GetPagedAsync(request, cancellationToken);
        var datas = result.Items.Select(MapToResponse).ToList();

        return Ok(ApiPagedResponse<ApprovalResponse>.Ok(
            datas,
            request.PageIndex,
            request.PageSize,
            result.TotalCount,
            "Get approval list success"
        ));
    }

    [HttpPost("approve")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(
        [FromBody] ApproveApprovalRequest request,
        CancellationToken cancellationToken)
    {
        var count = await _approvalCommandRepository.ApproveAsync(request, cancellationToken);

        return Ok(ApiResponse<int>.Ok(
            count,
            "Approve success",
            ResultCode.Updated
        ));
    }

    [HttpPost("reject")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(
        [FromBody] RejectApprovalRequest request,
        CancellationToken cancellationToken)
    {
        var count = await _approvalCommandRepository.RejectAsync(request, cancellationToken);

        return Ok(ApiResponse<int>.Ok(
            count,
            "Reject success",
            ResultCode.Updated
        ));
    }

    private static ApprovalResponse MapToResponse(ApprovalEntity entity)
    {
        var status = ApprovalStatus.FromCode(entity.StatusCode);

        return new ApprovalResponse
        {
            Id = entity.Id,
            ItemName = entity.ItemName,
            RequestReason = entity.RequestReason,
            StatusCode = status.Code,
            StatusName = status.Name,
            ApprovedReason = entity.ApprovedReason,
            RejectedReason = entity.RejectedReason,
            ActionBy = entity.ActionBy,
            ActionDate = entity.ActionDate,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,
            UpdatedBy = entity.UpdatedBy,
            UpdatedDate = entity.UpdatedDate,
            IsActive = entity.IsActive
        };
    }
}