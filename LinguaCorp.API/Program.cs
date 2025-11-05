using LinguaCorp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure to listen on Cloud Run's PORT
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Services
builder.Services.AddControllers();
builder.Services.AddSingleton<IPhraseService, PhraseService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only redirect to HTTPS in non-Cloud Run environments
// Cloud Run handles HTTPS termination
var isCloudRun = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("K_SERVICE"));
if (!isCloudRun && !app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Health check endpoint for Cloud Run
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .ExcludeFromDescription();

// Swagger redirect
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();