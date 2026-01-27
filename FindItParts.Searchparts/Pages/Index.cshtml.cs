using FindItParts.Searchparts.Models;
using FindItParts.Searchparts.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindItParts.Searchparts.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MockDataService _mockDataService;

        public IndexModel(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public string Query { get; set; }
        public SearchResponse SearchResults { get; set; }

        public void OnGet(string query)
        {
            Query = query;
            SearchResults = _mockDataService.SearchProducts(query);
        }
    }
}
