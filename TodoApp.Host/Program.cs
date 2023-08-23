using Mumrich.SpaDevMiddleware.Domain.Types;
using Mumrich.SpaDevMiddleware.Extensions;
using TodoApp.Host;

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();

appSettings.SinglePageApps.Add("/", new()
{
    DevServerAddress = "http://localhost:3456",
    SpaRootPath = "todo-app",
    NodeStartScript = "dev",
    NodeBuildOutputPath = "build",
    NodePackageManager = NodePackageManager.Yarn,
    Regex = "dev server running at:",
    Bundler = BundlerType.ViteJs,
    SpaAssetsExpression = "^(src|node_modules|favicon.+|@[a-zA-Z]+|.*vite.*|.*\\.json|.*\\.js|.*\\.css)$"
});

builder.Services.AddControllersWithViews();
builder.Services.AddCors();
builder.RegisterSinglePageAppDevMiddleware(appSettings);

var app = builder.Build();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.MapControllers();
app.MapSinglePageApps(appSettings);

app.Run();