using FluentValidation;
using SharedKernel.Exceptions;
using System;

namespace SharedKernel.Intefaces
{
    public interface IValidatorHandler
    {
        void Validate(object model);
    }

    public class ValidatorHandler : IValidatorHandler
    {
        public virtual void Validate(object model)
        {
            if (model == null) return;

            Type objectType = model.GetType();

            string assemblyQualifiedName = $"{objectType.Namespace}.{objectType.Name}Validator, {objectType.Assembly}";

            Type type = Type.GetType(assemblyQualifiedName);

            if (type == null) return;

            var validator = (IValidator)Activator.CreateInstance(type);

            var context = new ValidationContext<object>(model);

            var result = validator.Validate(context);

            if (!result.IsValid)
            {
                throw new BadRequestException(result.Errors);
            }
        }
    }
}
