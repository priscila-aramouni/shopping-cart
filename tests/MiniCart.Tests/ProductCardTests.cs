using Bunit;
using Microsoft.AspNetCore.Components;
using MiniCart.Models;
using MiniCart.Shared;

namespace MiniCart.Tests
{
    public class ProductCardTests : TestContext
    {
        [Fact]
        public void Clicking_Add_Invokes_Callback_With_Product()
        {
            var product = new Product { Id = 1, Name = "Apple", Price = 1.00m };
            Product? received = null;

            var cut = RenderComponent<ProductCard>(p => p
                .Add(x => x.Product, product)
                .Add(x => x.AddToCart, EventCallback.Factory.Create<Product>(this, pr =>
                {
                    received = pr;
                }))
            );

            cut.Find("button").Click();

            Assert.NotNull(received);
            Assert.Equal(1, received!.Id);
            Assert.Equal("Apple", received!.Name);
            Assert.Equal(1.00m, received!.Price);

        }
    }
}
