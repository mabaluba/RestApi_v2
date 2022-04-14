using FluentValidation;
using M10_RestApi.ModelsDto;

namespace M10_RestApi.ModelsValidation
{
    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0);
            RuleFor(i => i.FirstName).NotEmpty();
            RuleFor(i => i.LastName).NotEmpty();
            RuleFor(i => i.Email).EmailAddress();
            RuleFor(i => i.PhoneNumber).Matches(@"^\d{3}-\d{3}-\d{4}$");
        }
    }
}