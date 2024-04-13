#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Diagnostics;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.AcoustId.Models;

namespace NoMercy.Providers.AcoustId.Client;

public class FingerprintClient : BaseClient
{
    public FingerprintClient()
    {
        AcoustID.Configuration.ClientKey = ApiInfo.AcousticId;
    }

    private Task<Fingerprint?> WithFingerprint(string[] appendices, FingerPrintData fingerprintData, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["client"] = ApiInfo.AcousticId,
            ["duration"] = fingerprintData.Duration.ToString(),
            ["fingerprint"] = fingerprintData.Fingerprint,
        };
        
        return Get<Fingerprint>("lookup?meta=" + string.Join("+", appendices), queryParams, priority);
    }

    public Task<Fingerprint?> Lookup(string file, bool? priority = false)
    {
        
        Process process = new Process();
        process.StartInfo.FileName = AppFiles.FpCalcPath; 
        process.StartInfo.Arguments = "-json \"" + file + "\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        
        process.WaitForExit();
        
        FingerPrintData? fingerprintData = JsonHelper.FromJson<FingerPrintData>(output);
        
        if (fingerprintData == null)
        {
            throw new Exception("Fingerprint data is null");
        }
        
        return WithFingerprint([
            "recordings",
            "releases",
            "tracks",
            "compress",
            "usermeta",
            "sources",
        ], fingerprintData, priority);
    }
    
}

public class FingerPrintData
{
    [JsonProperty("duration")]
    
    public double _duration { get; set; }
    
    [JsonProperty("fingerprint")] public string Fingerprint { get; set; }
    
    public int Duration
    {
        get => (int) Math.Floor(_duration);
        set => _duration = value;
    }
}
