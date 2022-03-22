using CRUD_Api.DataStore;
using CRUD_Api.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Configure persistent storage
var database = new FileStore();

app.MapDelete("/tools", (string tool) =>
{
    var succeeded = database.Delete(tool);
    if (succeeded)
    {
        return Results.Ok(new { 
            entity = tool,
            message = "Tool removed"
        });
    }

    return Results.NotFound();
})
.WithName("DeleteTool");

app.MapGet("/tools", () =>
{
    return database.Get();
})
.WithName("GetTools");

app.MapPost("/tools", (ToolModel toolPurchase) =>
{
    database.Add(toolPurchase);
    return "Added: " + toolPurchase.ToString() + " the repository";
})
.WithName("AddTool"); //Inte rest standard. Döp till vad du vill.

app.MapPut("/tools", (string existingTool, string newTool) =>
{
    var succeeded = database.Update(existingTool, newTool);
    if(succeeded)
    {
        return Results.Ok(new
        {
            entity = existingTool,
            message = "The existing tool " + existingTool + " has been replaced with " + newTool
        });
    }
    return Results.NotFound();
})
.WithName("UpdateTool");

app.Run();