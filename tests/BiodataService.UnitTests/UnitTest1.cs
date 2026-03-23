using BiodataService.Application.Contracts;
using BiodataService.Application.Models;
using BiodataService.Application.Services;
using BiodataService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BiodataService.UnitTests;

public class BiodataServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateNewBiodata()
    {
        var repository = new Mock<IBiodataRepository>();
        repository.Setup(x => x.AddAsync(It.IsAny<Biodata>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Biodata entity, CancellationToken _) => entity);

        var service = new BiodataAppService(repository.Object);
        var request = new BiodataRequest("Mikel", new DateOnly(2000, 1, 1), "Bandung", "08123456789");

        var result = await service.CreateAsync(request);

        result.Nama.Should().Be("Mikel");
        result.Id.Should().NotBe(Guid.Empty);
        repository.Verify(x => x.AddAsync(It.IsAny<Biodata>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenEntityMissing()
    {
        var repository = new Mock<IBiodataRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Biodata?)null);

        var service = new BiodataAppService(repository.Object);
        var request = new BiodataRequest("Mikel", new DateOnly(2000, 1, 1), "Bandung", "08123456789");

        var result = await service.UpdateAsync(Guid.NewGuid(), request);

        result.Should().BeFalse();
        repository.Verify(x => x.UpdateAsync(It.IsAny<Biodata>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
