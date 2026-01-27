namespace Avrhil.BigCommerce.API.Models;

public static class ProductMapper
{
    public static Product ToProduct(Part part)
    {
        return new Product
        {
            Name = part.Description,
            Sku = part.PartNumber,
            Price = part.Price,
            Description = $"{part.Brand} - {part.Description}",
            Type = "physical",
            Weight = 0
        };
    }

    public static Part ToPart(Product product)
    {
        return new Part
        {
            PartNumber = product.Sku,
            Description = product.Name,
            Price = product.Price,
            Brand = ExtractBrand(product.Description),
            Quantity = 0,
            ImageUrl = string.Empty
        };
    }

    public static List<Product> ToProducts(List<Part> parts)
    {
        return parts.Select(ToProduct).ToList();
    }

    public static List<Part> ToParts(List<Product> products)
    {
        return products.Select(ToPart).ToList();
    }

    private static string ExtractBrand(string description)
    {
        return description?.Split('-').FirstOrDefault()?.Trim() ?? string.Empty;
    }
}
