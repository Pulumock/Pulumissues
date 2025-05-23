﻿using System.Net.Http.Headers;
using Pulumock.Pulumissues.DatasetGenerator.Clients;
using Pulumock.Pulumissues.DatasetGenerator.Options;
using Pulumock.Pulumissues.DatasetGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.Configure<GitHubOptions>(options => configuration.GetSection(GitHubOptions.Key).Bind(options));
services.Configure<PulumiRepositoryOptions>(options => configuration.GetSection(PulumiRepositoryOptions.Key).Bind(options));
services.Configure<PulumiDotnetRepositoryOptions>(options => configuration.GetSection(PulumiDotnetRepositoryOptions.Key).Bind(options));
services.Configure<ProtiRepositoryOptions>(options => configuration.GetSection(ProtiRepositoryOptions.Key).Bind(options));
services.Configure<DatasetOptions>(options => configuration.GetSection(DatasetOptions.Key).Bind(options));

services.AddHttpClient<IGitHubClient, GitHubClient>((provider, client) =>
{
    GitHubOptions gitHubOptions = provider.GetRequiredService<IOptions<GitHubOptions>>().Value;

    client.BaseAddress = gitHubOptions.BaseUri;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitHubOptions.Token);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(gitHubOptions.MediaType));
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Pulumock-Pulumock.Pulumissues.DatasetGenerator", "1.0.0"));
});

services.AddTransient<IGitHubMiner, GitHubMiner<PulumiRepositoryOptions>>();
services.AddTransient<IGitHubMiner, GitHubMiner<PulumiDotnetRepositoryOptions>>();
services.AddTransient<IGitHubMiner, GitHubMiner<ProtiRepositoryOptions>>();
services.AddTransient<IDatasetGenerator, DatasetGenerator>();

ServiceProvider provider = services.BuildServiceProvider();

IDatasetGenerator datasetGenerator = provider.GetRequiredService<IDatasetGenerator>();

await datasetGenerator.GenerateDatasetAsync();
