using Microsoft.AspNetCore.Mvc;
using Tcc.Application.Dtos.Common;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Dtos.ModelResponse;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;

namespace Tcc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonQueryRepository _personQueryRepository;
    private readonly IPersonCommandRepository _personCommandRepository;

    public PersonsController(
        IPersonQueryRepository personQueryRepository,
        IPersonCommandRepository personCommandRepository)
    {
        _personQueryRepository = personQueryRepository;
        _personCommandRepository = personCommandRepository;
    }

    [HttpPost("list")]
    [ProducesResponseType(typeof(ApiPagedResponse<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonList(
        [FromBody] GetPersonListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _personQueryRepository.GetPagedAsync(request, cancellationToken);
        var datas = result.Items.Select(MapToResponse).ToList();

        return Ok(ApiPagedResponse<PersonResponse>.Ok(
            datas,
            request.PageIndex,
            request.PageSize,
            result.TotalCount,
            "Get person list success"
        ));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PersonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ViewPerson(int id, CancellationToken cancellationToken)
    {
        var person = await _personQueryRepository.GetByIdAsync(id, cancellationToken);

        if (person == null)
        {
            return NotFound(ApiErrorResponse.Fail(
                ResultCode.DataNotFound,
                "Person not found"
            ));
        }

        return Ok(ApiResponse<PersonResponse>.Ok(
            MapToResponse(person),
            "Get person success"
        ));
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePerson(
        [FromBody] CreatePersonRequest request,
        CancellationToken cancellationToken)
    {
        var person = await _personCommandRepository.CreateAsync(request, cancellationToken);

        return Ok(ApiResponse<PersonResponse>.Ok(
            MapToResponse(person),
            "Create person success",
            ResultCode.Created
        ));
    }

    [HttpPost("update")]
    [ProducesResponseType(typeof(ApiResponse<PersonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePerson(
        [FromBody] UpdatePersonRequest request,
        CancellationToken cancellationToken)
    {
        var person = await _personCommandRepository.UpdateAsync(request, cancellationToken);

        if (person == null)
        {
            return NotFound(ApiErrorResponse.Fail(
                ResultCode.DataNotFound,
                "Person not found"
            ));
        }

        return Ok(ApiResponse<PersonResponse>.Ok(
            MapToResponse(person),
            "Update person success",
            ResultCode.Updated
        ));
    }

    private static PersonResponse MapToResponse(PersonEntity person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            FullName = $"{person.FirstName} {person.LastName}",
            DateOfBirth = person.DateOfBirth.ToString("yyyy-MM-dd"),
            Age = CalculateAge(person.DateOfBirth),
            Address = person.Address,
            CreatedBy = person.CreatedBy,
            CreatedDate = person.CreatedDate,
            UpdatedBy = person.UpdatedBy,
            UpdatedDate = person.UpdatedDate,
            IsActive = person.IsActive
        };
    }

    private static int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
            age--;

        return age;
    }
}