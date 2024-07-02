using System.Text;

class Program
{
    private static readonly HttpClient client = new();

    static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .Build();

        string functionUrl = configuration["functionUrl"] ?? "samples";
        Console.WriteLine($"function url: {functionUrl}");

        var functionKey = configuration["functionKey"] ?? "samples";

        string dataToUpload = "This is a sample text to upload to blob storage.";
        byte[] byteArray = Encoding.UTF8.GetBytes(dataToUpload);

        if (!string.IsNullOrEmpty(functionKey))
        {
            client.DefaultRequestHeaders.Add("x-functions-key", functionKey);
        }

        using var content = new ByteArrayContent(byteArray);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        try
        {
            HttpResponseMessage response = await client.PostAsync(functionUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from Azure Function: " + responseBody);
            }
            else
            {
                Console.WriteLine("Failed to call Azure Function. Status code: " + response.StatusCode);
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request exception: {e.Message}");
        }
        Console.ReadLine();
    }
}
