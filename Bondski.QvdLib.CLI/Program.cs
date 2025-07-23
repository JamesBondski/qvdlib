using Bondski.QvdLib;
using CommandLine;

Parser.Default.ParseArguments<FieldsOptions,object>(args)
    .WithParsed<FieldsOptions>(FieldsOptions.Handle);
