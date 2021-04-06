using SharedKernel;

namespace ProductService.Infrastructure
{
    public class DesignTimeDbContextFactory
        : ApplicationContextDesignTimeDbContextFactory<ProductContext>
    {
        public DesignTimeDbContextFactory() : base()
        {
        }
    }
}
