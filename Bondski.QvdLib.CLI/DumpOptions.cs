using CommandLine;
using Bondski.QvdLib;

[Verb("dump", HelpText = "Gibt die Daten einer QVD-Datei auf der Konsole aus.")]
public class DumpOptions
{
    [Value(0, MetaName = "file", Required = true, HelpText = "Pfad zur QVD-Datei.")]
    public string File { get; set; }

    [Option("row", HelpText = "Zeilenbereich im Format <von>:<bis> oder einzelne Zeile.")]
    public string Row { get; set; }

    [Option("delimiter", Default = "|", HelpText = "Trennzeichen für die Ausgabe.")]
    public string Delimiter { get; set; }

    [Option("header", HelpText = "Gibt die Feldnamen als Kopfzeile aus.")]
    public bool Header { get; set; }

    [Option("hiderowno", HelpText = "Blendet die Zeilennummer aus.")]
    public bool HideRowNo { get; set; }

    public static void Handle(DumpOptions opts)
    {
        using var reader = new QvdReader(opts.File);

        int from = 0;
        int to = reader.Header.NoOfRecords - 1;

        if (!string.IsNullOrEmpty(opts.Row))
        {
            var parts = opts.Row.Split(':');
            if (parts.Length == 1)
            {
                from = to = int.Parse(parts[0]);
            }
            else if (parts.Length == 2)
            {
                from = int.Parse(parts[0]);
                to = int.Parse(parts[1]);
            }
        }

        if (opts.Header)
        {
            var headerFields = reader.Header.Fields.Select(f => f.Name);
            if (!opts.HideRowNo)
            {
                headerFields = new[] { "RowNo" }.Concat(headerFields);
            }
            Console.WriteLine(string.Join(opts.Delimiter, headerFields));
        }

        int currentRow = 0;
        int outputRow = from;
        while (currentRow <= to && reader.NextRow())
        {
            if (currentRow >= from)
            {
                var values = reader.GetValues().Select(v => v.ToString() ?? "");
                if (!opts.HideRowNo)
                {
                    values = new[] { outputRow.ToString() }.Concat(values);
                }
                Console.WriteLine(string.Join(opts.Delimiter, values));
                outputRow++;
            }
            currentRow++;
        }
    }
}