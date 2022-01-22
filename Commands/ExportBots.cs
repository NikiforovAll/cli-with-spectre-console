#pragma warning disable CS8765

namespace CliWithSpectreConsole.Commands;

using Data;
using HtmlAgilityPack;
using Spectre.Console;

public class ExportBotsSettings : CommandSettings
{
}

public class ExportBots : Command<ExportBotsSettings>
{
    private readonly RobotContext db;
    private static readonly Uri BaseUrl = new("https://mod-dotnet-bot.net");
    private static readonly Uri GalleryUrl = new(BaseUrl, "gallery");

    public ExportBots(RobotContext db) => this.db = db;

    public override int Execute(
        CommandContext context, ExportBotsSettings settings)
    {
        var web = new HtmlWeb();
        var doc = web.Load(GalleryUrl);
        var robots = FindRobots(doc).ToList();

        this.db.Database.EnsureCreated();
        this.db.AddRange(robots);

        try
        {
            this.db.SaveChanges();
        }
        catch (Exception e) when (e.InnerException is not null)
        {
            AnsiConsole.WriteException(e.InnerException);
            return -1;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Markup(
            "[default on grey] exported robots [/]");
        AnsiConsole.Markup(
            $"[white on green] {robots.Count} [/]");
        AnsiConsole.WriteLine();

        return 0;
    }

    private static IEnumerable<Robot> FindRobots(HtmlDocument document)
    {
        var figures = document
            .DocumentNode
            .SelectNodes(@"//figure/img");

        foreach (var figure in figures)
        {
            var attr = figure.Attributes
                    .FirstOrDefault(el => el.Name == "src")
                    ?.Value
                ?? throw new InvalidOperationException(nameof(FindRobots));

            var fullFigureUrl = new Uri(BaseUrl, attr);

            yield return new Robot(
                fullFigureUrl.Segments.Last(), fullFigureUrl.ToString());
        }
    }
}
