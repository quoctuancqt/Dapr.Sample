using SharedKernel.EventBus.Events;
using System.Threading.Tasks;

namespace SharedKernel.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event)
            where TIntegrationEvent : IntegrationEvent;
    }
}
