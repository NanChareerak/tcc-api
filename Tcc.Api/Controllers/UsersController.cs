using Microsoft.AspNetCore.Mvc;
using Tcc.Application.Dtos.Common;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Dtos.ModelResponse;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;

namespace Tcc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandRepository _userCommandRepository;

    public UsersController(
        IUserQueryRepository userQueryRepository,
        IUserCommandRepository userCommandRepository)
    {
        _userQueryRepository = userQueryRepository;
        _userCommandRepository = userCommandRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userQueryRepository.GetAllAsync(cancellationToken);
        var response = users.Select(MapToResponse).ToList();

        return Ok(ApiResponse<List<UserResponse>>.Ok(response, "Get users success"));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userQueryRepository.LoginAsync(request);

        if (user == null)
        {
            return Unauthorized(
                ApiErrorResponse.Fail(
                    ResultCode.InvalidCredentials,
                    "Invalid username or password"
                )
            );
        }

        return Ok(ApiResponse<UserResponse>.Ok(MapToResponse(user), "Login success"));
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] LoginRequest request)
    {
        var user = await _userCommandRepository.CreateAsync(request);

        return Ok(ApiResponse<UserResponse>.Ok(
            MapToResponse(user),
            "Create user success",
            ResultCode.Created
        ));
    }

    [HttpPost("update")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] LoginRequest request)
    {
        var user = await _userCommandRepository.UpdateAsync(request);

        if (user == null)
        {
            return NotFound(
                ApiErrorResponse.Fail(
                    ResultCode.DataNotFound,
                    "User not found"
                )
            );
        }

        return Ok(ApiResponse<UserResponse>.Ok(
            MapToResponse(user),
            "Update user success",
            ResultCode.Updated
        ));
    }

    private static UserResponse MapToResponse(UserEntity user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}