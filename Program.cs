using CliWithSpectreConsole.Commands;
using CliWithSpectreConsole.Data;
using CliWithSpectreConsole.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient();
services.AddDbContext<RobotContext>(opt =>
    opt.UseSqlite("Data Source=robots.db"));

var app = new CommandApp(new TypeRegistrar(services));

app.Configure(c =>
{
    c.AddCommand<ExportBots>("scrape");
    c.AddCommand<ListBots>("list");
    c.AddCommand<DownloadBot>("download")
        .WithExample(new[] {"download", "dotnet-bot-1.png"})
        .WithExample(new[] {"download", "--random"});
});

app.Run(args);

