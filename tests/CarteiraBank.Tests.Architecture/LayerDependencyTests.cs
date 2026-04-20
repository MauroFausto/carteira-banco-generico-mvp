using NetArchTest.Rules;

namespace CarteiraBank.Tests.Architecture;

public sealed class LayerDependencyTests
{
    [Fact]
    public void DomainNaoDeveDependerDeInfraOuApi()
    {
        var result = Types.InAssembly(typeof(CarteiraBank.Domain.Models.Cliente).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("CarteiraBank.Infra", "CarteiraBank.Services.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationNaoDeveDependerDaApi()
    {
        var result = Types.InAssembly(typeof(CarteiraBank.Application.Services.ClienteAppService).Assembly)
            .ShouldNot()
            .HaveDependencyOn("CarteiraBank.Services.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
