using System.Collections.Immutable;
using IEBackend.CatalogRepo;
using IEBackend.Shared;
using System.Text.Json;

namespace IEBackend.Catalog;

public class Catalog
{
	// TODO: These could be a dict or similar, so we can iterate through it to prevent code copying

	record struct InternalCatalog(
		ImmutableArray<CategoryItem> categories_,
		ImmutableArray<ProductItem> products_
	);

	private InternalCatalog catalog_;


	public Catalog()
	{
		catalog_.categories_ = ImmutableArray<CategoryItem>.Empty;
		catalog_.products_ = ImmutableArray<ProductItem>.Empty;

		Update(new Version(0, 0), new Version(1, 1));
	}

	//TODO: add swapping
	//	some flag or other mechanism to force a reload?
	//	proper lock - noone should have a mix ever
	private void Update(Version currentVersion, Version newVersion)
	{
		var InternalCatalog newCatalog;
		var jsonDocCatalog = CatalogRepoService.GetUpdate<CategoryItem>(currentVersion, newVersion);
		var jsonDocProducts = CatalogRepoService.GetUpdate<ProductItem>(currentVersion, newVersion);

		if (jsonDocCatalog != null && jsonDocProducts != null)
		{
			catalog_.categories_ = Builder<CategoryItem>(jsonDocCatalog);
			catalog_.products_ = Builder<ProductItem>(jsonDocProducts);
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
