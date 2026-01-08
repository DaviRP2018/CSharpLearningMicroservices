using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class CheckoutModel(IBasketService basketService, ILogger<CheckoutModel> logger) : PageModel
{
    [BindProperty] public BasketCheckoutModel Order { get; set; } = default!;
    public ShoppingCartModel Cart { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await basketService.LoadUserBasket();
        return Page();
    }

    public async Task<IActionResult> OnPostCheckoutAsync()
    {
        logger.LogInformation("Checkout button clicked");

        Cart = await basketService.LoadUserBasket();

        if (!ModelState.IsValid)
            return Page();

        // Assumption customerId is passed in from the UI authenticated user swn
        // TODO: fix this GUID, it shouldn't be hardcoded
        Order.CustomerId = new Guid("9dbc33d1-2398-4b96-adc4-2752846e5b90");
        Order.UserName = Cart.UserName;
        Order.TotalPrice = Cart.TotalPrice;

        await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}