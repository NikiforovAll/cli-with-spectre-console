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

if (args.Length == 0)
{
    AnsiConsole.Write(new FigletText(".NET Bots CLI").Centered().Color(Color.Purple));
}

await app.RunAsync(args);
