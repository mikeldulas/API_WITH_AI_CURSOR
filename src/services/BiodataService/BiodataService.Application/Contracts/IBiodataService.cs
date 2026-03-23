using BiodataService.Application.Models;

namespace BiodataService.Application.Contracts;

public interface IBiodataService
{
    Task<IReadOnlyList<BiodataResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BiodataResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BiodataResponse> CreateAsync(BiodataRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, BiodataRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
