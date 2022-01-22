# .NET Bots Gallery with Spectre.Console

`Spectre.Console` provides application model to bind `args[]` to git-style commands.

```csharp
var app = new CommandApp();

app.Configure(c =>
{
    c.AddCommand<ExportBots>("scrape");
    c.AddCommand<ListBots>("list");
    c.AddCommand<DownloadBot>("download")
        .WithExample(new[] {"download", "--random"});
});

await app.RunAsync(args);
```

## Demo

`dotnet run -- -h`

![help](/assets/help.png)

`dotnet run -- scrape`

![list](/assets/scrape.png)

`dotnet run -- list`

➕🎉 <https://www.nuget.org/packages/Spectre.Console.Extensions.Table>

![list](/assets/bot-list.png)

`dotnet run -- download`

➕🎉 <https://www.nuget.org/packages/Spectre.Console.Extensions.Progress>

![download](/assets/download-bot.png)

## Reference

* <https://spectreconsole.net/cli/composing>
* <https://github.com/nikiforovall/Spectre.Console.Extensions>
* Known issues <https://github.com/spectreconsole/spectre.console/issues/698>
