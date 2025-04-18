using Microsoft.Extensions.Options;
using Pulumock.Pulumissues.Dataset.Clients.Responses;
using Pulumock.Pulumissues.Dataset.Options;

namespace Pulumock.Pulumissues.Dataset.Clients;

internal interface IGitHubClient
{
    public Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync<T>(IOptions<T> options, int page, int pageSize) where T : RepositoryOptionsBase;
}
