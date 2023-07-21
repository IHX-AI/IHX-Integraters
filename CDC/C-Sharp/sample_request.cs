using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

public static class FileUploader
{
    public static async Task<string> SendFileWithMetadata(string filePath, string metadata, string processInfo, string apikey, string token, string apiUrl)
    {
        using (var client = new HttpClient())
        {
            // Create a new HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("x-api-key", apikey);
            request.Headers.Add("access-token", token);

            // Create a multipart form data content
            var multipartContent = new MultipartFormDataContent();

            // Create a FileStream for reading the file
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Create a StreamContent from the FileStream and add it to the multipart content
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf"); // Specify content type as PDF
                multipartContent.Add(fileContent, "document", Path.GetFileName(filePath));

                // Add the metadata as a string content to the multipart content
                var metadataContent = new StringContent(metadata);
                metadataContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                multipartContent.Add(metadataContent, "metadata");

                // Add the metadata as a string content to the multipart content
                var processContent = new StringContent(processInfo);
                processContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                multipartContent.Add(processContent, "process_info");

                // Set the multipart content as the request's content
                request.Content = multipartContent;

                // Send the request and retrieve the response
                var response = await client.SendAsync(request);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var responseContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("File and metadata successfully sent.");
                    Console.WriteLine("Response content: " + responseContent);

                    return responseContent;
                }
                else
                {
                    Console.WriteLine($"Error sending file and metadata. StatusCode: {response.StatusCode}");
                    return null;
                }
            }
        }
    }
    public static async Task<string> Authorize(string identityURL,string clientID,string clientSecret,string username,string password,string grantType,string apiKey)
        {
        var data = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", clientID },
            { "username", username },
            { "password", password },
            { "grant_type", grantType },
            { "client_secret", clientSecret }
        });

        try
        {
            Console.WriteLine("Sending Request to Authorize");
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            var response = await client.PostAsync(identityURL, data);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Authorized Successfully");
                var responseContent = await response.Content.ReadAsStringAsync();
                // Parse the JSON response to retrieve the access_token
                var json = JObject.Parse(responseContent);
                var token = (string)json["access_token"];

                return token;
            }
            else
            {
                Console.WriteLine("Authorization Failed");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public static void Main()
    {
        string identityURL = "<identity-url>";
        string clientID = "<client-id>";
        string clientSecret = "<client-secret>";
        string username = "<username>";
        string password = "<password>";
        string grantType = "<grant-type>";
        string apikey = "<x-api-key";
        //get the access-token
        string token = Authorize(identityURL, clientID, clientSecret, username, password, grantType, apikey).GetAwaiter().GetResult();

        string apiUrl = "<api-url>";
        string pdfFilePath = "<pdf-file-path>";
        string metadata = "{\"IHXPayerID\":\"<ihx-payer-id>\"}";
        string processInfo = "{\"KYC_Classification\":\"true\", \"Page_Error\":\"IGNORE\"}";

        string responseContent = SendFileWithMetadata(pdfFilePath, metadata, processInfo, apikey, token, apiUrl).GetAwaiter().GetResult();

        // Further processing with the response content
        Console.WriteLine("Results: " + responseContent);
    }
}
