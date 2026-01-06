using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Pages;

public class IndexModel : PageModel
{
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();

    public void OnGet()
    {
    }
}