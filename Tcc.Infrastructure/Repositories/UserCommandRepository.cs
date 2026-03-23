using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class UserCommandRepository : IUserCommandRepository
{
    private readonly AppDbContext _context;

    public UserCommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> CreateAsync(LoginRequest request)
    {
        var hasher = new PasswordHasher<UserEntity>();

        var user = new UserEntity
        {
            Username = request.Username
        };

        user.PasswordHash = hasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<UserEntity?> UpdateAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null)
            return null;

        var hasher = new PasswordHasher<UserEntity>();
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        await _context.SaveChangesAsync();
        return user;
    }
}