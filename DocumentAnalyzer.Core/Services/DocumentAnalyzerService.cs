using DocumentAnalyzer.Core.Interfaces;
using DocumentAnalyzer.Core.Models;
using Microsoft.ML;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;
using Tesseract;

namespace DocumentAnalyzer.Core.Services
{
    /// <summary>
    /// Implementação do serviço de análise de documentos
    /// </summary>
    public class DocumentAnalyzerService : IDocumentAnalyzer
    {
        private readonly MLContext _mlContext;
        private readonly string _tesseractDataPath;

        public DocumentAnalyzerService(string tesseractDataPath = "./tessdata")
        {
            _mlContext = new MLContext(seed: 0);
            _tesseractDataPath = tesseractDataPath;
        }

        /// <summary>
        /// Analisa a legibilidade de um documento
        /// </summary>
        public async Task<int> AnalyzeReadabilityAsync(string documentPath)
        {
            // Extrair texto do documento
            string extractedText = await ExtractTextAsync(documentPath);
            
            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return 0; // Documento não contém texto legível
            }

            // Calcular pontuação de legibilidade baseada em vários fatores
            int score = CalculateReadabilityScore(extractedText, documentPath);
            
            return score;
        }

        /// <summary>
        /// Classifica o tipo de um documento
        /// </summary>
        public async Task<DocumentType> ClassifyDocumentTypeAsync(string documentPath)
        {
            string extractedText = await ExtractTextAsync(documentPath);
            string fileExtension = Path.GetExtension(documentPath).ToLowerInvariant();
            
            // Classificação baseada em regras simples (em um cenário real, usaríamos ML treinado)
            
            // Verificar se é uma foto
            if (IsImageFile(fileExtension) && string.IsNullOrWhiteSpace(extractedText))
            {
                return DocumentType.Photo;
            }
            
            // Verificar se é um documento de identidade
            if (ContainsIdentityKeywords(extractedText))
            {
                return DocumentType.Identity;
            }
            
            // Verificar se é uma nota fiscal
            if (ContainsInvoiceKeywords(extractedText))
            {
                return DocumentType.Invoice;
            }
            
            // Verificar se é um comprovante de residência
            if (ContainsAddressProofKeywords(extractedText))
            {
                return DocumentType.AddressProof;
            }
            
            // Se não conseguimos classificar, retorna desconhecido
            return DocumentType.Unknown;
        }

        /// <summary>
        /// Extrai texto de um documento usando OCR
        /// </summary>
        public async Task<string> ExtractTextAsync(string documentPath)
        {
            string fileExtension = Path.GetExtension(documentPath).ToLowerInvariant();
            
            // Processar diferentes tipos de arquivo
            if (IsImageFile(fileExtension))
            {
                return await ExtractTextFromImageAsync(documentPath);
            }
            else if (fileExtension == ".pdf")
            {
                return await ExtractTextFromPdfAsync(documentPath);
            }
            else
            {
                // Para outros tipos de arquivo, tentar ler como texto
                try
                {
                    return await File.ReadAllTextAsync(documentPath);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        #region Métodos auxiliares

        private async Task<string> ExtractTextFromImageAsync(string imagePath)
        {
            try
            {
                using var engine = new TesseractEngine(_tesseractDataPath, "por", EngineMode.Default);
                using var img = Pix.LoadFromFile(imagePath);
                using var page = engine.Process(img);
                
                return page.GetText();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao extrair texto da imagem: {ex.Message}");
                return string.Empty;
            }
        }

        private async Task<string> ExtractTextFromPdfAsync(string pdfPath)
        {
            // Em um cenário real, usaríamos iText7 para extrair texto de PDFs
            // Aqui, simplificamos para fins de demonstração
            return "Texto extraído do PDF (simulação)";
        }

        private int CalculateReadabilityScore(string text, string filePath)
        {
            // Fatores que afetam a legibilidade
            int textLength = text.Length;
            int wordCount = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            
            // Verificar se o texto está vazio ou muito curto
            if (textLength < 10 || wordCount < 3)
            {
                return 10; // Pontuação baixa para textos muito curtos
            }
            
            // Calcular pontuação base
            int baseScore = 50;
            
            // Ajustar com base no comprimento do texto (mais texto geralmente indica melhor legibilidade)
            baseScore += Math.Min(wordCount / 10, 20);
            
            // Verificar a qualidade do texto (presença de caracteres estranhos, etc.)
            double nonAlphanumericRatio = text.Count(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) / (double)textLength;
            if (nonAlphanumericRatio > 0.3)
            {
                baseScore -= 20; // Penalizar textos com muitos caracteres não alfanuméricos
            }
            
            // Limitar a pontuação entre 0 e 100
            return Math.Max(0, Math.Min(100, baseScore));
        }

        private bool IsImageFile(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || 
                   extension == ".bmp" || extension == ".tiff" || extension == ".gif";
        }

        private bool ContainsIdentityKeywords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
                
            string normalizedText = text.ToLowerInvariant();
            
            // Palavras-chave comuns em documentos de identidade
            string[] keywords = new[] { 
                "identidade", "rg", "cpf", "carteira", "nacional", "habilitação", 
                "cnh", "passaporte", "documento de identidade", "registro geral" 
            };
            
            return keywords.Any(keyword => normalizedText.Contains(keyword));
        }

        private bool ContainsInvoiceKeywords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
                
            string normalizedText = text.ToLowerInvariant();
            
            // Palavras-chave comuns em notas fiscais
            string[] keywords = new[] { 
                "nota fiscal", "nf-e", "nfe", "danfe", "cnpj", "imposto", 
                "icms", "valor total", "item", "quantidade", "preço unitário" 
            };
            
            return keywords.Any(keyword => normalizedText.Contains(keyword));
        }

        private bool ContainsAddressProofKeywords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
                
            string normalizedText = text.ToLowerInvariant();
            
            // Palavras-chave comuns em comprovantes de residência
            string[] keywords = new[] { 
                "conta", "fatura", "energia", "água", "gás", "telefone", 
                "internet", "residencial", "endereço", "cep" 
            };
            
            // Verificar padrão de CEP
            bool hasCepPattern = Regex.IsMatch(text, "\\d{5}-?\\d{3}");
            
            return keywords.Any(keyword => normalizedText.Contains(keyword)) || hasCepPattern;
        }

        #endregion
    }
}