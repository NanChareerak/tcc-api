using Microsoft.EntityFrameworkCore;
using Tcc.Application.Dtos.ModelRequest;
using Tcc.Application.Interfaces.IRepositories;
using Tcc.Core.Entities;
using Tcc.Infrastructure.Data;

namespace Tcc.Infrastructure.Repositories;

public class PersonQueryRepository : IPersonQueryRepository
{
    private readonly AppDbContext _context;

    public PersonQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<PersonEntity> Items, int TotalCount)> GetPagedAsync(
        GetPersonListRequest request,
        CancellationToken cancellationToken)
    {
        var query = _context.Persons.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim();

            query = query.Where(x =>
                x.FirstName.Contains(keyword) ||
                x.LastName.Contains(keyword) ||
                ((x.FirstName + " " + x.LastName).Contains(keyword)) ||
                (x.Address != null && x.Address.Contains(keyword)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Id)
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<PersonEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}