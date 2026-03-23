using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class PersonCommandRepository : IPersonCommandRepository
{
    private readonly AppDbContext _context;

    public PersonCommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PersonEntity> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var entity = new PersonEntity
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Address = request.Address?.Trim(),
            CreatedBy = "SYSTEM"
        };

        _context.Persons.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<PersonEntity?> UpdateAsync(UpdatePersonRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Persons
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            return null;

        entity.FirstName = request.FirstName.Trim();
        entity.LastName = request.LastName.Trim();
        entity.DateOfBirth = request.DateOfBirth;
        entity.Address = request.Address?.Trim();
        entity.UpdatedBy = "SYSTEM";

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}