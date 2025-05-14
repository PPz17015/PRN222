

using System.IO;

using System;
using System.Net;
using System.Net.Http;

class program
{

    //static void Main(String [] args)
    //{
    //    WebRequest request = WebRequest.Create("http://www.google.com");
    //    request.Credentials = CredentialCache.DefaultCredentials;
    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //    Console.WriteLine("Response Status Code: " + response.StatusDescription);
    //    Console.WriteLine(new string('-', 50));
    //    Stream dataStream = response.GetResponseStream();
    //    StreamReader streamReader = new StreamReader(dataStream);
    //    String ResponseFromServer = streamReader.ReadToEnd();
    //    Console.WriteLine(ResponseFromServer);
    //    Console.WriteLine(new string
    //        ('-', 50));
    //    streamReader.Close();
    //    dataStream.Close();
    //    response.Close();
    //    Console.WriteLine("Press any key to exit.");

    //}

    static readonly HttpClient client = new HttpClient();


    static async Task Main()
    {   
        String uri= "http://www.google.com";
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);

        }



    }

}