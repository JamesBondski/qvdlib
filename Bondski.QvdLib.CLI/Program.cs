using Bondski.QvdLib;
using CommandLine;

Parser.Default.ParseArguments<FieldsOptions, DumpOptions>(args)
    .WithParsed<FieldsOptions>(FieldsOptions.Handle)
    .WithParsed<DumpOptions>(DumpOptions.Handle);
