using FindItParts.Searchparts.Models;

namespace FindItParts.Searchparts.Services;

public class MockDataService
{
    private List<FindItPartsProduct> _products;

    public MockDataService()
    {
        LoadMockData();
    }

    private void LoadMockData()
    {
        _products = GenerateMockProducts();
    }

    public SearchResponse SearchProducts(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new SearchResponse { Products = new List<FindItPartsProduct>(), TotalResults = 0 };
        }

        var filtered = _products.Where(p =>
            p.PartNumber.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Manufacturer.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            (p.Tags != null && p.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)))
        ).ToList();

        return new SearchResponse { Products = filtered, TotalResults = filtered.Count };
    }

    public FindItPartsProduct GetProductById(string id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    private List<FindItPartsProduct> GenerateMockProducts()
    {
        var manufacturers = new[] { "Bendix", "Meritor", "Eaton", "Dana", "Wabco", "Haldex", "ConMet", "Gunite", "SAF-Holland", "Hendrickson" };
        var products = new List<FindItPartsProduct>();
        var random = new Random(42);

        var partTypes = new[]
        {
            ("Brake Pad Set", "Brakes", 150, 450, "https://images.unsplash.com/photo-1486262715619-67b85e0b08d3?w=400&h=400&fit=crop"),
            ("Brake Rotor", "Brakes", 200, 600, "https://images.unsplash.com/photo-1625047509168-a7026f36de04?w=400&h=400&fit=crop"),
            ("Air Disc Brake Caliper", "Brakes", 400, 900, "https://images.unsplash.com/photo-1619642751034-765dfdf7c58e?w=400&h=400&fit=crop"),
            ("Brake Chamber", "Brakes", 100, 300, "https://images.unsplash.com/photo-1580273916550-e323be2ae537?w=400&h=400&fit=crop"),
            ("Slack Adjuster", "Brakes", 50, 150, "https://images.unsplash.com/photo-1581092160562-40aa08e78837?w=400&h=400&fit=crop"),
            ("Air Spring", "Suspension", 200, 500, "https://images.unsplash.com/photo-1581092918056-0c4c3acd3789?w=400&h=400&fit=crop"),
            ("Shock Absorber", "Suspension", 150, 400, "https://images.unsplash.com/photo-1581092162384-8987c1d64718?w=400&h=400&fit=crop"),
            ("Leaf Spring", "Suspension", 300, 700, "https://images.unsplash.com/photo-1581092160607-ee22621dd758?w=400&h=400&fit=crop"),
            ("King Pin Kit", "Suspension", 80, 200, "https://images.unsplash.com/photo-1581092795360-fd1ca04f0952?w=400&h=400&fit=crop"),
            ("Bushing Kit", "Suspension", 40, 120, "https://images.unsplash.com/photo-1581092580497-e0d23cbdf1dc?w=400&h=400&fit=crop"),
            ("LED Headlight", "Lighting", 100, 350, "https://images.unsplash.com/photo-1549317661-bd32c8ce0db2?w=400&h=400&fit=crop"),
            ("Tail Light Assembly", "Lighting", 60, 180, "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400&h=400&fit=crop"),
            ("Marker Light", "Lighting", 15, 45, "https://images.unsplash.com/photo-1513828583688-c52646db42da?w=400&h=400&fit=crop"),
            ("Work Light", "Lighting", 50, 150, "https://images.unsplash.com/photo-1565008576549-57569a49371d?w=400&h=400&fit=crop"),
            ("Light Bar", "Lighting", 200, 600, "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400&h=400&fit=crop"),
            ("Alternator", "Electrical", 250, 650, "https://images.unsplash.com/photo-1581092160562-40aa08e78837?w=400&h=400&fit=crop"),
            ("Starter Motor", "Electrical", 300, 750, "https://images.unsplash.com/photo-1581092918056-0c4c3acd3789?w=400&h=400&fit=crop"),
            ("Battery", "Electrical", 150, 400, "https://images.unsplash.com/photo-1609557927087-f9cf8e88de18?w=400&h=400&fit=crop"),
            ("Wiring Harness", "Electrical", 80, 250, "https://images.unsplash.com/photo-1581092160607-ee22621dd758?w=400&h=400&fit=crop"),
            ("Sensor", "Electrical", 30, 100, "https://images.unsplash.com/photo-1581092795360-fd1ca04f0952?w=400&h=400&fit=crop")
        };

        for (int i = 0; i < 100; i++)
        {
            var mfg = manufacturers[random.Next(manufacturers.Length)];
            var partType = partTypes[random.Next(partTypes.Length)];
            var basePrice = random.Next(partType.Item3, partType.Item4);
            var accountDiscount = random.Next(10, 25);
            var accountPrice = basePrice * (100 - accountDiscount) / 100m;
            var corePrice = partType.Item2 == "Brakes" ? random.Next(25, 100) : 0;
            var qty = random.Next(5, 50);

            products.Add(new FindItPartsProduct
            {
                Id = (912345 + i).ToString(),
                PartNumber = $"{mfg.Substring(0, 3).ToUpper()}-{random.Next(1000, 9999)}",
                Description = $"{mfg} {partType.Item1}",
                Manufacturer = mfg,
                Brand = mfg,
                CorePrice = basePrice,
                AccountPrice = accountPrice,
                Quantity = qty,
                InventoryStatus = qty > 10 ? "In Stock" : "Low Stock",
                ImageUrl = partType.Item5,
                Tags = GenerateTags(qty, basePrice, random),
                PiesAttributes = new PiesAttributes
                {
                    PartType = partType.Item1,
                    PartTerminologyId = (10000 + i).ToString(),
                    Position = GetPosition(partType.Item1),
                    Weight = $"{random.Next(1, 50)} lbs",
                    Dimensions = $"{random.Next(6, 24)} x {random.Next(4, 18)} x {random.Next(2, 12)} inches",
                    Material = GetMaterial(partType.Item1),
                    CountryOfOrigin = random.Next(2) == 0 ? "USA" : "China"
                },
                Variants = new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        VariantId = (501234 + i).ToString(),
                        Price = basePrice,
                        AccountPrice = accountPrice,
                        CorePrice = corePrice,
                        Quantity = qty,
                        Location = GetLocation(random),
                        EstimatedShipDays = random.Next(1, 3)
                    }
                }
            });
        }

        return products;
    }

    private List<string> GenerateTags(int qty, decimal price, Random random)
    {
        var tags = new List<string>();
        if (qty > 20) tags.Add("In Stock");
        if (price > 400) tags.Add("Premium");
        if (random.Next(3) == 0) tags.Add("Express Shipping");
        if (random.Next(4) == 0) tags.Add("Top Rated");
        if (random.Next(5) == 0) tags.Add("OEM");
        return tags;
    }

    private string GetPosition(string partType)
    {
        if (partType.Contains("Brake")) return "Front/Rear";
        if (partType.Contains("Light")) return "Exterior";
        if (partType.Contains("Spring")) return "Axle";
        return "Universal";
    }

    private string GetMaterial(string partType)
    {
        if (partType.Contains("Brake Pad")) return "Ceramic";
        if (partType.Contains("Rotor")) return "Cast Iron";
        if (partType.Contains("LED")) return "Polycarbonate/LED";
        if (partType.Contains("Spring")) return "Steel";
        return "Metal/Composite";
    }

    private string GetLocation(Random random)
    {
        var locations = new[] { "Dallas, TX", "Atlanta, GA", "Chicago, IL", "Los Angeles, CA", "Phoenix, AZ" };
        return locations[random.Next(locations.Length)];
    }
}
