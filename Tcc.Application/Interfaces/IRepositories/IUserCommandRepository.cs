using Tcc.Application.Dtos.ModelRequest;
using Tcc.Core.Entities;

namespace Tcc.Application.Interfaces.IRepositories;

public interface IUserCommandRepository
{
    Task<UserEntity> CreateAsync(LoginRequest request);
    Task<UserEntity?> UpdateAsync(LoginRequest request);
}