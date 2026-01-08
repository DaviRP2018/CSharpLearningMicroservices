using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class CartModel(IBasketService basketService, ILogger<CartModel> logger) : PageModel
{
    public ShoppingCartModel Cart { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await basketService.LoadUserBasket();

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveFromCartAsync(Guid productId)
    {
        logger.LogInformation("Remove from cart button clicked");
        Cart = await basketService.LoadUserBasket();

        Cart.Items.RemoveAll(x => x.ProductId == productId);
        await basketService.StoreBasket(new StoreBasketRequest(Cart));

        return RedirectToPage();
    }
}