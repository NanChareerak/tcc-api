using Tcc.Application.Dtos.ModelRequest;
using Tcc.Core.Entities;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IUserQueryRepository
{
    Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<UserEntity?> LoginAsync(LoginRequest request);
}