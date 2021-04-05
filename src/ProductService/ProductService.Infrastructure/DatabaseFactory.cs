using AutoBogus;
using ProductService.Application.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Intefaces;
using System;
using System.Linq;

namespace ProductService.Infrastructure
{
    public class DatabaseFactory
    {
        private readonly IApplicationContext _context;

        public DatabaseFactory(IApplicationContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        public void SeedData()
        {
            var products = _context.GetDbSet<Product>();

            if (!products.Any())
            {
                var faker = new AutoFaker<Product>()
                                    .RuleFor(x => x.Id, fk => Guid.NewGuid().ToString())
                                    .RuleFor(x => x.Name, fk => fk.Commerce.Product())
                                    .RuleFor(x => x.Description, fk => fk.Commerce.ProductDescription())
                                    .RuleFor(x => x.CreatedAt, fk => DateTime.UtcNow)
                                    .RuleFor(x => x.CreatedBy, fk => "Administrator")
                                    .RuleFor(x => x.Price, fk => decimal.Parse(fk.Commerce.Price()));

                products.AddRange(faker.Generate(5));

                _context.CommitAsync().GetAwaiter().GetResult();
            }
        }
    }
}
