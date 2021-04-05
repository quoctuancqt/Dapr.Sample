using SharedKernel;

namespace ProductService.Application.Dto
{
    public class ProductQuerySearch : BaseQuery
    {
        public new string[] SearchFields = new string[] { "Name" };
    }
}
