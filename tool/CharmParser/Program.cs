using System.CommandLine.DragonFruit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Auraclast;
using Auraclast.Charms.Plaintext;
using System.IO;
using System.Linq;
using System.Text;

public class Program 
{
    /// <summary>
    /// Attempts to parse charm plaintext into a csv of charm data
    /// </summary>
    /// <param name="input">The directory to search for plaintext charm files</param>
    /// <param name="output">The directory to use for output</param>
    public static void Main(string input = "../../data/charms_plaintext", string output = "../../data/charms_csv")
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<Program>();
        var parser = new PlaintextCharmParser(logger);
        var files = Directory.GetFiles(input);
        foreach(var file in files)
        {
            logger.LogInformation("Opening file {file}", file);
            var name = Path.GetFileNameWithoutExtension(file);
            using var fs = new FileStream(file, FileMode.Open);
            var vals = parser.ReadStreamAsync(name, fs).GetAwaiter().GetResult();
            logger.LogInformation("Found {num} charms for splat {splat}", vals.Count, name);
            
            var path = Path.Join(output, name + ".csv");
            using var outputFs = new FileStream(path, FileMode.Create);
            using var outputWriter = new StreamWriter(outputFs);
            outputWriter.WriteLine("Splat,Category,Name,Dots/Xp,Type,Resonance,Flavor,System");
            foreach(var val in vals)
            {
                outputWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}",
                    StringToCSVCell(val.Splat),
                    StringToCSVCell(val.Category),
                    StringToCSVCell(val.Name),
                    val.Dots,
                    StringToCSVCell(val.Type),
                    StringToCSVCell(string.Join(",", val.Resonance)),
                    StringToCSVCell(val.Flavor),
                    StringToCSVCell(val.System)
                    );
                foreach(var enh in val.Enhancements)
                {
                    outputWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}",
                        string.Empty,
                        "(enhancement)",
                        StringToCSVCell(enh.Name),
                        enh.Cost,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        StringToCSVCell(enh.System)
                        );
                }
            }
            outputWriter.Flush();
        }
    }

    // from https://stackoverflow.com/questions/6377454/escaping-tricky-string-to-csv-format
    public static string StringToCSVCell(string str)
    {
        bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
        if (mustQuote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char nextChar in str)
            {
                sb.Append(nextChar);
                if (nextChar == '"')
                    sb.Append("\"");
            }
            sb.Append("\"");
            return sb.ToString();
        }

        return str;
    }
}