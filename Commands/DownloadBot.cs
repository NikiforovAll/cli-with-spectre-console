using System.ComponentModel;

#pragma warning disable CS8765

namespace CliWithSpectreConsole.Commands;

using Data;
using Spectre.Console.Extensions.Progress;

public class DownloadBotSettings : CommandSettings
{
    [CommandArgument(0, "[name]")]
    [Description("Download bot by name")]

    public string? Name { get; set; }

    [CommandOption("-r|--random")]
    [Description("Specifies if random bot should be stored in the system")]
    public bool IsRandom { get; set; }
}

public class DownloadBot : Command<DownloadBotSettings>
{
    private readonly HttpClient httpClient;
    private readonly RobotContext db;

    public DownloadBot(HttpClient httpClient, RobotContext db)
    {
        this.httpClient = httpClient;
        this.db = db;
    }

    public override int Execute(
        CommandContext context, DownloadBotSettings settings)
    {
        if (settings.IsRandom)
        {
            var toSkip = Random
                .Shared.Next(0, this.db.Robots.Count());
            settings.Name = this.db
                .Robots.Skip(toSkip).Take(1).First().Name;
        }

        var robot = this.db
            .Robots.First(r => r.Name == settings.Name);

        var fileName = string.Empty;

        AnsiConsole.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                new RemainingTimeColumn(), new SpinnerColumn(),
            }).StartAsync(
                this.httpClient,
                new HttpRequestMessage(HttpMethod.Get, robot.Uri),
                "Downloading a bot", stream =>
                {
                    fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
                    using (var fileStream = File.Create(fileName))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }

                    return Task.CompletedTask;
                }).Wait();

        AnsiConsole.WriteLine(fileName);
        // TODO: use stream/span based API instead of file caching
        var image = new CanvasImage(fileName).MaxWidth(16);
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel(image));

        return 0;
    }
}
