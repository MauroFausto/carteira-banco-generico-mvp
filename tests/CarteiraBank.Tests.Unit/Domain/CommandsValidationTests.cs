using CarteiraBank.Domain.Commands;

namespace CarteiraBank.Tests.Unit.Domain;

public sealed class CommandsValidationTests
{
    [Fact]
    public void RegisterCreditRequestCommand_DeveInvalidarValorMenorOuIgualAZero()
    {
        var command = new RegisterCreditRequestCommand
        {
            ClienteId = Guid.NewGuid(),
            ValorSolicitado = 0,
            QuantidadeParcelasSolicitada = 1,
            Finalidade = "Teste"
        };

        var valido = command.IsValid();

        Assert.False(valido);
        Assert.NotEmpty(command.ValidationResult.Errors);
    }

    [Fact]
    public void CreateAgreementCommand_DeveValidarQuantidadeParcelas()
    {
        var command = new CreateAgreementCommand
        {
            ContratoId = Guid.NewGuid(),
            QuantidadeParcelas = 0,
            ValorDesconto = 10
        };

        Assert.False(command.IsValid());
    }
}
