using FluentValidation;

namespace chatbot.Application.Commands.AddMessage;

public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
{
    public AddMessageCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty()
            .WithMessage("ID do chat é obrigatório");

        RuleFor(x => x.Sender)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Remetente é obrigatório e não pode exceder 100 caracteres");

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Conteúdo é obrigatório e não pode exceder 2000 caracteres");
    }
} 