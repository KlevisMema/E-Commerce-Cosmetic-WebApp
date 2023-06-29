using CosmeticWeb.Data;
using CosmeticWeb.Models;

namespace CosmeticWeb.Seeders
{
    public static class OrdersSeeds
    {
        public static async Task SeedOrders(IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            _context!.Database.EnsureCreated();

            if (!_context.Products!.Any())
            {
                await _context.Products!.AddRangeAsync(new List<Product>()
                {
                    new Product
                    {
                         Id = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56eeb1d0"),
                         Name = "test",
                         Price = 432.0M,
                         PreviousPrice = 123.00M,
                         Description =  "sdsdsd",
                         Rating = 5,
                         CreatedAt = DateTime.Now,
                         ModifiedAt = DateTime.Now,
                         Image = "the-red-version-of-s23-ultra-is-more-like-v0-dJYvPBy0tuhsx4OGfXvcPdNGpKP6sRokRk_jyj6hFaU (1)231821257.jpg",
                         CategoryId = Guid.Parse("a00bf730-2b0a-4654-a1b7-62dc0b2d9cc3")
                    },
                    new Product
                    {
                         Id = Guid.Parse("6dbcb6d2-d785-419f-8ccb-be05a13784c2"),
                         Name = "test",
                         Price = 12.0M,
                         PreviousPrice = 11.00M,
                         Description =  "sdsdsd",
                         Rating = 5,
                         CreatedAt = DateTime.Now,
                         ModifiedAt = DateTime.Now,
                         Image = "the-red-version-of-s23-ultra-is-more-like-v0-dJYvPBy0tuhsx4OGfXvcPdNGpKP6sRokRk_jyj6hFaU (1)231821257.jpg",
                         CategoryId = Guid.Parse("a00bf730-2b0a-4654-a1b7-62dc0b2d9cc3")
                    },
                });
            }

            if (!_context.OrderItems!.Any())
            {
                await _context.OrderItems!.AddRangeAsync(new List<OrderItem>()
                {
                     new OrderItem
                     {
                        Id = Guid.Parse("93984830-ff4f-4b7e-bc77-bc55577cf037"),
                        Price = 123.3M,
                        Quantity = 3,
                        OrderId = Guid.Parse("50e70d81-aba7-46a7-a538-5582f4652ee2") ,
                        ProductId = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56eeb1d0"),
                     },
                     new OrderItem
                     {
                        Id = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56eeb1d1"),
                        Price = 145.3M,
                        Quantity = 4,
                        OrderId = Guid.Parse("9724d458-a598-4acf-90d3-d8484d4b0cd3") ,
                        ProductId = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56eeb1d0"),
                     },
                     new OrderItem
                     {
                        Id = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56ceb1d0"),
                        Price = 12.00M,
                        Quantity = 13,
                        OrderId = Guid.Parse("9724d458-a598-4acf-90d3-d8484d4b0cd2") ,
                        ProductId = Guid.Parse("1acc0874-e48d-4a3e-8cfc-37ca56eeb1d0"),
                     },
                });
            }

            //if (!_context.Orders!.Any())
            //{
            //    await _context.Orders!.AddRangeAsync(new List<Order>()
            //    {
            //        new Order
            //        {
            //            Id = Guid.Parse("be85e9ee-6ce4-40ef-aecc-521a15334205"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("5/23/2022 3:25:18 PM")
            //        },
            //        new Order
            //        {
            //            Id = Guid.Parse("50e70d81-aba7-46a7-a538-5582f4652ee2"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("5/23/2022 3:25:18 PM")
            //        },
            //        new Order
            //        {
            //            Id = Guid.Parse("57181b38-733c-41c6-9041-7b4e485095a0"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("5/23/2022 3:25:18 PM")
            //        },
            //        new Order
            //        {
            //            Id = Guid.Parse("38537166-fbf8-44be-ab7b-ab29e4be1146"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("5/23/2022 3:25:18 PM")
            //        },
            //        new Order
            //        {
            //            Id = Guid.Parse("9724d458-a598-4acf-90d3-d8484d4b0cd2"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("5/23/2023 3:25:18 PM")
            //        },
            //        new Order
            //        {
            //            Id = Guid.Parse("9724d458-a598-4acf-90d3-d8484d4b0cd3"),
            //            CustomerEmail = "testemail@email.com",
            //            CustomerAddress = "test addr",
            //            CustomerPhone = "1234567",
            //            CustomerName = "test  name",
            //            CreatedDate = DateTime.Parse("4/23/2023 12:00:00 AM")
            //        },
            //    });

            //    await _context.SaveChangesAsync();
            //}
        }
    }
}