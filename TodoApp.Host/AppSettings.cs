using Mumrich.SpaDevMiddleware.Domain.Contracts;
using Mumrich.SpaDevMiddleware.Domain.Models;

namespace TodoApp.Host;

public class AppSettings : ISpaDevServerSettings
{
    public Dictionary<string, SpaSettings> SinglePageApps { get; set; } = new();
    public string SpaRootPath { get; set; } = Directory.GetCurrentDirectory();
}