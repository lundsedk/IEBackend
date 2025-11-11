using IEBackend.Shared;
using IEBackend.Catalog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Catalog>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/getProductDetails", (Catalog catalog, string id) =>
{
    var products = catalog.GetProducts();
    var product = products.FirstOrDefault(p => p.id == id);

    if (string.IsNullOrEmpty(product.id))
        return Results.NotFound();

    return Results.Ok(product);

})
.WithName("GetProductDetails")
.WithOpenApi();

app.MapGet("/getProductListing", (Catalog catalog, string category) =>
{
    var products = catalog.GetProducts();
    var list = products
        .Where(p => string.Equals(p.category, category, StringComparison.OrdinalIgnoreCase))
        .ToArray();

    return Results.Ok(list);
})
.WithName("GetProductListing")
.WithOpenApi();


app.Run();