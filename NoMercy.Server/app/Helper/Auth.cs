using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Helper;

public static class Auth
{
    private static string BaseUrl => Config.AuthBaseUrl;
    private static readonly string TokenUrl = $"{BaseUrl}protocol/openid-connect/token";

    private static string? PublicKey { get; set; }
    private static string? TokenClientId => Config.TokenClientId;
    private static string? TokenClientSecret => Config.TokenClientSecret;

    private static string? RefreshToken { get; set; }
    public static string? AccessToken { get; private set; }
    private static int? ExpiresIn { get; set; }

    private static int? NotBefore { get; set; }

    private static JwtSecurityToken? _jwtSecurityToken;

    private static IWebHost? TempServer { get; set; }

    public static Task Init()
    {
        if (!File.Exists(AppFiles.TokenFile)) File.WriteAllText(AppFiles.TokenFile, "{}");

        AuthKeys();

        AccessToken = GetAccessToken();
        RefreshToken = GetRefreshToken();
        ExpiresIn = TokenExpiration();
        NotBefore = TokenNotBefore();

        if (AccessToken == null || RefreshToken == null || ExpiresIn == null)
        {
            TokenByBrowser();
            return Task.CompletedTask;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        _jwtSecurityToken = tokenHandler.ReadJwtToken(AccessToken);

        var expiresInDays = _jwtSecurityToken.ValidTo.AddDays(-5).Subtract(DateTime.UtcNow).Days;

        var expired = NotBefore == null && expiresInDays >= 0;

        if (!expired)
            TokenByRefreshGrand();
        else
            TokenByBrowser();

        if (AccessToken == null || RefreshToken == null || ExpiresIn == null)
            throw new Exception("Failed to get tokens");

        return Task.CompletedTask;
    }

    private static void TokenByBrowser()
    {
        var baseUrl = new Uri($"{BaseUrl}/protocol/openid-connect/auth");
        var redirectUri = HttpUtility.UrlEncode($"http://localhost:{Networking.InternalServerPort}/sso-callback");

        IEnumerable<string> query = new Dictionary<string, string>
        {
            ["redirect_uri"] = redirectUri,
            ["client_id"] = "nomercy-server",
            ["response_type"] = "code",
            ["scope"] = "openid offline_access email profile"
        }.Select(x => $"{x.Key}={x.Value}");

        var url = new Uri($"{baseUrl}?{string.Join("&", query)}").ToString();

        TempServer = Networking.TempServer();
        TempServer.StartAsync().Wait();

        OpenBrowser(url);

        CheckToken();
    }

    private static void CheckToken()
    {
        Task.Run(async () =>
        {
            await Task.Delay(1000);

            if (AccessToken == null || RefreshToken == null || ExpiresIn == null)
                CheckToken();
            else
                TempServer?.StopAsync().Wait();
        }).Wait();
    }

    private static void SetTokens(string response)
    {
        dynamic data = JsonConvert.DeserializeObject(response)
                       ?? throw new Exception("Failed to deserialize JSON");

        if (data.access_token == null || data.refresh_token == null || data.expires_in == null)
        {
            File.Delete(AppFiles.TokenFile);
            TokenByBrowser();

            return;
        }

        var tmp = File.OpenWrite(AppFiles.TokenFile);
        tmp.SetLength(0);
        tmp.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data, Formatting.Indented)));
        tmp.Close();

        Logger.Auth(@"Tokens refreshed");

        AccessToken = data.access_token;
        RefreshToken = data.refresh_token;
        ExpiresIn = data.expires_in;
        NotBefore = data["not-before-policy"];
    }

    private static dynamic TokenData()
    {
        return JsonConvert.DeserializeObject(File.ReadAllText(AppFiles.TokenFile))
               ?? throw new Exception("Failed to deserialize JSON");
    }

    private static string? GetAccessToken()
    {
        var data = TokenData();

        return data.access_token;
    }

    private static string? GetRefreshToken()
    {
        var data = TokenData();
        return data.refresh_token;
    }

    private static int? TokenExpiration()
    {
        var data = TokenData();
        return data.expires_in;
    }

    private static int? TokenNotBefore()
    {
        var data = TokenData();
        return data["not-before-policy"];
    }

    private static void AuthKeys()
    {
        Logger.Auth(@"Getting auth keys");

        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetStringAsync(BaseUrl).Result;

        dynamic data = JsonConvert.DeserializeObject(response)
                       ?? throw new Exception("Failed to deserialize JSON");

        PublicKey = data.public_key;
    }

    private static void TokenByPasswordGrant(string username, string password)
    {
        if (TokenClientId == null || TokenClientSecret == null)
            throw new Exception("Auth keys not initialized");

        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        List<KeyValuePair<string, string>> body = new()
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", TokenClientId),
            new KeyValuePair<string, string>("client_secret", TokenClientSecret),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

        var response = client.PostAsync(TokenUrl, new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;

        SetTokens(response);
    }

    private static void TokenByRefreshGrand()
    {
        if (TokenClientId == null || TokenClientSecret == null || RefreshToken == null || _jwtSecurityToken == null)
            throw new Exception("Auth keys not initialized");

        var expiresInDays = _jwtSecurityToken.ValidTo.AddDays(-5).Subtract(DateTime.UtcNow).Days;
        if (expiresInDays >= 0)
        {
            Logger.Auth($"Token is still valid for {expiresInDays} day{(expiresInDays == 1 ? "" : "s")}");
            return;
        }

        Logger.Auth(@"Refreshing token");

        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        List<KeyValuePair<string, string>> body = new()
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", TokenClientId),
            new KeyValuePair<string, string>("client_secret", TokenClientSecret),
            new KeyValuePair<string, string>("refresh_token", RefreshToken),
            new KeyValuePair<string, string>("scope", "openid offline_access email profile")
        };

        var response = client.PostAsync(TokenUrl, new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;

        SetTokens(response);
    }

    public static void TokenByAuthorizationCode(string code)
    {
        Logger.Auth(@"Getting token by authorization code");
        if (TokenClientId == null || TokenClientSecret == null)
            throw new Exception("Auth keys not initialized");

        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var body = new List<KeyValuePair<string, string>>()
        {
            new("grant_type", "authorization_code"),
            new("client_id", TokenClientId),
            new("client_secret", TokenClientSecret),
            new("scope", "openid offline_access email profile"),
            new("redirect_uri", $"http://localhost:{Networking.InternalServerPort}/sso-callback"),
            new("code", code)
        };

        var response = client.PostAsync(TokenUrl, new FormUrlEncodedContent(body))
            .Result.Content.ReadAsStringAsync().Result;

        SetTokens(response);
    }

    private static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Process.Start("xdg-open", url); // Works ok on linux
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            Process.Start("open", url); // Not tested
        else
            throw new Exception("Unsupported OS");
    }
}