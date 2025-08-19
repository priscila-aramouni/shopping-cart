using Bunit;
using MiniCart.Models;
using MiniCart.Shared;
using Microsoft.AspNetCore.Components; // Cart.razor

namespace MiniCart.Tests
{
    public class CartComponentTests : TestContext
    {
        [Fact]
        public void Shows_Empty_Message_When_No_Items()
        {
            var cut = RenderComponent<Cart>(p => p.Add(x => x.CartItems, new List<Product>()));

            Assert.Contains("Your cart is empty.", cut.Markup);
        }


        [Fact]
        public void Renders_Items_And_Total_When_Items_Exist()
        {
            var items = new List<Product>
    {
        new Product { Id = 1, Name = "Apple",  Price = 1.00m },
        new Product { Id = 2, Name = "Orange", Price = 0.75m },
    };

            var cut = RenderComponent<Cart>(p => p.Add(x => x.CartItems, items));

            // Items present
            var liTexts = cut.FindAll("li").Select(li => li.TextContent.Trim()).ToList();
            Assert.Contains(liTexts, t => t.Contains("Apple - $1.00"));
            Assert.Contains(liTexts, t => t.Contains("Orange - $0.75"));

            // Total
            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$1.75", cut.Markup);
        }


        [Fact]
        public void Clicking_Remove_Invokes_Callback_With_Selected_Item()
        {
            var items = new List<Product> { new Product { Id = 1, Name = "Apple", Price = 1.00m } };
            Product? removed = null;

            var cut = RenderComponent<Cart>(p => p
                .Add(x => x.CartItems, items)
                .Add(x => x.RemoveFromCart,
                     EventCallback.Factory.Create<Product>(this, pr => removed = pr))
            );

            cut.Find("button").Click();
            Assert.NotNull(removed);
            Assert.Equal(1, removed!.Id);
        }
    }
}
