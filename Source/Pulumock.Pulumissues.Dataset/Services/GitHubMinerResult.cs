using Pulumock.Pulumissues.Dataset.Clients.Responses;

namespace Pulumock.Pulumissues.Dataset.Services;

internal sealed record GitHubMinerResult
{
    public required string Repository { get; init; }
    public required IReadOnlyCollection<GetGitHubIssuesAsyncResponse> Issues { get; init; }
}
