using System;
using System.Net;

public sealed class Report
{
    public string? Title { get; init; }
    public string? Text { get; init; }
    public DateTime Date { get; init; }
}

public interface IReportExporter
{
    string Export(Report report);
}

public sealed class HtmlReportExporter : IReportExporter
{
    public string Export(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);

        string title = WebUtility.HtmlEncode(report.Title ?? string.Empty);
        string text = WebUtility.HtmlEncode(report.Text ?? string.Empty);
        string date = WebUtility.HtmlEncode(
            report.Date.ToString("yyyy-MM-dd HH:mm:ss"));

        return "<article>\n" +
               $"    <h1>{title}</h1>\n" +
               $"    <p>{text}</p>\n" +
               $"    <footer>{date}</footer>\n" +
               "</article>";
    }
}

public static class ReportService
{
    public static string Export(Report report, IReportExporter exporter)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentNullException.ThrowIfNull(exporter);

        return exporter.Export(report);
    }
}

internal class Program
{
    private static void Main()
    {
        var report = new Report
        {
            Title = "Итоги",
            Text = "Текст отчёта: <важно>",
            Date = DateTime.Now
        };

        string html = ReportService.Export(report, new HtmlReportExporter());
        Console.WriteLine(html);
    }
}