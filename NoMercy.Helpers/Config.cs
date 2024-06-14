namespace NoMercy.Helpers;

public class Config
{
    
    public static readonly string AuthBaseUrl = "https://auth-dev.nomercy.tv/realms/NoMercyTV/";
    public static readonly string TokenClientSecret = "1lHWBazSTHfBpuIzjAI6xnNjmwUnryai";
    public static readonly string TokenClientId = "nomercy-server";
    
    public static readonly string ApiBaseUrl = "https://api-dev.nomercy.tv/";
    public static readonly string ApiServerBaseUrl = $"{ApiBaseUrl}/v1/server/";
    
    
    public static readonly ushort ServerPort = 7626;
    
}