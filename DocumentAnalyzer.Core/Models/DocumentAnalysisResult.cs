namespace DocumentAnalyzer.Core.Models
{
    /// <summary>
    /// Resultado da análise de um documento
    /// </summary>
    public class DocumentAnalysisResult
    {
        /// <summary>
        /// Nome original do arquivo
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do documento identificado
        /// </summary>
        public DocumentType DocumentType { get; set; } = DocumentType.Unknown;

        /// <summary>
        /// Pontuação de legibilidade (0-100)
        /// </summary>
        public int ReadabilityScore { get; set; }

        /// <summary>
        /// Texto extraído do documento
        /// </summary>
        public string ExtractedText { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o documento é legível (pontuação > 50)
        /// </summary>
        public bool IsReadable => ReadabilityScore > 50;

        /// <summary>
        /// Confiança na classificação do tipo de documento (0-100)
        /// </summary>
        public int ClassificationConfidence { get; set; }
    }
}