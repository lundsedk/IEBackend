namespace IEBackend.Shared;

public record struct CategoryItem
{
    public string id { get; init; }
    public string name { get; init; }
    public string description { get; init; }

    public CategoryItem(string id, string name, string description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}