using DocumentAnalyzer.Core.Interfaces;
using DocumentAnalyzer.Core.Models;
using DocumentAnalyzer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAnalyzer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentAnalyzer _documentAnalyzer;
        private readonly ILogger<DocumentController> _logger;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(
            IDocumentAnalyzer documentAnalyzer,
            ILogger<DocumentController> logger,
            IWebHostEnvironment environment)
        {
            _documentAnalyzer = documentAnalyzer;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Faz upload e analisa um documento
        /// </summary>
        [HttpPost("analyze")]
        public async Task<ActionResult<DocumentAnalysisResponse>> AnalyzeDocument([FromForm] DocumentUploadRequest request)
        {
            if (request.Document == null || request.Document.Length == 0)
            {
                return BadRequest(DocumentAnalysisResponse.Error("Nenhum arquivo", "Nenhum arquivo foi enviado"));
            }

            try
            {
                // Salvar o arquivo temporariamente
                string uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Garantir que o diretório existe

                string uniqueFileName = $"{Guid.NewGuid()}_{request.Document.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Document.CopyToAsync(fileStream);
                }

                // Analisar o documento
                var result = new DocumentAnalysisResult
                {
                    FileName = request.Document.FileName
                };

                // Classificar o tipo de documento
                result.DocumentType = await _documentAnalyzer.ClassifyDocumentTypeAsync(filePath);
                
                // Analisar legibilidade
                result.ReadabilityScore = await _documentAnalyzer.AnalyzeReadabilityAsync(filePath);
                
                // Extrair texto (limitado para a resposta)
                result.ExtractedText = await _documentAnalyzer.ExtractTextAsync(filePath);
                
                // Definir confiança na classificação (simulado para demonstração)
                result.ClassificationConfidence = result.DocumentType != DocumentType.Unknown ? 85 : 30;

                // Limpar o arquivo temporário após a análise
                System.IO.File.Delete(filePath);

                return Ok(DocumentAnalysisResponse.FromResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar documento: {Message}", ex.Message);
                return StatusCode(500, DocumentAnalysisResponse.Error(
                    request.Document?.FileName ?? "desconhecido", 
                    "Ocorreu um erro ao processar o documento"));
            }
        }
    }
}