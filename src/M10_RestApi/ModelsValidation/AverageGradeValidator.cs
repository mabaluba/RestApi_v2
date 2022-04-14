using FluentValidation;
using M10_RestApi.ModelsDto;

namespace M10_RestApi.ModelsValidation
{
    public class AverageGradeValidator : AbstractValidator<AverageGradeDto>
    {
        public AverageGradeValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0);
            RuleFor(i => i.StudentAverageGrade).InclusiveBetween(0, 5.0);
        }
    }
}