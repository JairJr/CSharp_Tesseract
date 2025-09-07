using DocumentAnalyzer.Core.Interfaces;
using DocumentAnalyzer.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Document Analyzer API", Version = "v1" });
});

// Registrar serviços da aplicação
builder.Services.AddSingleton<IDocumentAnalyzer>(provider =>
{
    // Caminho para os dados do Tesseract OCR
    string tesseractDataPath = Path.Combine(builder.Environment.ContentRootPath, "tessdata");
    return new DocumentAnalyzerService(tesseractDataPath);
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Criar diretório para uploads
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "uploads"));

// Criar diretório para dados do Tesseract OCR
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "tessdata"));

app.Run();