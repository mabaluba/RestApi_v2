namespace M10_RestApi.ModelsValidation;

using FluentValidation;
using ModelsDto;

public class StudentFirstLastNameDtoValidator : AbstractValidator<StudentFirstLastNameDto>
{
    public StudentFirstLastNameDtoValidator()
    {
        RuleFor(i => i.FirstName).NotEmpty().WithMessage(i => $"{nameof(i.FirstName)} cannot be null or whitespace.");
        RuleFor(i => i.LastName).NotEmpty().WithMessage(i => $"{nameof(i.LastName)} cannot be null or whitespace.");
    }
}