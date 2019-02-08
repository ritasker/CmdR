using FluentValidation;

namespace BasicSample.Features.Echo
{
    public class EchoValidator : AbstractValidator<Echo>
    {
        public EchoValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}