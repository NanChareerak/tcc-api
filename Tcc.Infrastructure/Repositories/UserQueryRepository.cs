using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly AppDbContext _context;

    public UserQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Username == username);

        Console.WriteLine(user == null ? "User not found" : $"Found: {user.Username}");
        return user;
    }

    public async Task<UserEntity?> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var user = await GetByUsernameAsync(request.Username.Trim());

        if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
            return null;

        var hasher = new PasswordHasher<UserEntity>();

        try
        {
            var result = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password.Trim()
            );

            if (result == PasswordVerificationResult.Success)
                return user;

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = hasher.HashPassword(user, request.Password);
                await _context.SaveChangesAsync();
                return user;
            }

            return null;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid password format");
            return null;
        }
    }

}