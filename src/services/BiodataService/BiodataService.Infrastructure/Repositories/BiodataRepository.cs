using BiodataService.Application.Contracts;
using BiodataService.Domain.Entities;
using BiodataService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BiodataService.Infrastructure.Repositories;

public class BiodataRepository(BiodataDbContext dbContext) : IBiodataRepository
{
    public async Task<IReadOnlyList<Biodata>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Biodata
            .AsNoTracking()
            .OrderBy(x => x.Nama)
            .ToListAsync(cancellationToken);
    }

    public Task<Biodata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Biodata.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Biodata> AddAsync(Biodata biodata, CancellationToken cancellationToken = default)
    {
        dbContext.Biodata.Add(biodata);
        await dbContext.SaveChangesAsync(cancellationToken);
        return biodata;
    }

    public async Task<bool> UpdateAsync(Biodata biodata, CancellationToken cancellationToken = default)
    {
        dbContext.Biodata.Update(biodata);
        var affected = await dbContext.SaveChangesAsync(cancellationToken);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Biodata.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        dbContext.Biodata.Remove(existing);
        var affected = await dbContext.SaveChangesAsync(cancellationToken);
        return affected > 0;
    }
}
