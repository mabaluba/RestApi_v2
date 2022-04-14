using FluentValidation;
using M10_RestApi.ModelsDto;

namespace M10_RestApi.ModelsValidation
{
    public class AttendanceValidator : AbstractValidator<AttendanceDto>
    {
        public AttendanceValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0);
            RuleFor(i => i.LectureTopic).NotEmpty();
            RuleFor(i => i.StudentFirstName).NotEmpty();
            RuleFor(i => i.StudentLastName).NotEmpty();
            RuleFor(i => i.IsAttended).NotNull();
            RuleFor(i => i.HomeworkMark).NotNull().InclusiveBetween(0, 5);
        }
    }
}