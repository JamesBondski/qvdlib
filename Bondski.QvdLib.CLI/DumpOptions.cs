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

    [Option("valueid", HelpText = "Gibt zusätzlich den Index des Wertes aus.")]
    public bool ValueId { get; set; }

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

        // Header-Zeile
        if (opts.Header)
        {
            var headerFields = reader.Header.Fields.Select(f => f.Name);
            if (opts.ValueId)
                headerFields = headerFields.Select(f => $"{f}/Index");
            if (!opts.HideRowNo)
                headerFields = new[] { "RowNo" }.Concat(headerFields);
            Console.WriteLine(string.Join(opts.Delimiter, headerFields));
        }

        int currentRow = 0;
        int outputRow = from;
        while (currentRow <= to && reader.NextRow())
        {
            if (currentRow >= from)
            {
                var values = reader.GetValues();
                var output = new List<string>();

                if (!opts.HideRowNo)
                    output.Add(outputRow.ToString());

                for (int i = 0; i < values.Length; i++)
                {
                    if (opts.ValueId)
                        output.Add($"{values[i].ToString() ?? ""}/{reader.GetValueIndex(i)}");
                    else
                        output.Add(values[i].ToString() ?? "");
                }

                Console.WriteLine(string.Join(opts.Delimiter, output));
                outputRow++;
            }
            currentRow++;
        }
    }
}