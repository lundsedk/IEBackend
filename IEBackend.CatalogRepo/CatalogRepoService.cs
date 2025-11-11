using System.Dynamic;
using System.Text.Json;

namespace IEBackend.CatalogRepo;

//include some baseurl
//	https://api.example.com/v1/products/123?include=reviews&page=2#section


public class CatalogRepoService
{
	private static readonly string BaseURL = "http://InfinityElectronics/ERPInterface/CatalogUpdate/";
	private static readonly string CategorySegment = "Categories?";
	private static readonly string ProductSegment = "Products?";
	private static readonly string currentVersionParam = "from=";
	private static readonly string newVersionParam = "&to=";

	private static readonly string categoryFileName = "categories-sample-v1.json";
	private static readonly string productsFileName = "products-sample-v1.json";

	public static JsonDocument? GetCategoryUpdate(Version currentVersion, Version newVersion)
	{

		string fullApiString = 	BaseURL +
								CategorySegment + 
								currentVersionParam +
								currentVersion.ToString() +
								newVersionParam +
								newVersion.ToString();

		// Mock API call
		if (fullApiString == "http://InfinityElectronics/ERPInterface/CatalogUpdate/Categories?from=0.0.0&to=1.0.0")
		{
			return ReadJsonFromFile(categoryFileName);
		} else {
			return null;
		}

	}

	public static JsonDocument? GetProductUpdate(Version currentVersion, Version newVersion)
	{

		string fullApiString = 	BaseURL +
								ProductSegment + 
								currentVersionParam +
								currentVersion.ToString() +
								newVersionParam +
								newVersion.ToString();

		// Mock API call
		if (fullApiString == "http://InfinityElectronics/ERPInterface/CatalogUpdate/Products?from=0.0.0&to=1.0.0")
		{
			return ReadJsonFromFile(productsFileName);
		} else {
			return null;
		}

	}

	//version object in shared

	private static JsonDocument? ReadJsonFromFile(string relativeFileName)
	{
		if (!File.Exists(relativeFileName))
		{
			return null;
		}

		try
		{
			string JsonRawString = File.ReadAllText(relativeFileName);
			JsonDocument JsonData = JsonDocument.Parse(JsonRawString);
			return JsonData;
		}
		catch (IOException ex)
		{
			throw;
		}
		catch (JsonException ex)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw;
		}
	}
}
