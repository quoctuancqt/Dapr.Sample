using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public BadRequestException(string message)
            : this()
        {
            Failures.Add("message", new string[] { message });
        }

        public BadRequestException(object obj)
            : this()
        {
            var props = obj.GetType().GetProperties();

            foreach (var prop in props)
            {
                string value = (string)obj.GetType().GetProperty(prop.Name).GetValue(obj);

                Failures.Add(prop.Name, new string[] { value });
            }
        }

        public BadRequestException(IList<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }
}
