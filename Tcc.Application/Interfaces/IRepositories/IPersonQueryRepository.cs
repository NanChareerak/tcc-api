using Tcc.Application.Dtos.ModelRequest;
using Tcc.Core.Entities;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IPersonQueryRepository
{
    Task<(List<PersonEntity> Items, int TotalCount)> GetPagedAsync(GetPersonListRequest request, CancellationToken cancellationToken);
    Task<PersonEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
}