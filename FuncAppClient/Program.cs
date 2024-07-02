using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        // URL of the Azure Function
        string functionUrl = "https://myfunctionapp323.azurewebsites.net/api/UploadToBlobFunction";

        // Replace <your_function_key> with your actual function key if required
        string functionKey = "Ud9AXPv5HU6FCFoEwMU9VOcmZXmwZ0YP_CSGsJBPoaXgAzFu1LcJBA==";

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
