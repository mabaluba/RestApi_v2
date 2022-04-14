using FluentValidation;
using M10_RestApi.ModelsDto;

namespace M10_RestApi.ModelsValidation
{
    public class AttendancePostValidator : AbstractValidator<AttendancePostDto>
    {
        public AttendancePostValidator()
        {
            RuleFor(i => i.LectureTopic).NotEmpty();
            RuleFor(i => i.StudentFirstName).NotEmpty();
            RuleFor(i => i.StudentLastName).NotEmpty();
            RuleFor(i => i.IsAttended).NotNull();
            RuleFor(i => i.HomeworkMark).NotNull().InclusiveBetween(0, 5);
        }
    }
}