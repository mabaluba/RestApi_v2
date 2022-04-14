using FluentValidation;
using M10_RestApi.ModelsDto;

namespace M10_RestApi.ModelsValidation
{
    public class LecturePostValidator : AbstractValidator<LecturePostDto>
    {
        public LecturePostValidator()
        {
            RuleFor(i => i.Topic).NotEmpty();
            RuleFor(i => i.Date).NotEmpty().Must(date => date > new System.DateTime(2022, 1, 1, 0, 0, 0))
                .WithMessage("Date must be greater than 2021-01-01.");
            RuleFor(i => i.TeacherId).GreaterThanOrEqualTo(0);
        }
    }
}