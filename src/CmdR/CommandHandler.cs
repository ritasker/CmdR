namespace CmdR
{
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Validation;

    public abstract class CommandHandler<TCommand> : ICommandHandler
    {
        protected CommandHandler(string path)
        {
            Path = path;
        }

        public Task HandleRequest(HttpContext context)
        {
            Context = context;
            var command = GetCommand(context);
            var validator = GetValidator(context);
            
            var result = validator.Validate(command);

            if (result.IsValid)
            {
                return Handle(command);   
            }

            context.Response.StatusCode = 422;
            context.Response.Headers.Add("content-type", "application/json");
            return context.Response.WriteAsync(FormatErrors(result.Errors));
        }

        private string FormatErrors(IEnumerable<ValidationFailure> validationFailures)
        {
            var validationResultModel = new ValidationResultModel();
            
            var errors = new Dictionary<string, List<string>>();

            foreach (var failure in validationFailures)
            {
                if(!errors.TryGetValue(failure.PropertyName, out var error))
                {
                    errors.Add(failure.PropertyName, new List<string>{failure.ErrorMessage});
                }
                else
                {
                    error.Add(failure.ErrorMessage);
                }
            }

            validationResultModel.Errors = errors;
            return JsonSerializer.Serialize(validationResultModel);
        }

        private static IValidator GetValidator(HttpContext context)
        {
            var validatorLocator = context.RequestServices.GetService<IValidatorLocator>();
            return validatorLocator.GetValidator<TCommand>();
        }

        private static TCommand GetCommand(HttpContext context)
        {
            
            return JsonSerializer.Deserialize<TCommand>(AsString(context.Request.BodyReader.AsStream()));
        }

        protected abstract Task Handle(TCommand command);

        public string Path { get; }
        protected HttpContext Context { get; private set; }


        private static string AsString(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEndAsync().GetAwaiter().GetResult();
            }
        }
    }
}