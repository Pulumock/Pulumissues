using Pulumock.Pulumissues.DatasetGenerator.Clients.Responses;

namespace Pulumock.Pulumissues.DatasetGenerator.Services;

internal sealed record GitHubMinerResult
{
    public required string Repository { get; init; }
    public required IReadOnlyCollection<GetGitHubIssuesAsyncResponse> Issues { get; init; }
}
