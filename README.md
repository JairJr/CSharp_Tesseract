# Analisador de Documentos

Uma ferramenta em C# para análise e classificação de documentos que os usuários fazem upload em um portal. A aplicação avalia a qualidade e legibilidade dos documentos, além de classificá-los em diferentes categorias como documentos de identidade, fotos, notas fiscais e comprovantes de residência.

## Funcionalidades

- **Upload de Documentos**: Interface amigável para envio de arquivos
- **Análise de Legibilidade**: Avaliação da qualidade e legibilidade do documento
- **Classificação Automática**: Identificação do tipo de documento
- **Extração de Texto**: Utiliza OCR para extrair texto de imagens e PDFs
- **API RESTful**: Endpoints para integração com outros sistemas

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework web para a API e interface
- **C#**: Linguagem de programação principal
- **Tesseract OCR**: Para extração de texto de imagens
- **ML.NET**: Para classificação de documentos
- **SixLabors.ImageSharp**: Para processamento de imagens
- **iText7**: Para processamento de PDFs
- **Bootstrap**: Para a interface de usuário

## Estrutura do Projeto

### DocumentAnalyzer.Core

Biblioteca principal com a lógica de negócios:

- **Models**: Definições de tipos de documentos e resultados de análise
- **Interfaces**: Contratos para os serviços de análise
- **Services**: Implementações dos serviços de análise

### DocumentAnalyzer.Web

Aplicação web com API e interface de usuário:

- **Controllers**: Endpoints da API para upload e análise
- **Models**: DTOs para requisições e respostas
- **wwwroot**: Interface de usuário em HTML/CSS/JavaScript

## Como Executar

### Pré-requisitos

- .NET 7.0 SDK ou superior
- Dados de treinamento do Tesseract OCR (para português)

### Passos para Execução

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Baixe os dados do Tesseract para a pasta `DocumentAnalyzer.Web/tessdata`
4. Execute o comando: `dotnet run --project DocumentAnalyzer.Web`
5. Acesse a aplicação em `https://localhost:5001` ou `http://localhost:5000`

## Uso da API

### Analisar Documento

```http
POST /api/document/analyze
```

Corpo da requisição (multipart/form-data):
- `document`: Arquivo do documento a ser analisado
- `description`: Descrição opcional do documento

Resposta:
```json
{
  "success": true,
  "fileName": "exemplo.pdf",
  "documentType": "Invoice",
  "readabilityScore": 85,
  "isReadable": true,
  "classificationConfidence": 92,
  "extractedTextPreview": "Texto extraído do documento..."
}
```

Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.
