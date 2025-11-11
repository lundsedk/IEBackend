using System.Reflection;
using System.Collections.Generic;
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

    // helper to read fields out of the boxed/internal struct (InternalCatalog)
private static T GetStructField<T>(object boxedStruct, string memberName)
{
    var t = boxedStruct.GetType();

    // try property (positional record-structs expose public properties)
    var prop = t.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (prop != null)
        return (T)prop.GetValue(boxedStruct)!;

    // try field (in case it's a plain field)
    var field = t.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (field != null)
        return (T)field.GetValue(boxedStruct)!;

    // try compiler-generated backing field pattern: <name>k__BackingField
    var backing = t.GetField($"<{memberName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
    if (backing != null)
        return (T)backing.GetValue(boxedStruct)!;

    Assert.True(false, $"Member '{memberName}' not found on {t.FullName}");
    return default!;
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

        // _catalog is a private struct field on Catalog; read it boxed then inspect its members
        var boxedCatalog = GetPrivateField<object>(catalog, "_catalog");

        var cats = GetStructField<IReadOnlyList<CategoryItem>>(boxedCatalog, "_categories");
        var prods = GetStructField<IReadOnlyList<ProductItem>>(boxedCatalog, "_products");

        Assert.NotNull(cats);
        Assert.NotNull(prods);
    }

    [Fact]
    public void Categories_loaded_have_at_least_one_item_and_expected_first_id()
    {
        var catalog = new Catalog();
        var boxedCatalog = GetPrivateField<object>(catalog, "_catalog");
        var cats = GetStructField<IReadOnlyList<CategoryItem>>(boxedCatalog, "_categories");

        Assert.True(cats.Count > 0, "Expected at least one category loaded.");
        Assert.Equal("cat-01", cats[0].id);
    }

    [Fact]
    public void Products_loaded_have_at_least_one_item_and_reference_category()
    {
        var catalog = new Catalog();
        var boxedCatalog = GetPrivateField<object>(catalog, "_catalog");
        var prods = GetStructField<IReadOnlyList<ProductItem>>(boxedCatalog, "_products");

        Assert.True(prods.Count > 0, "Expected at least one product loaded.");
        Assert.Contains(prods, p => p.category == "cat-01");
    }

    [Fact]
    public void All_categories_have_non_empty_id_and_name()
    {
        var catalog = new Catalog();
        var boxedCatalog = GetPrivateField<object>(catalog, "_catalog");
        var cats = GetStructField<IReadOnlyList<CategoryItem>>(boxedCatalog, "_categories");

        Assert.True(cats.Count > 0, "Expected categories present to validate properties.");
        foreach (var c in cats)
        {
            Assert.False(string.IsNullOrWhiteSpace(c.id));
            Assert.False(string.IsNullOrWhiteSpace(c.name));
        }
    }
}