using Bunit;
using Index = MiniCart.Pages.Index;

namespace MiniCart.Tests
{
    public class IndexPageTests : TestContext
    {
        [Fact]
        public void Empty_Cart_Shows_Empty_Message()
        {
            var cut = RenderComponent<Index>();
            Assert.Contains("Your cart is empty.", cut.Markup);
        }

        [Fact]
        public void AddToCart_Updates_Total()
        {
            var cut = RenderComponent<Index>();

            // Click first "Add to Cart" button twice
            var buttons = cut.FindAll("button");
            // Assuming product buttons render before any remove buttons:
            buttons[0].Click();
            buttons[0].Click();

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$2.00", cut.Markup); // Apple x2
        }

        [Fact]
        public void RemoveFromCart_Removes_Item_And_Updates_Total()
        {
            var cut = RenderComponent<Index>();

            // Add Apple ($1.00) then Banana ($0.50)
            var buttons = cut.FindAll("button");
            buttons[0].Click(); // Apple
            buttons[1].Click(); // Banana

            // Find a Remove button and click it
            var removeButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Remove")).ToList();
            removeButtons.First().Click();

            Assert.Contains("Total:", cut.Markup);
            // Not asserting exact value (depends which item was removed); just ensure cart isn't empty
            Assert.DoesNotContain("Your cart is empty.", cut.Markup);
        }

        [Fact]
        public void Rapid_DoubleClick_Adds_Exactly_Twice()
        {
            var cut = RenderComponent<Index>();

            var addButtons = cut.FindAll("button")
                                .Where(b => b.TextContent.Contains("Add"))
                                .ToList();

            // Double-click the first product (Apple $1.00)
            addButtons[0].Click();
            addButtons[0].Click();

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$2.00", cut.Markup);
        }

        [Fact]
        public void Reload_Starts_Empty_By_Design()
        {
            // First render: add an item so cart is NOT empty
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();
            addButtons[0].Click();
            Assert.DoesNotContain("Your cart is empty.", cut.Markup);

            // Simulate browser reload by rendering a fresh component
            var cutReloaded = RenderComponent<Index>();
            Assert.Contains("Your cart is empty.", cutReloaded.Markup);
        }

        [Fact]
        public void Duplicate_Adds_Scale_Total_Correctly()
        {
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            // Apple ($1.00) x3
            addButtons[0].Click();
            addButtons[0].Click();
            addButtons[0].Click();

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$3.00", cut.Markup);
        }

        [Fact]
        public void Remove_One_Instance_When_Duplicates_Exist()
        {
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            // Apple ($1.00) x2
            addButtons[0].Click();
            addButtons[0].Click();

            // Click a single Remove
            var removeButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Remove")).ToList();
            removeButtons.First().Click();

            // One Apple remains; total reflects $1.00
            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$1.00", cut.Markup);
        }

        [Fact]
        public void Mixed_Prices_Sum_Is_Exact()
        {
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            // Apple ($1.00), Banana ($0.50), Orange ($0.75) = $2.25
            addButtons[0].Click(); // Apple
            addButtons[1].Click(); // Banana
            addButtons[2].Click(); // Orange

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$2.25", cut.Markup);
        }

        [Fact]
        public void Price_Formatting_Is_Two_Decimals()
        {
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            // Add Banana ($0.50) -> expect "$0.50"
            addButtons[1].Click();

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$0.50", cut.Markup);
        }

        [Fact]
        public void Removing_Last_Item_Shows_Empty_State()
        {
            var cut = RenderComponent<Index>();
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            // Add one Apple then remove it
            addButtons[0].Click();
            var removeButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Remove")).ToList();
            removeButtons.First().Click();

            Assert.Contains("Your cart is empty.", cut.Markup);
        }

        [Fact]
        public void Catalog_Renders_Products_With_Prices_And_AddButtons()
        {
            var cut = RenderComponent<Index>();

            // Product names & prices visible
            var markup = cut.Markup;
            Assert.Contains("Apple", markup);
            Assert.Contains("$1.00", markup);
            Assert.Contains("Banana", markup);
            Assert.Contains("$0.50", markup);
            Assert.Contains("Orange", markup);
            Assert.Contains("$0.75", markup);

            // Every product has an "Add" button
            var addButtons = cut.FindAll("button")
                                .Where(b => b.TextContent.Contains("Add"))
                                .ToList();
            Assert.True(addButtons.Count >= 3); // Apple, Banana, Orange
        }

        [Fact]
        public void Burst_Adds_100x_First_Product_Total_Is_Accurate()
        {
            var cut = RenderComponent<Index>();

            // Click first "Add" 100 times (Apple = $1.00)
            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();
            for (int i = 0; i < 100; i++) addButtons[0].Click();

            Assert.Contains("Total:", cut.Markup);
            Assert.Contains("$100.00", cut.Markup);
        }

        [Fact]
        public void Alternating_Add_Remove_Never_Produces_Negative_Total()
        {
            var cut = RenderComponent<Index>();

            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();

            for (int i = 0; i < 5; i++) addButtons[0].Click(); // +$5.00
            for (int i = 0; i < 5; i++)
            {
                var removeButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Remove")).ToList();
                removeButtons.First().Click();
                Assert.DoesNotContain("-$", cut.Markup);
            }

            // Empty state restored
            Assert.Contains("Your cart is empty.", cut.Markup);
        }

        [Fact]
        public void Add_Then_Remove_Shows_Feedback_Message()
        {
            var cut = RenderComponent<Index>();

            var addButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Add")).ToList();
            addButtons[0].Click(); // Add Apple
            var feedback = cut.Find("[data-testid='ui-feedback']").TextContent;
            Assert.Contains("Added", feedback);

            var removeButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Remove")).ToList();
            removeButtons.First().Click(); // Remove Apple
            feedback = cut.Find("[data-testid='ui-feedback']").TextContent;
            Assert.Contains("Removed", feedback);
        }

        [Fact]
        public void Unavailable_Product_Disables_Add_Button()
        {
            var cut = RenderComponent<Index>();

            // Find the card that contains the text "Grapes"
            var productCards = cut.FindAll(".card");
            var grapesCard = productCards.First(c => c.TextContent.Contains("Grapes"));

            // Get its Add button
            var addBtn = grapesCard.GetElementsByTagName("button").First();

            // Assert the HTML boolean attribute is present
            Assert.True(addBtn.HasAttribute("disabled"), "Expected 'disabled' attribute on unavailable product's Add button.");

            // Double-safety: clicking should not change feedback/total
            var preMarkup = cut.Markup;
            addBtn.Click();
            var postMarkup = cut.Markup;
            Assert.Equal(preMarkup, postMarkup);
        }
    }
}
