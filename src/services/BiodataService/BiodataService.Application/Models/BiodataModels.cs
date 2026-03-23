namespace BiodataService.Application.Models;

public record BiodataRequest(string Nama, DateOnly TanggalLahir, string Alamat, string NoHp);

public record BiodataResponse(
    Guid Id,
    string Nama,
    DateOnly TanggalLahir,
    string Alamat,
    string NoHp,
    DateTime CreatedAt,
    DateTime UpdatedAt);
