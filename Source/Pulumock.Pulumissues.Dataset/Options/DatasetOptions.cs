namespace Pulumock.Pulumissues.Dataset.Options;

internal sealed class DatasetOptions
{
    public const string Key = "Pulumock.Pulumissues.Dataset";
    
    public required string FilePath { get; init; }
}
