using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly string subscriptionKey = "chave";
    private static readonly string endpoint = "endpoint";
    private static readonly string region = "regiao";

    static async Task Main(string[] args)
    {
        // Caminho do documento
        string filePath = "caminho/do/seu/documento.txt"; 

        // Ler o conteúdo do arquivo
        string textToTranslate = await ReadDocument(filePath);
        
        string targetLanguage = "pt"; 

        if (!string.IsNullOrEmpty(textToTranslate))
        {
            string translatedText = await TranslateText(textToTranslate, targetLanguage);
            Console.WriteLine($"Texto traduzido: {translatedText}");
        }
        else
        {
            Console.WriteLine("O documento está vazio ou não foi encontrado.");
        }
    }

    // Método para ler o conteúdo de um documento de texto
    static async Task<string> ReadDocument(string filePath)
    {
        try
        {
            // Verifica se o arquivo existe
            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else
            {
                Console.WriteLine("Arquivo não encontrado.");
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler o documento: {ex.Message}");
            return string.Empty;
        }
    }

    // Método para traduzir o texto usando a API do Azure Translator
    static async Task<string> TranslateText(string text, string language)
    {
        string route = $"/translate?api-version=3.0&to={language}";
        object[] body = new object[] { new { Text = text } };
        var requestBody = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);

            HttpResponseMessage response = await client.PostAsync(endpoint + route, requestBody);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
