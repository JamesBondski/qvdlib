using CommandLine;
using Bondski.QvdLib;

[Verb("fields", HelpText = "Gibt die Feldnamen einer QVD-Datei aus.")]
public class FieldsOptions
{
    [Value(0, MetaName = "file", Required = true, HelpText = "Pfad zur QVD-Datei.")]
    public string File { get; set; }

    [Option('v', "verbose", Required = false, HelpText = "Zeigt zusätzliche Feldinformationen an.")]
    public bool Verbose { get; set; }

    public static void Handle(FieldsOptions opts)
    {
        var reader = new QvdReader(opts.File);
        for (int i = 0; i < reader.Header.Fields.Length; i++)
        {
            var field = reader.Header.Fields[i];
            if (opts.Verbose)
            {
                Console.WriteLine($"{i}: {field.Name} (BitOffset={field.BitOffset}, BitWidth={field.BitWidth}, Bias={field.Bias}, NoOfSymbols={field.NoOfSymbols})");
            }
            else
            {
                Console.WriteLine($"{i}: {field.Name}");
            }
        }
    }
}