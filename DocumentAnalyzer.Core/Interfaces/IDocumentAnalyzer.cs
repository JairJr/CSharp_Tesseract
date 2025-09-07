using DocumentAnalyzer.Core.Models;
namespace DocumentAnalyzer.Core.Interfaces;
/// <summary>
/// Interface para serviços de análise de documentos
/// </summary>
public interface IDocumentAnalyzer
{
    /// <summary>
    /// Analisa a legibilidade de um documento
    /// </summary>
    /// <param name="documentPath">Caminho do arquivo do documento</param>
    /// <returns>Pontuação de legibilidade de 0 a 100</returns>
    Task<int> AnalyzeReadabilityAsync(string documentPath);

    /// <summary>
    /// Classifica o tipo de um documento
    /// </summary>
    /// <param name="documentPath">Caminho do arquivo do documento</param>
    /// <returns>Tipo do documento classificado</returns>
    Task<DocumentType> ClassifyDocumentTypeAsync(string documentPath);

    /// <summary>
    /// Extrai texto de um documento
    /// </summary>
    /// <param name="documentPath">Caminho do arquivo do documento</param>
    /// <returns>Texto extraído do documento</returns>
    Task<string> ExtractTextAsync(string documentPath);
}
