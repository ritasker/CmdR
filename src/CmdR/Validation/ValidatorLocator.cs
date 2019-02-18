using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace CmdR.Validation
{
    public class ValidatorLocator : IValidatorLocator
    {
        private readonly IEnumerable<IValidator> validators;
        private readonly ConcurrentDictionary<Type, IValidator> foundValidators = new ConcurrentDictionary<Type, IValidator>();

        public ValidatorLocator(IEnumerable<IValidator> validators) => this.validators = validators;
        
        public IValidator GetValidator<T>() => foundValidators.GetOrAdd(typeof(T), FindValidator);
        
        private IValidator FindValidator(Type type)
        {
            var fullType = CreateValidatorType(type);

            var available = validators
                .Where(validator => fullType.GetTypeInfo().IsInstanceOfType(validator))
                .ToArray();

            if (available.Length > 1)
            {
                var names = string.Join(", ", available.Select(v => v.GetType().Name));
                var message = string.Concat(
                    "Ambiguous choice between multiple validators for type ",
                    type.Name,
                    ". The validators available are: ",
                    names);

                throw new InvalidOperationException(message);
            }

            return available.FirstOrDefault();
        }

        private static Type CreateValidatorType(Type type)
        {
            return typeof(AbstractValidator<>).MakeGenericType(type);
        }
    }
}