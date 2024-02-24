using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Devices")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/devices", Order = 10)]
public class DevicesController : Controller
{
    [HttpGet]
    public DevicesDto[] Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return
        [
            new DevicesDto
            {
                Id = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                DeviceId = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2023-12-24 20:27:42"),
                UpdatedAt = DateTime.Parse("2023-12-24 20:27:42"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "07fa2fd0-c6f1-3caf-b074-b0a1438c591c",
                DeviceId = "07fa2fd0-c6f1-3caf-b074-b0a1438c591c",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "07fa2fd0-c6f1-3caf-b074-b0a1438c591c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "113.32.181.109",
                CreatedAt = DateTime.Parse("2024-01-26 08:28:21"),
                UpdatedAt = DateTime.Parse("2024-01-26 08:28:21"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "080c6d23-5f05-3dff-b6be-0d6805f1d441",
                DeviceId = "080c6d23-5f05-3dff-b6be-0d6805f1d441",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "Vue Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-19 06:11:06"),
                UpdatedAt = DateTime.Parse("2023-12-19 06:11:06"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "13c1c788-62dc-3b61-b210-11e3b91adc6f",
                DeviceId = "13c1c788-62dc-3b61-b210-11e3b91adc6f",
                Browser = "Chrome WebView",
                Os = "Android 10",
                Device = "Samsung SM-N960F",
                Type = "tv",
                Name = "13c1c788-62dc-3b61-b210-11e3b91adc6f",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-17 05:26:19"),
                UpdatedAt = DateTime.Parse("2024-01-17 05:26:19"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "1e92b81c-37bc-35b3-bcef-94e49603d37a",
                DeviceId = "1e92b81c-37bc-35b3-bcef-94e49603d37a",
                Browser = "Android",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "Nokia Streaming Box 8000",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2023-12-31 11:23:40"),
                UpdatedAt = DateTime.Parse("2023-12-31 11:23:40"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "281ac965-5e69-399d-b484-11fa45e0298a",
                DeviceId = "281ac965-5e69-399d-b484-11fa45e0298a",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-23 17:46:43"),
                UpdatedAt = DateTime.Parse("2024-01-23 17:46:43"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "287d91f2-4121-3292-b99d-1ab5d7d07463",
                DeviceId = "287d91f2-4121-3292-b99d-1ab5d7d07463",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "287d91f2-4121-3292-b99d-1ab5d7d07463",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-20 00:33:40"),
                UpdatedAt = DateTime.Parse("2023-12-20 00:33:40"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "2e01031e-544a-3df0-b671-665f5df1c048",
                DeviceId = "2e01031e-544a-3df0-b671-665f5df1c048",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "2e01031e-544a-3df0-b671-665f5df1c048",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-18 03:18:59"),
                UpdatedAt = DateTime.Parse("2024-01-18 03:18:59"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "3145f7e8-edb5-3bc9-b810-58d1129da6c1",
                DeviceId = "3145f7e8-edb5-3bc9-b810-58d1129da6c1",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "Vue Laptop Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.75",
                CreatedAt = DateTime.Parse("2023-12-19 06:11:13"),
                UpdatedAt = DateTime.Parse("2023-12-19 06:11:13"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "3188d3f7-5856-3a6d-bc3c-b63803a6d18e",
                DeviceId = "3188d3f7-5856-3a6d-bc3c-b63803a6d18e",
                Browser = "Chrome",
                Os = "Linux x86_64",
                Device = "",
                Type = "desktop",
                Name = "3188d3f7-5856-3a6d-bc3c-b63803a6d18e",
                CustomName = null,
                Version = "0.0.1",
                Ip = "76.247.64.178",
                CreatedAt = DateTime.Parse("2024-02-07 11:52:46"),
                UpdatedAt = DateTime.Parse("2024-02-07 11:52:46"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "3484f2f1-430a-353a-b768-ba9463e6d1f4",
                DeviceId = "3484f2f1-430a-353a-b768-ba9463e6d1f4",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "3484f2f1-430a-353a-b768-ba9463e6d1f4",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-01 10:46:00"),
                UpdatedAt = DateTime.Parse("2024-01-01 10:46:00"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "37e3817a-3c57-34ae-bca0-158bece55abb",
                DeviceId = "37e3817a-3c57-34ae-bca0-158bece55abb",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-23 04:01:49"),
                UpdatedAt = DateTime.Parse("2024-01-23 04:01:49"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "3ce88a2b-46ec-3ba9-b708-4a38979f7aa7",
                DeviceId = "3ce88a2b-46ec-3ba9-b708-4a38979f7aa7",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "3ce88a2b-46ec-3ba9-b708-4a38979f7aa7",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-01-03 21:44:47"),
                UpdatedAt = DateTime.Parse("2024-01-03 21:44:47"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "3d6c4e63-31c3-3c52-b779-1bb6bd853c62",
                DeviceId = "3d6c4e63-31c3-3c52-b779-1bb6bd853c62",
                Browser = "Chrome",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "3d6c4e63-31c3-3c52-b779-1bb6bd853c62",
                CustomName = null,
                Version = "0.0.1",
                Ip = "37.101.172.247",
                CreatedAt = DateTime.Parse("2024-02-03 22:19:53"),
                UpdatedAt = DateTime.Parse("2024-02-03 22:19:53"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "40d6e3ed-1e4f-330e-b80d-a14e68e365c7",
                DeviceId = "40d6e3ed-1e4f-330e-b80d-a14e68e365c7",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "40d6e3ed-1e4f-330e-b80d-a14e68e365c7",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-19 06:11:33"),
                UpdatedAt = DateTime.Parse("2023-12-19 06:11:33"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "42ce1a2c-f1fb-3617-b534-b647a98757a8",
                DeviceId = "42ce1a2c-f1fb-3617-b534-b647a98757a8",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "42ce1a2c-f1fb-3617-b534-b647a98757a8",
                CustomName = null,
                Version = "0.0.1",
                Ip = "2.229.51.240",
                CreatedAt = DateTime.Parse("2024-02-01 14:04:44"),
                UpdatedAt = DateTime.Parse("2024-02-01 14:04:44"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "445e1040-1e6d-3bba-b912-cb2304d1a018",
                DeviceId = "445e1040-1e6d-3bba-b912-cb2304d1a018",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "445e1040-1e6d-3bba-b912-cb2304d1a018",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.75",
                CreatedAt = DateTime.Parse("2024-01-04 00:55:43"),
                UpdatedAt = DateTime.Parse("2024-01-04 00:55:43"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "44838f55-1c3d-3422-bfbe-6c384942476b",
                DeviceId = "44838f55-1c3d-3422-bfbe-6c384942476b",
                Browser = "Opera",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "44838f55-1c3d-3422-bfbe-6c384942476b",
                CustomName = null,
                Version = "0.0.1",
                Ip = "109.136.253.255",
                CreatedAt = DateTime.Parse("2024-01-29 10:36:14"),
                UpdatedAt = DateTime.Parse("2024-01-29 10:36:14"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "461b4210-c7dd-3f64-bd95-e83985cf6e05",
                DeviceId = "461b4210-c7dd-3f64-bd95-e83985cf6e05",
                Browser = "Safari",
                Os = "Mac OS 10.15.7",
                Device = "Apple iPad",
                Type = "tablet",
                Name = "461b4210-c7dd-3f64-bd95-e83985cf6e05",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-01-03 20:41:03"),
                UpdatedAt = DateTime.Parse("2024-01-03 20:41:03"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "4638e9a4-910b-3711-b7f7-765e33d2a49d",
                DeviceId = "4638e9a4-910b-3711-b7f7-765e33d2a49d",
                Browser = "",
                Os = " ",
                Device = "",
                Type = "desktop",
                Name = "4638e9a4-910b-3711-b7f7-765e33d2a49d",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-31 11:23:25"),
                UpdatedAt = DateTime.Parse("2023-12-31 11:23:25"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "46f7677a-8df2-35ca-b1f1-52dfc29dc57f",
                DeviceId = "46f7677a-8df2-35ca-b1f1-52dfc29dc57f",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "46f7677a-8df2-35ca-b1f1-52dfc29dc57f",
                CustomName = null,
                Version = "0.0.1",
                Ip = "72.252.30.35",
                CreatedAt = DateTime.Parse("2024-01-27 08:41:45"),
                UpdatedAt = DateTime.Parse("2024-01-27 08:41:45"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "4d20b6cf-4289-33f3-b44d-9111be7ca9be",
                DeviceId = "4d20b6cf-4289-33f3-b44d-9111be7ca9be",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "4d20b6cf-4289-33f3-b44d-9111be7ca9be",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-21 03:44:33"),
                UpdatedAt = DateTime.Parse("2023-12-21 03:44:33"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8",
                DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8",
                Browser = "Chrome",
                Os = "Linux x86_64",
                Device = "",
                Type = "desktop",
                Name = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8",
                CustomName = null,
                Version = "0.0.1",
                Ip = "75.80.49.225",
                CreatedAt = DateTime.Parse("2024-02-20 09:12:43"),
                UpdatedAt = DateTime.Parse("2024-02-20 09:12:43"),
                ActivityLogs =
                [
                    new ActivityLogDto
                    {
                        Id = 13096,
                        Type = "Connected",
                        Time = 1708420363651,
                        CreatedAt = DateTime.Parse("2024-02-20 09:12:43"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:12:43"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13097,
                        Type = "Connected",
                        Time = 1708420417695,
                        CreatedAt = DateTime.Parse("2024-02-20 09:13:37"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:13:37"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13099,
                        Type = "Connected",
                        Time = 1708420418404,
                        CreatedAt = DateTime.Parse("2024-02-20 09:13:38"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:13:38"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13102,
                        Type = "Connected",
                        Time = 1708420520458,
                        CreatedAt = DateTime.Parse("2024-02-20 09:15:20"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:15:20"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13104,
                        Type = "Connected",
                        Time = 1708420521162,
                        CreatedAt = DateTime.Parse("2024-02-20 09:15:21"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:15:21"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13098,
                        Type = "Disconnected",
                        Time = 1708420417904,
                        CreatedAt = DateTime.Parse("2024-02-20 09:13:37"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:13:37"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13100,
                        Type = "Disconnected",
                        Time = 1708420433641,
                        CreatedAt = DateTime.Parse("2024-02-20 09:13:53"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:13:53"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13101,
                        Type = "Disconnected",
                        Time = 1708420480103,
                        CreatedAt = DateTime.Parse("2024-02-20 09:14:40"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:14:40"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13103,
                        Type = "Disconnected",
                        Time = 1708420520644,
                        CreatedAt = DateTime.Parse("2024-02-20 09:15:20"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:15:20"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    },
                    new ActivityLogDto
                    {
                        Id = 13105,
                        Type = "Disconnected",
                        Time = 1708420534504,
                        CreatedAt = DateTime.Parse("2024-02-20 09:15:34"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:15:34"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "4e5aecc9-8776-3b1d-b613-ef84ceee75b8"
                    }
                ]
            },
            new DevicesDto
            {
                Id = "5153ba28-3a63-3bea-bd4b-a8a05f5e40e0",
                DeviceId = "5153ba28-3a63-3bea-bd4b-a8a05f5e40e0",
                Browser = "Brave",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-02-04 12:11:45"),
                UpdatedAt = DateTime.Parse("2024-02-04 12:11:45"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "53fab257-b036-36a4-bc6c-924189dbc70d",
                DeviceId = "53fab257-b036-36a4-bc6c-924189dbc70d",
                Browser = "Firefox",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "728853c8-0163-3216-befa-16339cc64cde",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-02-06 15:22:54"),
                UpdatedAt = DateTime.Parse("2024-02-06 15:22:54"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "5d8e41c8-03c0-3696-bd3c-72ef5855d46c",
                DeviceId = "5d8e41c8-03c0-3696-bd3c-72ef5855d46c",
                Browser = "Brave",
                Os = " ",
                Device = "",
                Type = "mobile",
                Name = "Vue Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-26 23:29:12"),
                UpdatedAt = DateTime.Parse("2024-01-26 23:29:12"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "64d140d0-78cd-330b-ba92-590a4c363bb3",
                DeviceId = "64d140d0-78cd-330b-ba92-590a4c363bb3",
                Browser = "Brave",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "mobile",
                Name = "64d140d0-78cd-330b-ba92-590a4c363bb3",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-03 20:44:22"),
                UpdatedAt = DateTime.Parse("2024-01-03 20:44:22"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "657bd6e0-803c-3c21-ba07-0aa2b64b5456",
                DeviceId = "657bd6e0-803c-3c21-ba07-0aa2b64b5456",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "657bd6e0-803c-3c21-ba07-0aa2b64b5456",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-01-04 11:53:16"),
                UpdatedAt = DateTime.Parse("2024-01-04 11:53:16"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "678d2f40-ee6b-3a12-b016-78a6bac13f5a",
                DeviceId = "678d2f40-ee6b-3a12-b016-78a6bac13f5a",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "678d2f40-ee6b-3a12-b016-78a6bac13f5a",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-01 22:53:29"),
                UpdatedAt = DateTime.Parse("2024-01-01 22:53:29"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "68680fdf-7ee1-3153-be06-ae2c3e554cd6",
                DeviceId = "68680fdf-7ee1-3153-be06-ae2c3e554cd6",
                Browser = "Firefox",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "728853c8-0163-3216-befa-16339cc64cde",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-02-04 12:00:12"),
                UpdatedAt = DateTime.Parse("2024-02-04 12:00:12"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "6b3347fe-34b2-3079-b35c-d4eb1a4b30d8",
                DeviceId = "6b3347fe-34b2-3079-b35c-d4eb1a4b30d8",
                Browser = "Opera",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "44838f55-1c3d-3422-bfbe-6c384942476b",
                CustomName = null,
                Version = "0.0.1",
                Ip = "109.136.253.255",
                CreatedAt = DateTime.Parse("2024-02-06 19:18:30"),
                UpdatedAt = DateTime.Parse("2024-02-06 19:18:30"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "6cd06f7f-c2a6-32b3-b895-76105ba3cd81",
                DeviceId = "6cd06f7f-c2a6-32b3-b895-76105ba3cd81",
                Browser = "Chrome WebView",
                Os = "Android 10",
                Device = "Samsung SM-N960F",
                Type = "tv",
                Name = "6cd06f7f-c2a6-32b3-b895-76105ba3cd81",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-10 16:25:49"),
                UpdatedAt = DateTime.Parse("2024-01-10 16:25:49"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7096b9a9-a20f-3508-be8c-332ee03728bc",
                DeviceId = "7096b9a9-a20f-3508-be8c-332ee03728bc",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "Vue Laptop Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.75",
                CreatedAt = DateTime.Parse("2024-02-02 09:28:12"),
                UpdatedAt = DateTime.Parse("2024-02-02 09:28:12"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "70b1b193-8864-36c5-b6a6-7bf29cf75d06",
                DeviceId = "70b1b193-8864-36c5-b6a6-7bf29cf75d06",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9a3a1698-af1a-3549-b670-2215d1b69895",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-02-07 12:52:47"),
                UpdatedAt = DateTime.Parse("2024-02-07 12:52:47"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7917e23e-ce31-3d14-b4f6-b7843aac6a16",
                DeviceId = "7917e23e-ce31-3d14-b4f6-b7843aac6a16",
                Browser = "Brave",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "7917e23e-ce31-3d14-b4f6-b7843aac6a16",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-22 15:45:20"),
                UpdatedAt = DateTime.Parse("2024-01-22 15:45:20"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7a797dfc-12a0-370b-b09a-1fdbb869d5cb",
                DeviceId = "7a797dfc-12a0-370b-b09a-1fdbb869d5cb",
                Browser = "Brave",
                Os = "Android 11",
                Device = "",
                Type = "tv",
                Name = "Vue Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-16 20:43:04"),
                UpdatedAt = DateTime.Parse("2024-01-16 20:43:04"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7b130713-4598-3d89-ba7d-8d7f59ad684d",
                DeviceId = "7b130713-4598-3d89-ba7d-8d7f59ad684d",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9a3a1698-af1a-3549-b670-2215d1b69895",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-27 14:11:48"),
                UpdatedAt = DateTime.Parse("2024-01-27 14:11:48"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                DeviceId = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                Browser = "Brave",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-19 19:39:14"),
                UpdatedAt = DateTime.Parse("2024-01-19 19:39:14"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "7e6b113f-6b89-3c4f-b3c8-c7f48b85b5b0",
                DeviceId = "7e6b113f-6b89-3c4f-b3c8-c7f48b85b5b0",
                Browser = "Chrome WebView",
                Os = "Android 10",
                Device = "Samsung SM-N960F",
                Type = "tv",
                Name = "6cd06f7f-c2a6-32b3-b895-76105ba3cd81",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-19 19:51:22"),
                UpdatedAt = DateTime.Parse("2024-01-19 19:51:22"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "846b155a-4df4-3635-b2fe-1c9fb1347aa4",
                DeviceId = "846b155a-4df4-3635-b2fe-1c9fb1347aa4",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "846b155a-4df4-3635-b2fe-1c9fb1347aa4",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-27 02:18:36"),
                UpdatedAt = DateTime.Parse("2023-12-27 02:18:36"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "882666f7-47f6-31dd-b1dd-26ba749f586e",
                DeviceId = "882666f7-47f6-31dd-b1dd-26ba749f586e",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "2e01031e-544a-3df0-b671-665f5df1c048",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-23 04:15:07"),
                UpdatedAt = DateTime.Parse("2024-01-23 04:15:07"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "9497779c-f07d-3b9a-bfe8-5ad791bdf363",
                DeviceId = "9497779c-f07d-3b9a-bfe8-5ad791bdf363",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "9497779c-f07d-3b9a-bfe8-5ad791bdf363",
                CustomName = null,
                Version = "0.0.1",
                Ip = "2.229.51.239",
                CreatedAt = DateTime.Parse("2024-01-31 07:13:08"),
                UpdatedAt = DateTime.Parse("2024-01-31 07:13:08"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "9a2beb93-c401-3336-b0c6-2af4d5bbbaf6",
                DeviceId = "9a2beb93-c401-3336-b0c6-2af4d5bbbaf6",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9a2beb93-c401-3336-b0c6-2af4d5bbbaf6",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-01 17:16:33"),
                UpdatedAt = DateTime.Parse("2024-01-01 17:16:33"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "9a3a1698-af1a-3549-b670-2215d1b69895",
                DeviceId = "9a3a1698-af1a-3549-b670-2215d1b69895",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9a3a1698-af1a-3549-b670-2215d1b69895",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.75",
                CreatedAt = DateTime.Parse("2024-01-18 16:33:30"),
                UpdatedAt = DateTime.Parse("2024-01-18 16:33:30"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "9bcdf81a-8875-3ea3-bec6-727dbc5e82ab",
                DeviceId = "9bcdf81a-8875-3ea3-bec6-727dbc5e82ab",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "b436dbb1-0915-3c49-b8ac-9ede3fef833a",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2023-12-24 21:37:44"),
                UpdatedAt = DateTime.Parse("2023-12-24 21:37:44"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "9e6eff02-0488-3e94-be5c-6a42b5579334",
                DeviceId = "9e6eff02-0488-3e94-be5c-6a42b5579334",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9e6eff02-0488-3e94-be5c-6a42b5579334",
                CustomName = null,
                Version = "0.1.0",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2023-12-19 16:42:15"),
                UpdatedAt = DateTime.Parse("2023-12-19 16:42:15"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "a29b794f-4337-38b2-bc73-0f148a5dbb0e",
                DeviceId = "a29b794f-4337-38b2-bc73-0f148a5dbb0e",
                Browser = "Firefox",
                Os = "Linux x86_64",
                Device = "",
                Type = "desktop",
                Name = "a29b794f-4337-38b2-bc73-0f148a5dbb0e",
                CustomName = null,
                Version = "0.0.1",
                Ip = "75.80.49.225",
                CreatedAt = DateTime.Parse("2024-02-20 09:08:25"),
                UpdatedAt = DateTime.Parse("2024-02-20 09:08:25"),
                ActivityLogs =
                [
                    new ActivityLogDto
                    {
                        Id = 13094,
                        Type = "Connected",
                        Time = 1708420105602,
                        CreatedAt = DateTime.Parse("2024-02-20 09:08:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:08:25"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "a29b794f-4337-38b2-bc73-0f148a5dbb0e"
                    },
                    new ActivityLogDto
                    {
                        Id = 13095,
                        Type = "Disconnected",
                        Time = 1708420117112,
                        CreatedAt = DateTime.Parse("2024-02-20 09:08:37"),
                        UpdatedAt = DateTime.Parse("2024-02-20 09:08:37"),
                        UserId = "37d03e60-7b0a-4246-a85b-a5618966a383",
                        DeviceId = "a29b794f-4337-38b2-bc73-0f148a5dbb0e"
                    }
                ]
            },
            new DevicesDto
            {
                Id = "a699545e-e8c9-3ffa-bbb3-252756aa1f58",
                DeviceId = "a699545e-e8c9-3ffa-bbb3-252756aa1f58",
                Browser = "Chrome WebView",
                Os = "Android 10",
                Device = "Samsung SM-N960F",
                Type = "tv",
                Name = "a699545e-e8c9-3ffa-bbb3-252756aa1f58",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-03 14:47:30"),
                UpdatedAt = DateTime.Parse("2024-01-03 14:47:30"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "a70b5e9d-3929-3f68-b6ef-c1d5cfe431cc",
                DeviceId = "a70b5e9d-3929-3f68-b6ef-c1d5cfe431cc",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-18 00:47:41"),
                UpdatedAt = DateTime.Parse("2024-01-18 00:47:41"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "a847b127-9bbc-34fb-bf94-ee62836f8a3f",
                DeviceId = "a847b127-9bbc-34fb-bf94-ee62836f8a3f",
                Browser = "Firefox",
                Os = "Linux x86_64",
                Device = "",
                Type = "desktop",
                Name = "a847b127-9bbc-34fb-bf94-ee62836f8a3f",
                CustomName = null,
                Version = "0.0.1",
                Ip = "75.80.49.225",
                CreatedAt = DateTime.Parse("2024-02-13 23:23:00"),
                UpdatedAt = DateTime.Parse("2024-02-13 23:23:00"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "Vue Dev Brave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-02-02 10:54:35"),
                UpdatedAt = DateTime.Parse("2024-02-02 10:54:35"),
                ActivityLogs =
                [
                    new ActivityLogDto
                    {
                        Id = 13003,
                        Type = "Connected",
                        Time = 1708163141334,
                        CreatedAt = DateTime.Parse("2024-02-17 09:45:41"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:45:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13005,
                        Type = "Connected",
                        Time = 1708163214545,
                        CreatedAt = DateTime.Parse("2024-02-17 09:46:54"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:46:54"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13007,
                        Type = "Connected",
                        Time = 1708163242043,
                        CreatedAt = DateTime.Parse("2024-02-17 09:47:22"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:47:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13009,
                        Type = "Connected",
                        Time = 1708163827969,
                        CreatedAt = DateTime.Parse("2024-02-17 09:57:07"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:57:07"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13011,
                        Type = "Connected",
                        Time = 1708163828035,
                        CreatedAt = DateTime.Parse("2024-02-17 09:57:08"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:57:08"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13013,
                        Type = "Connected",
                        Time = 1708164382083,
                        CreatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UpdatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13015,
                        Type = "Connected",
                        Time = 1708164382150,
                        CreatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UpdatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13022,
                        Type = "Connected",
                        Time = 1708172279407,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13024,
                        Type = "Connected",
                        Time = 1708172279455,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13026,
                        Type = "Connected",
                        Time = 1708173379941,
                        CreatedAt = DateTime.Parse("2024-02-17 12:36:19"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:36:19"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13028,
                        Type = "Connected",
                        Time = 1708173380030,
                        CreatedAt = DateTime.Parse("2024-02-17 12:36:20"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:36:20"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13030,
                        Type = "Connected",
                        Time = 1708179533225,
                        CreatedAt = DateTime.Parse("2024-02-17 14:18:53"),
                        UpdatedAt = DateTime.Parse("2024-02-17 14:18:53"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13033,
                        Type = "Connected",
                        Time = 1708263197937,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:17"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:17"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13035,
                        Type = "Connected",
                        Time = 1708263224966,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:44"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:44"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13037,
                        Type = "Connected",
                        Time = 1708263225026,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:45"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:45"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13038,
                        Type = "Connected",
                        Time = 1708263229218,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:49"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:49"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13041,
                        Type = "Connected",
                        Time = 1708263274323,
                        CreatedAt = DateTime.Parse("2024-02-18 13:34:34"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:34:34"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13042,
                        Type = "Connected",
                        Time = 1708263961324,
                        CreatedAt = DateTime.Parse("2024-02-18 13:46:01"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:46:01"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13043,
                        Type = "Connected",
                        Time = 1708266281348,
                        CreatedAt = DateTime.Parse("2024-02-18 14:24:41"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:24:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13046,
                        Type = "Connected",
                        Time = 1708266305456,
                        CreatedAt = DateTime.Parse("2024-02-18 14:25:05"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:25:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13047,
                        Type = "Connected",
                        Time = 1708266309365,
                        CreatedAt = DateTime.Parse("2024-02-18 14:25:09"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:25:09"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13050,
                        Type = "Connected",
                        Time = 1708272275850,
                        CreatedAt = DateTime.Parse("2024-02-18 16:04:35"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:04:35"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13052,
                        Type = "Connected",
                        Time = 1708272389433,
                        CreatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13054,
                        Type = "Connected",
                        Time = 1708272389478,
                        CreatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13056,
                        Type = "Connected",
                        Time = 1708272449778,
                        CreatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13058,
                        Type = "Connected",
                        Time = 1708272449846,
                        CreatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13059,
                        Type = "Connected",
                        Time = 1708272641311,
                        CreatedAt = DateTime.Parse("2024-02-18 16:10:41"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:10:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13061,
                        Type = "Connected",
                        Time = 1708272765950,
                        CreatedAt = DateTime.Parse("2024-02-18 16:12:45"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:12:45"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13063,
                        Type = "Connected",
                        Time = 1708272766012,
                        CreatedAt = DateTime.Parse("2024-02-18 16:12:46"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:12:46"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13066,
                        Type = "Connected",
                        Time = 1708387468311,
                        CreatedAt = DateTime.Parse("2024-02-20 00:04:28"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:04:28"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13067,
                        Type = "Connected",
                        Time = 1708387472028,
                        CreatedAt = DateTime.Parse("2024-02-20 00:04:32"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:04:32"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13070,
                        Type = "Connected",
                        Time = 1708388027916,
                        CreatedAt = DateTime.Parse("2024-02-20 00:13:47"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:13:47"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13072,
                        Type = "Connected",
                        Time = 1708388075267,
                        CreatedAt = DateTime.Parse("2024-02-20 00:14:35"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:14:35"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13074,
                        Type = "Connected",
                        Time = 1708388285790,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13076,
                        Type = "Connected",
                        Time = 1708388285896,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13077,
                        Type = "Connected",
                        Time = 1708388311105,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:31"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:31"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13080,
                        Type = "Connected",
                        Time = 1708390767578,
                        CreatedAt = DateTime.Parse("2024-02-20 00:59:27"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:59:27"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13081,
                        Type = "Connected",
                        Time = 1708391028521,
                        CreatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13083,
                        Type = "Connected",
                        Time = 1708391028589,
                        CreatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13085,
                        Type = "Connected",
                        Time = 1708391994422,
                        CreatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13087,
                        Type = "Connected",
                        Time = 1708391994471,
                        CreatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13089,
                        Type = "Connected",
                        Time = 1708392065832,
                        CreatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13091,
                        Type = "Connected",
                        Time = 1708392065890,
                        CreatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13106,
                        Type = "Connected",
                        Time = 1708426072749,
                        CreatedAt = DateTime.Parse("2024-02-20 10:47:52"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:47:52"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13107,
                        Type = "Connected",
                        Time = 1708426132694,
                        CreatedAt = DateTime.Parse("2024-02-20 10:48:52"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:48:52"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13109,
                        Type = "Connected",
                        Time = 1708426184507,
                        CreatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13111,
                        Type = "Connected",
                        Time = 1708426184596,
                        CreatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13112,
                        Type = "Connected",
                        Time = 1708426342185,
                        CreatedAt = DateTime.Parse("2024-02-20 10:52:22"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:52:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13113,
                        Type = "Connected",
                        Time = 1708426402933,
                        CreatedAt = DateTime.Parse("2024-02-20 10:53:22"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:53:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13116,
                        Type = "Connected",
                        Time = 1708427390386,
                        CreatedAt = DateTime.Parse("2024-02-20 11:09:50"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:09:50"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13117,
                        Type = "Connected",
                        Time = 1708427534509,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13119,
                        Type = "Connected",
                        Time = 1708427534583,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13121,
                        Type = "Connected",
                        Time = 1708427545074,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13123,
                        Type = "Connected",
                        Time = 1708427545213,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13125,
                        Type = "Connected",
                        Time = 1708427848892,
                        CreatedAt = DateTime.Parse("2024-02-20 11:17:28"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:17:28"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13127,
                        Type = "Connected",
                        Time = 1708427862455,
                        CreatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13129,
                        Type = "Connected",
                        Time = 1708427862508,
                        CreatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13130,
                        Type = "Connected",
                        Time = 1708428188278,
                        CreatedAt = DateTime.Parse("2024-02-20 11:23:08"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:23:08"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13132,
                        Type = "Connected",
                        Time = 1708428437619,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:17"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:17"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13134,
                        Type = "Connected",
                        Time = 1708428473196,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13136,
                        Type = "Connected",
                        Time = 1708428473249,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13138,
                        Type = "Connected",
                        Time = 1708428629642,
                        CreatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13140,
                        Type = "Connected",
                        Time = 1708428629712,
                        CreatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13141,
                        Type = "Connected",
                        Time = 1708428751085,
                        CreatedAt = DateTime.Parse("2024-02-20 11:32:31"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:32:31"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13142,
                        Type = "Connected",
                        Time = 1708428904357,
                        CreatedAt = DateTime.Parse("2024-02-20 11:35:04"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:35:04"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13144,
                        Type = "Connected",
                        Time = 1708429405971,
                        CreatedAt = DateTime.Parse("2024-02-20 11:43:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:43:25"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13145,
                        Type = "Connected",
                        Time = 1708429452492,
                        CreatedAt = DateTime.Parse("2024-02-20 11:44:12"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:44:12"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13147,
                        Type = "Connected",
                        Time = 1708429596283,
                        CreatedAt = DateTime.Parse("2024-02-20 11:46:36"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:46:36"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13149,
                        Type = "Connected",
                        Time = 1708433180866,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:20"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:20"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13151,
                        Type = "Connected",
                        Time = 1708433186174,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13153,
                        Type = "Connected",
                        Time = 1708433186236,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13154,
                        Type = "Connected",
                        Time = 1708433969205,
                        CreatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13156,
                        Type = "Connected",
                        Time = 1708433969306,
                        CreatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13159,
                        Type = "Connected",
                        Time = 1708434481559,
                        CreatedAt = DateTime.Parse("2024-02-20 13:08:01"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:08:01"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13161,
                        Type = "Connected",
                        Time = 1708434703136,
                        CreatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13163,
                        Type = "Connected",
                        Time = 1708434703224,
                        CreatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13164,
                        Type = "Connected",
                        Time = 1708434793492,
                        CreatedAt = DateTime.Parse("2024-02-20 13:13:13"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:13:13"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13166,
                        Type = "Connected",
                        Time = 1708434807211,
                        CreatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13168,
                        Type = "Connected",
                        Time = 1708434807309,
                        CreatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13169,
                        Type = "Connected",
                        Time = 1708434964130,
                        CreatedAt = DateTime.Parse("2024-02-20 13:16:04"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:16:04"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13171,
                        Type = "Connected",
                        Time = 1708435016785,
                        CreatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13173,
                        Type = "Connected",
                        Time = 1708435016883,
                        CreatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13174,
                        Type = "Connected",
                        Time = 1708435178623,
                        CreatedAt = DateTime.Parse("2024-02-20 13:19:38"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:19:38"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13176,
                        Type = "Connected",
                        Time = 1708435258097,
                        CreatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13178,
                        Type = "Connected",
                        Time = 1708435258163,
                        CreatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13004,
                        Type = "Disconnected",
                        Time = 1708163198019,
                        CreatedAt = DateTime.Parse("2024-02-17 09:46:38"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:46:38"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13006,
                        Type = "Disconnected",
                        Time = 1708163240227,
                        CreatedAt = DateTime.Parse("2024-02-17 09:47:20"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:47:20"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13008,
                        Type = "Disconnected",
                        Time = 1708163825935,
                        CreatedAt = DateTime.Parse("2024-02-17 09:57:05"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:57:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13010,
                        Type = "Disconnected",
                        Time = 1708163828015,
                        CreatedAt = DateTime.Parse("2024-02-17 09:57:08"),
                        UpdatedAt = DateTime.Parse("2024-02-17 09:57:08"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13012,
                        Type = "Disconnected",
                        Time = 1708164378575,
                        CreatedAt = DateTime.Parse("2024-02-17 10:06:18"),
                        UpdatedAt = DateTime.Parse("2024-02-17 10:06:18"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13014,
                        Type = "Disconnected",
                        Time = 1708164382130,
                        CreatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UpdatedAt = DateTime.Parse("2024-02-17 10:06:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13023,
                        Type = "Disconnected",
                        Time = 1708172279420,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:59"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13025,
                        Type = "Disconnected",
                        Time = 1708173375577,
                        CreatedAt = DateTime.Parse("2024-02-17 12:36:15"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:36:15"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13027,
                        Type = "Disconnected",
                        Time = 1708173380002,
                        CreatedAt = DateTime.Parse("2024-02-17 12:36:20"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:36:20"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13029,
                        Type = "Disconnected",
                        Time = 1708177140584,
                        CreatedAt = DateTime.Parse("2024-02-17 13:39:00"),
                        UpdatedAt = DateTime.Parse("2024-02-17 13:39:00"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13031,
                        Type = "Disconnected",
                        Time = 1708180161661,
                        CreatedAt = DateTime.Parse("2024-02-17 14:29:21"),
                        UpdatedAt = DateTime.Parse("2024-02-17 14:29:21"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13032,
                        Type = "Disconnected",
                        Time = 1708191092727,
                        CreatedAt = DateTime.Parse("2024-02-17 17:31:32"),
                        UpdatedAt = DateTime.Parse("2024-02-17 17:31:32"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13034,
                        Type = "Disconnected",
                        Time = 1708263223197,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:43"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:43"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13036,
                        Type = "Disconnected",
                        Time = 1708263225009,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:45"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:45"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13039,
                        Type = "Disconnected",
                        Time = 1708263230489,
                        CreatedAt = DateTime.Parse("2024-02-18 13:33:50"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:33:50"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13040,
                        Type = "Disconnected",
                        Time = 1708263264457,
                        CreatedAt = DateTime.Parse("2024-02-18 13:34:24"),
                        UpdatedAt = DateTime.Parse("2024-02-18 13:34:24"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13044,
                        Type = "Disconnected",
                        Time = 1708266288932,
                        CreatedAt = DateTime.Parse("2024-02-18 14:24:48"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:24:48"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13045,
                        Type = "Disconnected",
                        Time = 1708266298133,
                        CreatedAt = DateTime.Parse("2024-02-18 14:24:58"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:24:58"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13048,
                        Type = "Disconnected",
                        Time = 1708266332713,
                        CreatedAt = DateTime.Parse("2024-02-18 14:25:32"),
                        UpdatedAt = DateTime.Parse("2024-02-18 14:25:32"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13049,
                        Type = "Disconnected",
                        Time = 1708272264912,
                        CreatedAt = DateTime.Parse("2024-02-18 16:04:24"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:04:24"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13051,
                        Type = "Disconnected",
                        Time = 1708272386618,
                        CreatedAt = DateTime.Parse("2024-02-18 16:06:26"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:06:26"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13053,
                        Type = "Disconnected",
                        Time = 1708272389452,
                        CreatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:06:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13055,
                        Type = "Disconnected",
                        Time = 1708272448035,
                        CreatedAt = DateTime.Parse("2024-02-18 16:07:28"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:07:28"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13057,
                        Type = "Disconnected",
                        Time = 1708272449828,
                        CreatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:07:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13060,
                        Type = "Disconnected",
                        Time = 1708272760100,
                        CreatedAt = DateTime.Parse("2024-02-18 16:12:40"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:12:40"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13062,
                        Type = "Disconnected",
                        Time = 1708272765992,
                        CreatedAt = DateTime.Parse("2024-02-18 16:12:45"),
                        UpdatedAt = DateTime.Parse("2024-02-18 16:12:45"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13064,
                        Type = "Disconnected",
                        Time = 1708277391641,
                        CreatedAt = DateTime.Parse("2024-02-18 17:29:51"),
                        UpdatedAt = DateTime.Parse("2024-02-18 17:29:51"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13065,
                        Type = "Disconnected",
                        Time = 1708333648287,
                        CreatedAt = DateTime.Parse("2024-02-19 09:07:28"),
                        UpdatedAt = DateTime.Parse("2024-02-19 09:07:28"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13068,
                        Type = "Disconnected",
                        Time = 1708387490609,
                        CreatedAt = DateTime.Parse("2024-02-20 00:04:50"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:04:50"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13069,
                        Type = "Disconnected",
                        Time = 1708387684967,
                        CreatedAt = DateTime.Parse("2024-02-20 00:08:04"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:08:04"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13071,
                        Type = "Disconnected",
                        Time = 1708388041930,
                        CreatedAt = DateTime.Parse("2024-02-20 00:14:01"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:14:01"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13073,
                        Type = "Disconnected",
                        Time = 1708388282799,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:02"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:02"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13075,
                        Type = "Disconnected",
                        Time = 1708388285875,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13078,
                        Type = "Disconnected",
                        Time = 1708388322794,
                        CreatedAt = DateTime.Parse("2024-02-20 00:18:42"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:18:42"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13079,
                        Type = "Disconnected",
                        Time = 1708390514465,
                        CreatedAt = DateTime.Parse("2024-02-20 00:55:14"),
                        UpdatedAt = DateTime.Parse("2024-02-20 00:55:14"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13082,
                        Type = "Disconnected",
                        Time = 1708391028559,
                        CreatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:03:48"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13084,
                        Type = "Disconnected",
                        Time = 1708391990769,
                        CreatedAt = DateTime.Parse("2024-02-20 01:19:50"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:19:50"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13086,
                        Type = "Disconnected",
                        Time = 1708391994447,
                        CreatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:19:54"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13088,
                        Type = "Disconnected",
                        Time = 1708392062925,
                        CreatedAt = DateTime.Parse("2024-02-20 01:21:02"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:21:02"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13090,
                        Type = "Disconnected",
                        Time = 1708392065862,
                        CreatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:21:05"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13092,
                        Type = "Disconnected",
                        Time = 1708392079080,
                        CreatedAt = DateTime.Parse("2024-02-20 01:21:19"),
                        UpdatedAt = DateTime.Parse("2024-02-20 01:21:19"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13093,
                        Type = "Disconnected",
                        Time = 1708404216900,
                        CreatedAt = DateTime.Parse("2024-02-20 04:43:36"),
                        UpdatedAt = DateTime.Parse("2024-02-20 04:43:36"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13108,
                        Type = "Disconnected",
                        Time = 1708426181164,
                        CreatedAt = DateTime.Parse("2024-02-20 10:49:41"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:49:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13110,
                        Type = "Disconnected",
                        Time = 1708426184557,
                        CreatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:49:44"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13114,
                        Type = "Disconnected",
                        Time = 1708426411480,
                        CreatedAt = DateTime.Parse("2024-02-20 10:53:31"),
                        UpdatedAt = DateTime.Parse("2024-02-20 10:53:31"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13115,
                        Type = "Disconnected",
                        Time = 1708427190907,
                        CreatedAt = DateTime.Parse("2024-02-20 11:06:30"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:06:30"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13118,
                        Type = "Disconnected",
                        Time = 1708427534556,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:14"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13120,
                        Type = "Disconnected",
                        Time = 1708427540273,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:20"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:20"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13122,
                        Type = "Disconnected",
                        Time = 1708427545162,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:25"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13124,
                        Type = "Disconnected",
                        Time = 1708427549756,
                        CreatedAt = DateTime.Parse("2024-02-20 11:12:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:12:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13126,
                        Type = "Disconnected",
                        Time = 1708427858879,
                        CreatedAt = DateTime.Parse("2024-02-20 11:17:38"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:17:38"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13128,
                        Type = "Disconnected",
                        Time = 1708427862484,
                        CreatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:17:42"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13131,
                        Type = "Disconnected",
                        Time = 1708428429569,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:09"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:09"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13133,
                        Type = "Disconnected",
                        Time = 1708428469635,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:49"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:49"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13135,
                        Type = "Disconnected",
                        Time = 1708428473228,
                        CreatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:27:53"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13137,
                        Type = "Disconnected",
                        Time = 1708428625580,
                        CreatedAt = DateTime.Parse("2024-02-20 11:30:25"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:30:25"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13139,
                        Type = "Disconnected",
                        Time = 1708428629685,
                        CreatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:30:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13143,
                        Type = "Disconnected",
                        Time = 1708429397960,
                        CreatedAt = DateTime.Parse("2024-02-20 11:43:17"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:43:17"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13146,
                        Type = "Disconnected",
                        Time = 1708429462412,
                        CreatedAt = DateTime.Parse("2024-02-20 11:44:22"),
                        UpdatedAt = DateTime.Parse("2024-02-20 11:44:22"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13148,
                        Type = "Disconnected",
                        Time = 1708433167294,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:07"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:07"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13150,
                        Type = "Disconnected",
                        Time = 1708433184638,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:24"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:24"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13152,
                        Type = "Disconnected",
                        Time = 1708433186210,
                        CreatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:46:26"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13155,
                        Type = "Disconnected",
                        Time = 1708433969271,
                        CreatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:59:29"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13157,
                        Type = "Disconnected",
                        Time = 1708433982431,
                        CreatedAt = DateTime.Parse("2024-02-20 12:59:42"),
                        UpdatedAt = DateTime.Parse("2024-02-20 12:59:42"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13158,
                        Type = "Disconnected",
                        Time = 1708434332372,
                        CreatedAt = DateTime.Parse("2024-02-20 13:05:32"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:05:32"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13160,
                        Type = "Disconnected",
                        Time = 1708434499058,
                        CreatedAt = DateTime.Parse("2024-02-20 13:08:19"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:08:19"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13162,
                        Type = "Disconnected",
                        Time = 1708434703203,
                        CreatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:11:43"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13165,
                        Type = "Disconnected",
                        Time = 1708434803073,
                        CreatedAt = DateTime.Parse("2024-02-20 13:13:23"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:13:23"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13167,
                        Type = "Disconnected",
                        Time = 1708434807288,
                        CreatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:13:27"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13170,
                        Type = "Disconnected",
                        Time = 1708435011694,
                        CreatedAt = DateTime.Parse("2024-02-20 13:16:51"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:16:51"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13172,
                        Type = "Disconnected",
                        Time = 1708435016844,
                        CreatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:16:56"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13175,
                        Type = "Disconnected",
                        Time = 1708435253079,
                        CreatedAt = DateTime.Parse("2024-02-20 13:20:53"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:20:53"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    },
                    new ActivityLogDto
                    {
                        Id = 13177,
                        Type = "Disconnected",
                        Time = 1708435258141,
                        CreatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UpdatedAt = DateTime.Parse("2024-02-20 13:20:58"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b"
                    }
                ]
            },
            new DevicesDto
            {
                Id = "ad7ce749-bec8-3e9f-b40b-df4066f62da1",
                DeviceId = "ad7ce749-bec8-3e9f-b40b-df4066f62da1",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "ImBrave",
                CustomName = null,
                Version = "0.0.1",
                Ip = "169.150.196.12",
                CreatedAt = DateTime.Parse("2024-01-10 13:17:37"),
                UpdatedAt = DateTime.Parse("2024-01-10 13:17:37"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "b1e90166-b05e-3f8f-bcaa-474fd334c04c",
                DeviceId = "b1e90166-b05e-3f8f-bcaa-474fd334c04c",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "b1e90166-b05e-3f8f-bcaa-474fd334c04c",
                CustomName = null,
                Version = "0.1.0",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-19 06:11:03"),
                UpdatedAt = DateTime.Parse("2023-12-19 06:11:03"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "c19e3c7a-aa28-353f-b382-5214c7d33f12",
                DeviceId = "c19e3c7a-aa28-353f-b382-5214c7d33f12",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "287d91f2-4121-3292-b99d-1ab5d7d07463",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2023-12-20 03:54:11"),
                UpdatedAt = DateTime.Parse("2023-12-20 03:54:11"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "c4013475-4d5e-39d9-bc47-eb2f67885c88",
                DeviceId = "c4013475-4d5e-39d9-bc47-eb2f67885c88",
                Browser = "Brave",
                Os = "Android 11",
                Device = "",
                Type = "tv",
                Name = "c4013475-4d5e-39d9-bc47-eb2f67885c88",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-03 20:44:11"),
                UpdatedAt = DateTime.Parse("2024-01-03 20:44:11"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "c6c06d17-18ba-32ae-b737-ebd389601884",
                DeviceId = "c6c06d17-18ba-32ae-b737-ebd389601884",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "c6c06d17-18ba-32ae-b737-ebd389601884",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-01-04 20:55:45"),
                UpdatedAt = DateTime.Parse("2024-01-04 20:55:45"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "c71d87ac-17c3-3b2a-bafa-f3287bb80702",
                DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "9a3a1698-af1a-3549-b670-2215d1b69895",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                UpdatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                ActivityLogs =
                [
                    new ActivityLogDto
                    {
                        Id = 13016,
                        Type = "Connected",
                        Time = 1708172261374,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    },
                    new ActivityLogDto
                    {
                        Id = 13018,
                        Type = "Connected",
                        Time = 1708172261433,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    },
                    new ActivityLogDto
                    {
                        Id = 13020,
                        Type = "Connected",
                        Time = 1708172270488,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:50"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:50"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    },
                    new ActivityLogDto
                    {
                        Id = 13017,
                        Type = "Disconnected",
                        Time = 1708172261412,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:41"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    },
                    new ActivityLogDto
                    {
                        Id = 13019,
                        Type = "Disconnected",
                        Time = 1708172269920,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:49"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:49"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    },
                    new ActivityLogDto
                    {
                        Id = 13021,
                        Type = "Disconnected",
                        Time = 1708172277868,
                        CreatedAt = DateTime.Parse("2024-02-17 12:17:57"),
                        UpdatedAt = DateTime.Parse("2024-02-17 12:17:57"),
                        UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                        DeviceId = "c71d87ac-17c3-3b2a-bafa-f3287bb80702"
                    }
                ]
            },
            new DevicesDto
            {
                Id = "ca8170bd-6632-3e49-b203-48670ad9659b",
                DeviceId = "ca8170bd-6632-3e49-b203-48670ad9659b",
                Browser = "Brave",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-02-06 15:50:02"),
                UpdatedAt = DateTime.Parse("2024-02-06 15:50:02"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "cae26aa7-0a82-3c3e-b9aa-ccba94d0ee39",
                DeviceId = "cae26aa7-0a82-3c3e-b9aa-ccba94d0ee39",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "cae26aa7-0a82-3c3e-b9aa-ccba94d0ee39",
                CustomName = null,
                Version = "0.1.0",
                Ip = "212.45.36.41",
                CreatedAt = DateTime.Parse("2023-12-20 02:52:23"),
                UpdatedAt = DateTime.Parse("2023-12-20 02:52:23"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "cb175a38-7fef-3f71-b651-52cd93a4958b",
                DeviceId = "cb175a38-7fef-3f71-b651-52cd93a4958b",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "07c915cd-84bc-3827-b24c-8fa36d01f8bb",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-02-04 14:28:59"),
                UpdatedAt = DateTime.Parse("2024-02-04 14:28:59"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "cb64795b-c3f4-3921-b8f8-eb28c3296451",
                DeviceId = "cb64795b-c3f4-3921-b8f8-eb28c3296451",
                Browser = "Brave",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "7d31e1b9-63ea-321a-b9e1-c5a9e67ac47c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-20 18:18:29"),
                UpdatedAt = DateTime.Parse("2024-01-20 18:18:29"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "d7be9db0-6f29-3ab1-bbb8-1d03d5ead1bd",
                DeviceId = "d7be9db0-6f29-3ab1-bbb8-1d03d5ead1bd",
                Browser = "Chrome",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "d7be9db0-6f29-3ab1-bbb8-1d03d5ead1bd",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-02-04 16:25:58"),
                UpdatedAt = DateTime.Parse("2024-02-04 16:25:58"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "d9237cf9-5674-317b-b538-ffa3e0867484",
                DeviceId = "d9237cf9-5674-317b-b538-ffa3e0867484",
                Browser = "Chrome",
                Os = "Android 6.0",
                Device = "LG Nexus 5",
                Type = "mobile",
                Name = "07fa2fd0-c6f1-3caf-b074-b0a1438c591c",
                CustomName = null,
                Version = "0.0.1",
                Ip = "113.32.181.109",
                CreatedAt = DateTime.Parse("2024-02-02 10:56:22"),
                UpdatedAt = DateTime.Parse("2024-02-02 10:56:22"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "d94c35d2-d8a6-3c9b-b3e3-f4a2fb52bd0d",
                DeviceId = "d94c35d2-d8a6-3c9b-b3e3-f4a2fb52bd0d",
                Browser = "Edge",
                Os = "Android 10",
                Device = "",
                Type = "tv",
                Name = "d94c35d2-d8a6-3c9b-b3e3-f4a2fb52bd0d",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.11",
                CreatedAt = DateTime.Parse("2024-01-01 20:03:53"),
                UpdatedAt = DateTime.Parse("2024-01-01 20:03:53"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "dec33301-ca2d-3068-b950-0318abb17e24",
                DeviceId = "dec33301-ca2d-3068-b950-0318abb17e24",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "dec33301-ca2d-3068-b950-0318abb17e24",
                CustomName = null,
                Version = "0.0.1",
                Ip = "77.99.231.239",
                CreatedAt = DateTime.Parse("2024-01-25 14:32:35"),
                UpdatedAt = DateTime.Parse("2024-01-25 14:32:35"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "df9c4c1a-181e-3eab-b367-31c6cec3f9f5",
                DeviceId = "df9c4c1a-181e-3eab-b367-31c6cec3f9f5",
                Browser = "Chrome WebView",
                Os = "Android 12",
                Device = "Nokia Streaming",
                Type = "tv",
                Name = "2e01031e-544a-3df0-b671-665f5df1c048",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2024-01-23 20:25:54"),
                UpdatedAt = DateTime.Parse("2024-01-23 20:25:54"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "e26bdd4f-4f2c-3a8a-b592-7946ce6d6366",
                DeviceId = "e26bdd4f-4f2c-3a8a-b592-7946ce6d6366",
                Browser = "Brave",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "e26bdd4f-4f2c-3a8a-b592-7946ce6d6366",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.75",
                CreatedAt = DateTime.Parse("2024-01-02 13:45:59"),
                UpdatedAt = DateTime.Parse("2024-01-02 13:45:59"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "e341fb94-74cb-3df2-bf56-58f0ce195150",
                DeviceId = "e341fb94-74cb-3df2-bf56-58f0ce195150",
                Browser = "Safari",
                Os = "Mac OS 10.15.7",
                Device = "Apple iPad",
                Type = "tablet",
                Name = "e341fb94-74cb-3df2-bf56-58f0ce195150",
                CustomName = null,
                Version = "0.0.1",
                Ip = "86.92.205.45",
                CreatedAt = DateTime.Parse("2024-01-02 13:55:32"),
                UpdatedAt = DateTime.Parse("2024-01-02 13:55:32"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "edbd1d1c-b39d-35ab-b98e-06a5b3ce1f02",
                DeviceId = "edbd1d1c-b39d-35ab-b98e-06a5b3ce1f02",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "edbd1d1c-b39d-35ab-b98e-06a5b3ce1f02",
                CustomName = null,
                Version = "0.0.1",
                Ip = "113.32.181.109",
                CreatedAt = DateTime.Parse("2024-01-16 11:34:26"),
                UpdatedAt = DateTime.Parse("2024-01-16 11:34:26"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "f64230fe-930b-33de-b9ab-cb796b71a74f",
                DeviceId = "f64230fe-930b-33de-b9ab-cb796b71a74f",
                Browser = "Chrome",
                Os = "Linux x86_64",
                Device = "",
                Type = "desktop",
                Name = "f64230fe-930b-33de-b9ab-cb796b71a74f",
                CustomName = null,
                Version = "0.0.1",
                Ip = "97.115.118.228",
                CreatedAt = DateTime.Parse("2024-01-26 22:46:24"),
                UpdatedAt = DateTime.Parse("2024-01-26 22:46:24"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "f855b504-4a52-30a5-b171-9babaaf86f8a",
                DeviceId = "f855b504-4a52-30a5-b171-9babaaf86f8a",
                Browser = "Chrome",
                Os = "Mac OS 10.15.7",
                Device = "Apple Macintosh",
                Type = "desktop",
                Name = "f855b504-4a52-30a5-b171-9babaaf86f8a",
                CustomName = null,
                Version = "0.0.1",
                Ip = "72.216.174.154",
                CreatedAt = DateTime.Parse("2023-12-22 08:33:45"),
                UpdatedAt = DateTime.Parse("2023-12-22 08:33:45"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "fb5a3afa-684c-3a8b-bcbf-f687fee34a12",
                DeviceId = "fb5a3afa-684c-3a8b-bcbf-f687fee34a12",
                Browser = "Electron",
                Os = "Windows 10",
                Device = "",
                Type = "desktop",
                Name = "fb5a3afa-684c-3a8b-bcbf-f687fee34a12",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-18 03:58:00"),
                UpdatedAt = DateTime.Parse("2024-01-18 03:58:00"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "fe16ba7a-8b26-3f1d-bb96-cd1868907d72",
                DeviceId = "fe16ba7a-8b26-3f1d-bb96-cd1868907d72",
                Browser = "",
                Os = " ",
                Device = "",
                Type = "desktop",
                Name = "fe16ba7a-8b26-3f1d-bb96-cd1868907d72",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.201",
                CreatedAt = DateTime.Parse("2024-01-06 13:10:28"),
                UpdatedAt = DateTime.Parse("2024-01-06 13:10:28"),
                ActivityLogs = []
            },
            new DevicesDto
            {
                Id = "feffe424-eef7-3f0d-bc00-26b8fa25dfd0",
                DeviceId = "feffe424-eef7-3f0d-bc00-26b8fa25dfd0",
                Browser = "Android",
                Os = "Android ",
                Device = "",
                Type = "mobile",
                Name = "feffe424-eef7-3f0d-bc00-26b8fa25dfd0",
                CustomName = null,
                Version = "0.0.1",
                Ip = "192.168.2.80",
                CreatedAt = DateTime.Parse("2023-12-31 12:54:17"),
                UpdatedAt = DateTime.Parse("2023-12-31 12:54:17"),
                ActivityLogs = []
            }
        ];
    }

    [HttpPost]
    public IActionResult Create()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Destroy()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
}

public class DevicesDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("custom_name")] public object? CustomName { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("activity_logs")] public ActivityLogDto[] ActivityLogs { get; set; }
}

public class ActivityLogDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public long Time { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("user_id")] public string UserId { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
}