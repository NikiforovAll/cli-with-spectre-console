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

await app.RunAsync(args);


var app2 = new CommandApp();

app2.Configure(c =>
{
    c.AddBranch("bots", bots =>
    {
        bots.AddCommand<ListBots>("list");
        bots.AddBranch("download", create =>
        {
            create.AddCommand<ExportBots>("table")
                .WithExample(new[] {"bots", "download", "table"});
            create.AddCommand<DownloadBot>("bot")
                .WithExample(new[] {"bots", "download", "bot"});
        });
    });
});
