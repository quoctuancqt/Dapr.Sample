using SharedKernel;

namespace OrderService.Application.Entities
{
    public class Address : ValueObject
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
    }
}
