using Microsoft.Extensions.Options;
using Pulumock.Pulumissues.DatasetGenerator.Clients.Responses;
using Pulumock.Pulumissues.DatasetGenerator.Options;

namespace Pulumock.Pulumissues.DatasetGenerator.Clients;

internal interface IGitHubClient
{
    public Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync<T>(IOptions<T> options, int page, int pageSize) where T : RepositoryOptionsBase;
}
