using BiodataService.Application.Contracts;
using BiodataService.Application.Models;
using BiodataService.Domain.Entities;

namespace BiodataService.Application.Services;

public class BiodataAppService(IBiodataRepository repository) : IBiodataService
{
    public async Task<IReadOnlyList<BiodataResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(cancellationToken);
        return result.Select(MapToResponse).ToList();
    }

    public async Task<BiodataResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(id, cancellationToken);
        return result is null ? null : MapToResponse(result);
    }

    public async Task<BiodataResponse> CreateAsync(BiodataRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var entity = new Biodata
        {
            Id = Guid.NewGuid(),
            Nama = request.Nama,
            TanggalLahir = request.TanggalLahir,
            Alamat = request.Alamat,
            NoHp = request.NoHp,
            CreatedAt = now,
            UpdatedAt = now
        };

        var created = await repository.AddAsync(entity, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<bool> UpdateAsync(Guid id, BiodataRequest request, CancellationToken cancellationToken = default)
    {
        var current = await repository.GetByIdAsync(id, cancellationToken);
        if (current is null)
        {
            return false;
        }

        current.Nama = request.Nama;
        current.TanggalLahir = request.TanggalLahir;
        current.Alamat = request.Alamat;
        current.NoHp = request.NoHp;
        current.UpdatedAt = DateTime.UtcNow;

        return await repository.UpdateAsync(current, cancellationToken);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }

    private static BiodataResponse MapToResponse(Biodata entity)
    {
        return new BiodataResponse(
            entity.Id,
            entity.Nama,
            entity.TanggalLahir,
            entity.Alamat,
            entity.NoHp,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
