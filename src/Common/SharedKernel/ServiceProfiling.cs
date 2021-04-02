using Microsoft.Extensions.Logging;
using SharedKernel.Intefaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SharedKernel
{
    public class ServiceProfiling<TDecorated, TService> : DispatchProxy
        where TService : class
    {
        private TDecorated _decorated;

        private ILogger<TService> _logger;

        private IValidatorHandler _validatorHandler;

        private string[] _filters = Array.Empty<string>();

        public static TDecorated CreateProxy(IServiceProvider provider)
        {
            object proxy = Create<TDecorated, ServiceProfiling<TDecorated, TService>>();

            ((ServiceProfiling<TDecorated, TService>)proxy).SetParameters(provider);

            return (TDecorated)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            string declaringType = $"{targetMethod.DeclaringType.Name}.{targetMethod.Name}";

            try
            {
                var newArgs = new List<object>(args);

                if (!_filters.Any(f => f.Equals(declaringType)))
                {
                    _logger.LogDebug("Start call to: {MethodName} with parameter {@newArgs}", targetMethod.Name, newArgs);
                }
            }
            catch
            {
            }
            var time = new Stopwatch();

            time.Start();

            Validate(args);

            var result = targetMethod.Invoke(_decorated, args);

            time.Stop();

            _logger.LogDebug($"End call to: {targetMethod.Name} ({time.ElapsedMilliseconds} milliseconds)");

            return result;
        }

        private void SetParameters(IServiceProvider provider)
        {
            Type type = typeof(TService);

            var args = GetArgs(provider, type.GetConstructors()[0]);

            _decorated = (TDecorated)Activator.CreateInstance(type, args);

            _logger = (ILogger<TService>)provider.GetService(typeof(ILogger<TService>));

            _validatorHandler = (IValidatorHandler)provider.GetService(typeof(IValidatorHandler));
        }

        private object[] GetArgs(IServiceProvider provider, ConstructorInfo constructor)
        {
            var args = new List<object>();

            var parameters = constructor?.GetParameters();

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    args.Add(provider.GetService(Type.GetType(parameter.ParameterType.AssemblyQualifiedName)));
                }
            }

            return args.ToArray();
        }

        private void Validate(object[] args)
        {
            foreach (var arg in args)
            {
                _validatorHandler.Validate(arg);
            }
        }
    }
}
