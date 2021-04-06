using SharedKernel;

namespace OrderService.Infrastructure
{
    public class DesignTimeDbContextFactory
        : ApplicationContextDesignTimeDbContextFactory<OrderContext>
    {
        public DesignTimeDbContextFactory() : base("OrderConnectionString")
        {
        }
    }
}
