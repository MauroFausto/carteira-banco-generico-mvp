using CarteiraBank.Domain.Core;
using FluentValidation;

namespace CarteiraBank.Domain.Commands;

public sealed class RegisterCreditRequestCommand : Command
{
    public Guid ClienteId { get; init; }
    public decimal ValorSolicitado { get; init; }
    public int QuantidadeParcelasSolicitada { get; init; }
    public string Finalidade { get; init; } = string.Empty;

    public override bool IsValid()
    {
        ValidationResult = new RegisterCreditRequestCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

public sealed class DecideCreditRequestCommand : Command
{
    public Guid SolicitacaoId { get; init; }
    public Guid SupervisorId { get; init; }
    public bool Aprovar { get; init; }
    public string? Motivo { get; init; }

    public override bool IsValid()
    {
        ValidationResult = new DecideCreditRequestCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

public sealed class CreateAgreementCommand : Command
{
    public Guid ContratoId { get; init; }
    public decimal ValorDesconto { get; init; }
    public int QuantidadeParcelas { get; init; }

    public override bool IsValid()
    {
        ValidationResult = new CreateAgreementCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

public sealed class IssueBilletCommand : Command
{
    public Guid ParcelaAcordoId { get; init; }

    public override bool IsValid()
    {
        ValidationResult = new IssueBilletCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}

public sealed class RegisterCreditRequestCommandValidator : AbstractValidator<RegisterCreditRequestCommand>
{
    public RegisterCreditRequestCommandValidator()
    {
        RuleFor(c => c.ClienteId).NotEmpty();
        RuleFor(c => c.ValorSolicitado).GreaterThan(0);
        RuleFor(c => c.QuantidadeParcelasSolicitada).GreaterThan(0);
        RuleFor(c => c.Finalidade).NotEmpty().MaximumLength(300);
    }
}

public sealed class DecideCreditRequestCommandValidator : AbstractValidator<DecideCreditRequestCommand>
{
    public DecideCreditRequestCommandValidator()
    {
        RuleFor(c => c.SolicitacaoId).NotEmpty();
        RuleFor(c => c.SupervisorId).NotEmpty();
    }
}

public sealed class CreateAgreementCommandValidator : AbstractValidator<CreateAgreementCommand>
{
    public CreateAgreementCommandValidator()
    {
        RuleFor(c => c.ContratoId).NotEmpty();
        RuleFor(c => c.QuantidadeParcelas).GreaterThan(0);
        RuleFor(c => c.ValorDesconto).GreaterThanOrEqualTo(0);
    }
}

public sealed class IssueBilletCommandValidator : AbstractValidator<IssueBilletCommand>
{
    public IssueBilletCommandValidator()
    {
        RuleFor(c => c.ParcelaAcordoId).NotEmpty();
    }
}
