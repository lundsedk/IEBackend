using System.Collections.Immutable;
using IEBackend.CatalogRepo;
using IEBackend.Shared;
using System.Text.Json;

namespace IEBackend.Catalog;

public class Catalog
{
	// TODO: These could be a dict or similar, so we can iterate through it to prevent code copying

	record struct InternalCatalog(
		ImmutableArray<CategoryItem> _categories,
		ImmutableArray<ProductItem> _products
	);

	private readonly object _swaplock = new();
	private InternalCatalog _catalog;


	public Catalog()
	{
		_catalog._categories = ImmutableArray<CategoryItem>.Empty;
		_catalog._products = ImmutableArray<ProductItem>.Empty;

		Update(new Version(0, 0), new Version(1, 1));
	}

	private void Update(Version currentVersion, Version newVersion)
	{
        var newCatalog = new InternalCatalog(
            ImmutableArray<CategoryItem>.Empty,
            ImmutableArray<ProductItem>.Empty
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

		}

	}

	private ImmutableArray<T> Builder<T>(JsonDocument jsonDoc)
	{

		// TODO:
		// naive version for now, maybe change to copying from existing version later

		var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		var list = JsonSerializer.Deserialize<List<T>>(jsonDoc, opts) ?? new List<T>();
		ImmutableArray<T> returnValue = list.ToImmutableArray();

		return returnValue;
	}





}
