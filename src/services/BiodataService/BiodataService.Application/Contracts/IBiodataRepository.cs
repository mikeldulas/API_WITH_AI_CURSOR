using BiodataService.Domain.Entities;

namespace BiodataService.Application.Contracts;

public interface IBiodataRepository
{
    Task<IReadOnlyList<Biodata>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Biodata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Biodata> AddAsync(Biodata biodata, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Biodata biodata, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
