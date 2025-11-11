using System.Collections.Immutable;
using IEBackend.CatalogRepo;
using IEBackend.Shared;
using System.Text.Json;

namespace IEBackend.Catalog;

public class Catalog
{
	ImmutableArray<CategoryItem> categories_;
	ImmutableArray<ProductItem> products_;


	public Catalog()
	{
		categories_ = new ImmutableArray<CategoryItem>();
		products_ = new ImmutableArray<ProductItem>();

		Update(new Version(1, 0), new Version(1, 1));
	}

	private void Update(Version currentVersion, Version newVersion)
	{
		var jsonDocCatalog = CatalogRepoService.GetUpdate<CategoryItem>(currentVersion, newVersion);
		var jsonDocProducts = CatalogRepoService.GetUpdate<ProductItem>(currentVersion, newVersion);

		if (jsonDocCatalog != null && jsonDocProducts != null)
		{
			categories_ = Builder<CategoryItem>(jsonDocCatalog);
			products_ = Builder<ProductItem>(jsonDocProducts);
		}

	}

	private ImmutableArray<T> Builder<T>(JsonDocument jsonDoc)
	{

		//TODO:
		// naive version for now, maybe change to copying from existing version later


		var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		var list = JsonSerializer.Deserialize<List<T>>(jsonDoc, opts) ?? new List<T>();
		ImmutableArray<T> returnValue = list.ToImmutableArray();

		return returnValue;
	}





}
