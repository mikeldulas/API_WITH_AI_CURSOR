namespace BiodataService.Domain.Entities;

public class Biodata
{
    public Guid Id { get; set; }
    public string Nama { get; set; } = string.Empty;
    public DateOnly TanggalLahir { get; set; }
    public string Alamat { get; set; } = string.Empty;
    public string NoHp { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
