using FluentValidation;

namespace CmdR.Validation
{
    public interface IValidatorLocator
    {
        IValidator GetValidator<T>();
    }
}