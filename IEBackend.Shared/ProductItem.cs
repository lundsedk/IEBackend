namespace IEBackend.Shared;

public record struct ProductItem : ICatalogMarker
{
    public string id { get; init; }
    public string title { get; init; }
    public decimal price { get; init; }
    public string description { get; init; }
    public string category { get; init; }
    public string image { get; init; }

    public ProductItem(string id, string title, decimal price, string description, string category, string image)
    {
        this.id = id;
        this.title = title;
        this.price = price;
        this.description = description;
        this.category = category;
        this.image = image;
    }
}


