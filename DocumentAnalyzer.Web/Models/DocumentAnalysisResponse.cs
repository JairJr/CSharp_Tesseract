using DocumentAnalyzer.Core.Models;

namespace DocumentAnalyzer.Web.Models
{
    /// <summary>
    /// Modelo de resposta para análise de documentos
    /// </summary>
    public class DocumentAnalysisResponse
    {
        /// <summary>
        /// Indica se a análise foi bem-sucedida
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensagem de erro (se houver)
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Nome do arquivo analisado
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do documento identificado
        /// </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação de legibilidade (0-100)
        /// </summary>
        public int ReadabilityScore { get; set; }

        /// <summary>
        /// Indica se o documento é legível
        /// </summary>
        public bool IsReadable { get; set; }

        /// <summary>
        /// Confiança na classificação (0-100)
        /// </summary>
        public int ClassificationConfidence { get; set; }

        /// <summary>
        /// Texto extraído do documento (limitado para a resposta)
        /// </summary>
        public string ExtractedTextPreview { get; set; } = string.Empty;

        /// <summary>
        /// Cria uma resposta a partir do resultado da análise
        /// </summary>
        public static DocumentAnalysisResponse FromResult(DocumentAnalysisResult result)
        {
            return new DocumentAnalysisResponse
            {
                Success = true,
                FileName = result.FileName,
                DocumentType = result.DocumentType.ToString(),
                ReadabilityScore = result.ReadabilityScore,
                IsReadable = result.IsReadable,
                ClassificationConfidence = result.ClassificationConfidence,
                ExtractedTextPreview = TruncateText(result.ExtractedText, 200)
            };
        }

        /// <summary>
        /// Cria uma resposta de erro
        /// </summary>
        public static DocumentAnalysisResponse Error(string fileName, string errorMessage)
        {
            return new DocumentAnalysisResponse
            {
                Success = false,
                FileName = fileName,
                ErrorMessage = errorMessage
            };
        }

        private static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength) + "...";
        }
    }
}