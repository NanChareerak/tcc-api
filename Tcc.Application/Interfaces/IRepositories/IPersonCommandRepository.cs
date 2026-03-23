using Tcc.Application.Dtos.ModelRequest;
using Tcc.Core.Entities;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IPersonCommandRepository
{
    Task<PersonEntity> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken);
    Task<PersonEntity?> UpdateAsync(UpdatePersonRequest request, CancellationToken cancellationToken);
}