using Microsoft.Extensions.Options;
using Pulumock.Pulumissues.DatasetGenerator.Clients;
using Pulumock.Pulumissues.DatasetGenerator.Clients.Responses;
using Pulumock.Pulumissues.DatasetGenerator.Options;

namespace Pulumock.Pulumissues.DatasetGenerator.Services;

internal sealed class GitHubMiner<T>(IGitHubClient gitHubClient, IOptions<T> options) : IGitHubMiner<T> where T : RepositoryOptionsBase
{
    public async Task<GitHubMinerResult> GetGitHubIssuesAsync()
    {
        var finalIssues = new List<GetGitHubIssuesAsyncResponse>();
        int page = 1;
        int pageLimit = 500;
        const int pageSize = 100;

        while (page < pageLimit)
        {
            IReadOnlyCollection<GetGitHubIssuesAsyncResponse> currentPage = await gitHubClient.GetGitHubIssuesAsync(options, page, pageSize);
            
            IEnumerable<GetGitHubIssuesAsyncResponse> filteredIssues = currentPage
                .Where(i => i.PullRequest is null);
            
            if (options.Value.Terms is not null && options.Value.Terms.Count > 0)
            {
                filteredIssues = filteredIssues.Where(i => 
                    ContainsTerm(i.Title, options.Value.Terms) || ContainsTerm(i.Body, options.Value.Terms));
            }
            
            finalIssues.AddRange(filteredIssues);

            if (currentPage.Count < pageSize)
            {
                break;
            }

            page++;
        }

        return new GitHubMinerResult
        {
            Repository = options.Value.Repo,
            Issues = finalIssues.AsReadOnly()
        };
    }

    private static bool ContainsTerm(string text, List<string> terms) => 
        terms.Any(t => text.Contains(t, StringComparison.InvariantCultureIgnoreCase));
}
