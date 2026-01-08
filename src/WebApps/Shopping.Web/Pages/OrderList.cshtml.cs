using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Ordering;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger) :
    PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        // Assumption customerId is passed in from the UI authenticated user
        // TODO: fix this hardcoded guid
        var customerId = new Guid("9dbc33d1-2398-4b96-adc4-2752846e5b90");

        var response = await orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}