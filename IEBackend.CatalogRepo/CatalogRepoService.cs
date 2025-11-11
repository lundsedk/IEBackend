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
	private static readonly string productSegment = "Products?";
	private static readonly string currentVersionParam = "from=";
	private static readonly string newVersionParam = "&to=";

	private static readonly string categoryFileName = "categories-sample-v1.json";
	private static readonly string productsFileName = "products-sample-v1.json";


	public static JsonDocument? GetUpdate<T>(Version currentVersion, Version newVersion) where T : ICatalogMarker
	{

		var (segment, filename) = GetSegmentAndFilename<T>() ;

		// Unused
		string fullApiString = 	BaseURL +
								segment +
								currentVersionParam +
								currentVersion.ToString() +
								newVersionParam +
								newVersion.ToString();

		// Mock API call
		return ReadJsonFromFile(filename);

	}


	private static JsonDocument? ReadJsonFromFile(string relativeFileName)
	{
		if (!File.Exists(relativeFileName))
		{
			throw new System.IO.FileNotFoundException($"Catalog JSON file not found: '{relativeFileName}'.");
		}

		try
		{
			string JsonRawString = File.ReadAllText(relativeFileName);
			JsonDocument JsonData = JsonDocument.Parse(JsonRawString);
			return JsonData;
		}
        catch (IOException ex)
        {
            throw new IOException($"Failed to read catalog JSON file '{relativeFileName}': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Failed to parse JSON from '{relativeFileName}': {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while loading '{relativeFileName}': {ex.Message}", ex);
        }
	}

	private static (string a, string b) GetSegmentAndFilename<T>() where T : ICatalogMarker
	{

		return typeof(T) switch
		{
			Type t when t == typeof(CategoryItem) => (categorySegment, categoryFileName),
			Type t when t == typeof(ProductItem) => (productSegment, productsFileName),
			_ => throw new NotSupportedException("Type not supported.")
		};
	
	}

}
