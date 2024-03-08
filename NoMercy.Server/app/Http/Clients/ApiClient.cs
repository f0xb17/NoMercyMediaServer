using System.Net.Http.Headers;
using Newtonsoft.Json;
using NoMercy.Server.app.Helper;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Clients;

public class ApiClient
{
    public static HttpClient Client { get; set; } = null!;
    
    public ApiClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        Client = new HttpClient(handler);
        
        Client.BaseAddress = new Uri("https://dev.nomercy.tv/api/v1/");
        
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);
        
    }

    private static T? Get<T>(string url)
    {
        return Client.GetAsync(url).ToJson<T>();
    }
    public dynamic? Get(string url)
    {
        return Get<dynamic>(url);
    }

    private static T? Post<T>(string url, HttpContent content)
    {
        return Client.PostAsync(url, content).ToJson<T>();
    }
    public dynamic? Post(string url, HttpContent content)
    {
        return Post<dynamic>(url, content);
    }

    private static T? Put<T>(string url, HttpContent content)
    {
        return Client.PutAsync(url, content).ToJson<T>();
    }
    public dynamic? Put(string url, HttpContent content)
    {
        return Put<dynamic>(url, content);
    }

    private static T? Delete<T>(string url)
    {
        return Client.DeleteAsync(url).ToJson<T>();
    }
    public dynamic? Delete(string url)
    {
        return Delete<dynamic>(url);
    }

    private static T? Patch<T>(string url, HttpContent content)
    {
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = content
        };
        
        return Client.SendAsync(request).ToJson<T>();
    }
    public dynamic? Patch(string url, HttpContent content)
    {
        return Patch<dynamic>(url, content);
    }

    private static T? Head<T>(string url)
    {
        return Client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)).ToJson<T>();
    }
    public dynamic? Head(string url)
    {
        return Head<dynamic>(url);
    }

    private static T? Options<T>(string url)
    {
        return Client.SendAsync(new HttpRequestMessage(HttpMethod.Options, url)).ToJson<T>();
    }
    public dynamic? Options(string url)
    {
        return Options<dynamic>(url);
    }

    private static T? Trace<T>(string url)
    {
        return Client.SendAsync(new HttpRequestMessage(HttpMethod.Trace, url)).ToJson<T>();
    }
    public dynamic? Trace(string url)
    {
        return Trace<dynamic>(url);
    }

    private static T? Send<T>(HttpRequestMessage request)
    {
        return Client.SendAsync(request).ToJson<T>();
    }
    public dynamic? Send(HttpRequestMessage request)
    {
        return Send<dynamic>(request);
    }
}

public static class ApiClientExtensions
{
    public static ApiClient WithToken(this ApiClient client, string token)
    {
        ApiClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
    
    public static dynamic? ToJson(this Task<HttpResponseMessage> response)
    {
        return JsonConvert.DeserializeObject<dynamic>(response.Result.Content.ReadAsStringAsync().Result);
    }
    public static T? ToJson<T>(this Task<HttpResponseMessage> response)
    {
        return JsonConvert.DeserializeObject<T>(response.Result.Content.ReadAsStringAsync().Result);
    }
}