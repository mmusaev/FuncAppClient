using Microsoft.Extensions.Configuration;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Add appsettings.json
          .Build();

        // Example of reading a configuration value
        string functionUrl = configuration["functionUrl"] ?? "samples";
        Console.WriteLine($"function url: {functionUrl}");


        // Replace <your_function_key> with your actual function key if required
        var functionKey = configuration["functionKey"] ?? "samples";

        // Sample data to upload
        string dataToUpload = "This is a sample text to upload to blob storage.";
        byte[] byteArray = Encoding.UTF8.GetBytes(dataToUpload);

        var client = new HttpClient();
        // If your function requires an authentication key, add it to the request header
        if (!string.IsNullOrEmpty(functionKey))
        {
            client.DefaultRequestHeaders.Add("x-functions-key", functionKey);
        }

        using (var content = new ByteArrayContent(byteArray))
        {
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // Send a POST request to the Azure Function
            HttpResponseMessage response = await client.PostAsync(functionUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Read and output the response body
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from Azure Function: " + responseBody);
            }
            else
            {
                Console.WriteLine("Failed to call Azure Function. Status code: " + response.StatusCode);
            }
            Console.ReadLine();
        }
    }
}
