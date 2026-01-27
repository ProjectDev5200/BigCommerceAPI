using FindItParts.Searchparts.Models;
using FindItParts.Searchparts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindItParts.Searchparts.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly MockDataService _mockDataService;

        public DetailsModel(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public FindItPartsProduct Product { get; set; }

        public IActionResult OnGet(string id)
        {
            Product = _mockDataService.GetProductById(id);
            
            if (Product == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
