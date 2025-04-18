using Pulumock.Pulumissues.DatasetGenerator.Options;

namespace Pulumock.Pulumissues.DatasetGenerator.Services;

internal interface IGitHubMiner
{
    public Task<GitHubMinerResult> GetGitHubIssuesAsync();
}

internal interface IGitHubMiner<T>: IGitHubMiner where T : RepositoryOptionsBase;
