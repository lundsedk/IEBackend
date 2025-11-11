using System.Collections.Immutable;
using IEBackend.CatalogRepo;
using IEBackend.Shared;
using System.Text.Json;

namespace IEBackend.Catalog;

public class Catalog
{

	record struct InternalCatalog(
		IReadOnlyList<CategoryItem> _categories,
		IReadOnlyList<ProductItem> _products
	);

	private Version _currentVersion;
	private readonly object _swaplock = new();
	private InternalCatalog _catalog;


	public Catalog()
	{
		_catalog._categories = Array.Empty<CategoryItem>();
		_catalog._products = Array.Empty<ProductItem>();

		Update(new Version(0, 0), new Version(1, 1));
	}

    public IReadOnlyList<CategoryItem> GetCategories() {
		return _catalog._categories;
	}
    public IReadOnlyList<ProductItem> GetProducts()
	{
		return _catalog._products;
	}

	private void Update(Version currentVersion, Version newVersion)
	{
		var newCatalog = new InternalCatalog(
			Array.Empty<CategoryItem>(),
			Array.Empty<ProductItem>()
		);

		var jsonDocCatalog = CatalogRepoService.GetUpdate<CategoryItem>(currentVersion, newVersion);
		var jsonDocProducts = CatalogRepoService.GetUpdate<ProductItem>(currentVersion, newVersion);

		if (jsonDocCatalog != null && jsonDocProducts != null)
		{
			newCatalog._categories = Builder<CategoryItem>(jsonDocCatalog);
			newCatalog._products = Builder<ProductItem>(jsonDocProducts);

			lock (_swaplock)
			{
				_catalog = newCatalog;
			}
			_currentVersion = newVersion;

		}

	}

	private IReadOnlyList<T> Builder<T>(JsonDocument jsonDoc)
	{

		var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		var list = JsonSerializer.Deserialize<List<T>>(jsonDoc, opts) ?? new List<T>();
		IReadOnlyList<T> returnValue = list.ToList();

		return returnValue;
	}





}
