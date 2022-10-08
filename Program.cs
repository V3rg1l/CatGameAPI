using System.IO.Compression;
using System.Reflection;
using Danilo.CatGame.API.Model;
using Danilo.CatGame.API.Validate;
using Microsoft.Extensions.Hosting.Internal;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.MapPost("/Files/Upload",
    async Task<IResult> (HttpRequest request) =>
    {
        if (!request.HasFormContentType)
            return Results.BadRequest();

        var objReturn = new ApiResponse();
        var path = builder.Environment.ContentRootPath;


        foreach (var file in request.Form.Files)
        {
            string name = file.Name;
            string filename = file.FileName;

            if (Path.GetExtension(file.FileName).Equals(".zip"))
            {
                objReturn.LstApiResponse = ValidateZipFile.Validate(file, path);
                break;
            }
                
        }

        objReturn.IsValidItens();
        //objReturn.ValidStructureFiles = true; -- mock true
        return Results.Ok(objReturn);
    });

app.MapPost("/Files/Save",
    async Task<IResult> (HttpRequest request) =>
    {
        if (!request.HasFormContentType)
            return Results.BadRequest();

        var filePath = builder.Environment.ContentRootPath + "zips/";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }


        foreach (var file in request.Form.Files)
        {
            string name = file.Name;
            string filename = file.FileName;

            if (Path.GetExtension(file.FileName).Equals(".zip"))
            {
                using (var stream = new FileStream(filePath + file.FileName, FileMode.Create))
                {
                    file.CopyTo(stream);
                };
                break;
            }

        }

        return Results.Ok();
    });

app.MapPost("/Files/Delete",
    async Task<IResult> (HttpRequest request) =>
    {
        var filePath = builder.Environment.ContentRootPath + "zips/";

        try
        {
            if (Directory.Exists(filePath))
            {
                string[] filePaths = Directory.GetFiles(filePath);
                foreach (string file in filePaths)
                    File.Delete(file);
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest();
        }

        
    });

app.Run();
