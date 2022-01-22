#pragma warning disable CS8765

namespace CliWithSpectreConsole.Commands;

using System.Data;
using System.Data.Common;
using Data;
using Microsoft.EntityFrameworkCore;
using Spectre.Console.Extensions.Table;

public class ListBotsSettings : CommandSettings
{
}

public class ListBots : Command<ListBotsSettings>
{
    private readonly RobotContext db;

    public ListBots(RobotContext db) => this.db = db;

    public override int Execute(
        CommandContext context, ListBotsSettings settings)
    {
        AnsiConsole.Write(new FigletText(".NET Bots").Centered().Color(Color.Purple));

        var dataset = new DataSet {DataSetName = "Bot Gallery",};
        var connection = this.db.Database.GetDbConnection();

        dataset.Tables.Add(RetrieveDataTable(connection, this.db.Robots));
        var dataSetToDisplay = dataset.FromDataSet(opt => opt.BorderColor(Color.Aqua));
        AnsiConsole.Write(dataSetToDisplay);
        return 0;
    }

    private static DataTable RetrieveDataTable(
        DbConnection connection, IQueryable query)
    {
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = query.ToQueryString();
        using var reader = cmd.ExecuteReader();
        var dt = new DataTable();
        dt.Load(reader);
        return dt;
    }
}
