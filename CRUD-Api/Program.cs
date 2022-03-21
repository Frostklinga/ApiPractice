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
var database = new CRUD_Api.FileStore();

app.MapDelete("/tools", (string tool) =>
{
    var succeeded = database.RemoveTool(tool);
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
    return database.Tools();
})
.WithName("GetTools");

app.MapPost("/tools", (string tool) =>
{
    database.AddTool(tool);
    return "Added: " + tool + " the repository of tools";
})
.WithName("AddTool"); //Inte rest standard. Döp till vad du vill.

app.MapPut("/tools", (string existingTool, string newTool) =>
{
    var succeeded = database.ReplaceTool(existingTool, newTool);
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
.WithName("ReplaceTool");

app.Run();