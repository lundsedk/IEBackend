using System.Dynamic;
using System.Text.Json;

using IEBackend.Shared;

namespace IEBackend.CatalogRepo;

//include some baseurl
//	https://api.example.com/v1/products/123?include=reviews&page=2#section


public class CatalogRepoService
{
	private static readonly string BaseURL = "http://InfinityElectronics/ERPInterface/CatalogUpdate/";
	private static readonly string categorySegment = "Categories?";
	private static readonly string croductSegment = "Products?";
	private static readonly string currentVersionParam = "from=";
	private static readonly string newVersionParam = "&to=";

	private static readonly string categoryFileName = "categories-sample-v1.json";
	private static readonly string productsFileName = "products-sample-v1.json";


	public static JsonDocument? GetUpdate<T>(Version currentVersion, Version newVersion)
	{

        string segment;
		string fileName;

		if (typeof(T) == typeof(CategoryItem))
		{
			segment = categorySegment;
			fileName = categoryFileName;
		}
		else if (typeof(T) == typeof(ProductItem))
		{
			segment = croductSegment;
			fileName = productsFileName;
		}
		else
		{
			throw new NotSupportedException("Type not supported.");
		}

		string fullApiString = BaseURL +
								segment +
								currentVersionParam +
								currentVersion.ToString() +
								newVersionParam +
								newVersion.ToString();

		// Mock API call
		return ReadJsonFromFile(fileName);

	}
	

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
