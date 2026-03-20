using System.Reflection;
using SudokuApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueLocal", policy =>
    {
        if (builder.Environment.IsDevelopment())
            policy.WithOrigins("http://localhost:5173", "https://localhost:5173");
        else
            policy.AllowAnyOrigin();

        policy
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DI registration
builder.Services.AddScoped<ISudokuService, SudokuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("VueLocal");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Sudoku API is running");
app.Run();
