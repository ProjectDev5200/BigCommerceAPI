namespace Avrhil.BigCommerce.API.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Sku { get; set; }
    public decimal Price { get; set; }
    public int Weight { get; set; }
    public string Description { get; set; }
}

public class ProductResponse
{
    public Product Data { get; set; }
    public Meta Meta { get; set; }
}

public class ProductsResponse
{
    public List<Product> Data { get; set; }
    public Meta Meta { get; set; }
}

public class Meta
{
    public Pagination Pagination { get; set; }
}

public class Pagination
{
    public int Total { get; set; }
    public int Count { get; set; }
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
