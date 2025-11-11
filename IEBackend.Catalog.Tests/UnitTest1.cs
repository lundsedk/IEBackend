using System.Reflection;
using System.Collections.Immutable;
using Xunit;
using IEBackend.Catalog;
using IEBackend.Shared;

namespace IEBackend.Catalog.Tests;


public class UnitTest1
{
    private static T GetPrivateField<T>(object instance, string fieldName)
    {
        var fi = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(fi);
        return (T)fi!.GetValue(instance)!;
    }

    [Fact]
    public void Constructor_throws_no_exceptions()
    {
        var ex = Record.Exception(() => new Catalog());
        Assert.Null(ex);
    }
    
    [Fact]
    public void Constructor_initializes_backing_fields()
    {
        var catalog = new Catalog();

        var cats = GetPrivateField<ImmutableArray<CategoryItem>>(catalog, "categories_");
        var prods = GetPrivateField<ImmutableArray<ProductItem>>(catalog, "products_");

        Assert.False(cats.IsDefault, "_categories should be initialized (not default).");
        Assert.False(prods.IsDefault, "_products should be initialized (not default).");
    }

    [Fact]
    public void Categories_loaded_have_at_least_one_item_and_expected_first_id()
    {
        var catalog = new Catalog();
        var cats = GetPrivateField<ImmutableArray<CategoryItem>>(catalog, "categories_");

        Assert.True(cats.Length > 0, "Expected at least one category loaded.");
        Assert.Equal("cat-01", cats[0].id);
    }

    [Fact]
    public void Products_loaded_have_at_least_one_item_and_reference_category()
    {
        var catalog = new Catalog();
        var prods = GetPrivateField<ImmutableArray<ProductItem>>(catalog, "products_");

        Assert.True(prods.Length > 0, "Expected at least one product loaded.");
        Assert.Contains(prods, p => p.category == "cat-01");
    }

    [Fact]
    public void All_categories_have_non_empty_id_and_name()
    {
        var catalog = new Catalog();
        var cats = GetPrivateField<ImmutableArray<CategoryItem>>(catalog, "categories_");

        Assert.True(cats.Length > 0, "Expected categories present to validate properties.");
        foreach (var c in cats)
        {
            Assert.False(string.IsNullOrWhiteSpace(c.id));
            Assert.False(string.IsNullOrWhiteSpace(c.name));
        }
    }
}
