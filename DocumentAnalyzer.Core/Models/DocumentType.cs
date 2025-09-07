namespace DocumentAnalyzer.Core.Models
{
    /// <summary>
    /// Tipos de documentos que o sistema pode classificar
    /// </summary>
    public enum DocumentType
    {
        Unknown = 0,
        Identity = 1,        // Documentos de identidade (RG, CNH, etc.)
        Photo = 2,           // Fotografias
        Invoice = 3,         // Notas fiscais
        AddressProof = 4,    // Comprovantes de residência
        Other = 99           // Outros tipos de documentos
    }
}