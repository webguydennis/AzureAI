using AzureAI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Cors Policy
builder.Services.AddCors((options) => {
    options.AddPolicy("DevCors", (corsBuilder) => {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) => {
        corsBuilder.WithOrigins("https://myProduction.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Text Analytics Service
builder.Services.AddSingleton<ITextSentimentService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var endpoint = configuration["TextAnalytics:Endpoint"] ?? "not found textanalytics endpoint";
    var apiKey = configuration["TextAnalytics:ApiKey"] ?? "not found textanalytics key";
    return new TextSentimentService(endpoint, apiKey);
});

// Add Speech To Text Service
builder.Services.AddSingleton<ISpeechToTextService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new SpeechToTextService(
        configuration["AzureSpeech:ApiKey"] ?? "not found SpeechToText ApiKey",
        configuration["AzureSpeech:Region"] ?? "not found SpeechToText Region"
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
} else {
    app.UseCors("ProdCors");
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
