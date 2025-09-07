namespace DocumentAnalyzer.Web.Models
{
    /// <summary>
    /// Modelo para requisição de upload de documento
    /// </summary>
    public class DocumentUploadRequest
    {
        /// <summary>
        /// Arquivo do documento a ser analisado
        /// </summary>
        public IFormFile Document { get; set; } = null!;

        /// <summary>
        /// Descrição opcional do documento
        /// </summary>
        public string? Description { get; set; }
    }
}