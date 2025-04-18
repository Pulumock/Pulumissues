using Microsoft.Extensions.Options;
using Pulumock.Pulumissues.Dataset.Clients;
using Pulumock.Pulumissues.Dataset.Clients.Responses;
using Pulumock.Pulumissues.Dataset.Options;

namespace Pulumock.Pulumissues.Dataset.Services;

internal sealed class GitHubMiner<T>(IGitHubClient gitHubClient, IOptions<T> options) : IGitHubMiner<T> where T : RepositoryOptionsBase
{
    public async Task<GitHubMinerResult> GetGitHubIssuesAsync()
    {
        var allIssues = new List<GetGitHubIssuesAsyncResponse>();
        int page = 1;
        int pageLimit = 500;
        const int pageSize = 100;

        while (page < pageLimit)
        {
            IReadOnlyCollection<GetGitHubIssuesAsyncResponse> currentPage = await gitHubClient.GetGitHubIssuesAsync(options, page, pageSize);
            
            var onlyIssues = currentPage
                .Where(i => i.PullRequest is null)
                .ToList();

            allIssues.AddRange(onlyIssues);

            if (currentPage.Count < pageSize)
            {
                break;
            }

            page++;
        }

        return new GitHubMinerResult
        {
            Repository = options.Value.Repo,
            Issues = allIssues.AsReadOnly()
        };
    }
}
