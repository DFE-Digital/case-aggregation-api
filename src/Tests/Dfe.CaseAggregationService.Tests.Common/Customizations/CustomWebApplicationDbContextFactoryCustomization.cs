using System.Collections.Specialized;
using AutoFixture;
using DfE.CoreLibs.Testing.Mocks.Authentication;
using DfE.CoreLibs.Testing.Mocks.WebApplicationFactory;
using Dfe.CaseAggregationService.Api;
using Dfe.CaseAggregationService.Api.Client.Extensions;
using Dfe.CaseAggregationService.Client;
using Dfe.CaseAggregationService.Client.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Security.Claims;
using Dfe.AcademiesApi.Client;
using Dfe.AcademiesApi.Client.Contracts;
using Dfe.AcademiesApi.Client.Security;
using Dfe.AcademiesApi.Client.Settings;
using Dfe.Complete.Client;
using Dfe.Complete.Client.Contracts;
using NSubstitute;

namespace Dfe.CaseAggregationService.Tests.Common.Customizations
{
    public class CustomWebApplicationDbContextFactoryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CustomWebApplicationDbContextFactory<Program>>(composer => composer.FromFactory(() =>
            {

                var factory = new CustomWebApplicationDbContextFactory<Program>()
                {
                    UseWireMock = true,
                    WireMockPort = 0,
                    ExternalServicesConfiguration = services =>
                    {
                        services.PostConfigure<AuthenticationOptions>(options =>
                        {
                            options.DefaultAuthenticateScheme = "TestScheme";
                            options.DefaultChallengeScheme = "TestScheme";
                        });

                        services.AddAuthentication("TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, MockJwtBearerHandler>("TestScheme", options => { });
                    },
                    ExternalHttpClientConfiguration = client =>
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "external-mock-token");
                    },
                    ExternalWireMockConfigOverride = (cfgBuilder, mockServer) =>
                    {
                        cfgBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["IntegrationTestOverride"] = "true",
                            ["AcademisationApiClient:BaseUrl"] = mockServer.Urls[0].TrimEnd('/') + "/academisation/",
                            ["AcademiesApiClient:BaseUrl"] = mockServer.Urls[0].TrimEnd('/') + "/academies/",
                            ["CompleteApiClient:BaseUrl"] = mockServer.Urls[0].TrimEnd('/') + "/complete/",
                            ["MfspApiClient:BaseUrl"] = mockServer.Urls[0].TrimEnd('/') + "/mfsp/",
                            ["RecastApiClient:BaseUrl"] = mockServer.Urls[0].TrimEnd('/') + "/recast/",
                        });

                        mockServer.LogEntriesChanged += LogEntriesChanged;
                    },
                    ExternalWireMockClientRegistration = (services, config, wireHttp) =>
                    {

                        services.AddHttpClient("MfspApiClient", (serviceProvider, httpClient) =>
                        {
                            var wConfig = serviceProvider.GetRequiredService<IConfiguration>();

                            httpClient.BaseAddress = new Uri(wConfig["MfspApiClient:BaseUrl"]!);
                        });

                        services.AddHttpClient("AcademisationApiClient", (serviceProvider, httpClient) =>
                        {
                            var wConfig = serviceProvider.GetRequiredService<IConfiguration>();

                            httpClient.BaseAddress = new Uri(wConfig["AcademisationApiClient:BaseUrl"]!);
                        });

                        services.AddHttpClient<ITrustsV4Client, TrustsV4Client>(
                                (httpClient, serviceProvider) =>
                                {
                                    var wConfig = serviceProvider.GetRequiredService<IConfiguration>();

                                    httpClient.BaseAddress = new Uri(wConfig["AcademiesApiClient:BaseUrl"]!);

                                    return ActivatorUtilities.CreateInstance<TrustsV4Client>(
                                        serviceProvider, httpClient, wConfig["AcademiesApiClient:BaseUrl"]!);
                                })
                                .AddHttpMessageHandler(serviceProvider =>
                                {
                                    var apiSettings = serviceProvider.GetRequiredService<AcademiesApiClientSettings>();
                                    return new ApiKeyHandler(apiSettings);
                                });

                        services.AddHttpClient("RecastApiClient", (serviceProvider, httpClient) =>
                        {
                            var wConfig = serviceProvider.GetRequiredService<IConfiguration>();

                            httpClient.BaseAddress = new Uri(wConfig["RecastApiClient:BaseUrl"]!);
                        });

                        //services.AddHttpClient<IProjectsClient, ProjectsClient>((httpClient, serviceProvider) =>
                        //{
                        //    var wConfig = serviceProvider.GetRequiredService<IConfiguration>();

                        //    httpClient.BaseAddress = new Uri(wConfig["CompleteApiClient:BaseUrl"]!);

                        //    return ActivatorUtilities.CreateInstance<ProjectsClient>(
                        //        serviceProvider, httpClient, wConfig["CompleteApiClient:BaseUrl"]!);
                        //});
                    }

                };

                var client = factory.CreateClient();

                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "CaseAggregationApiClient:BaseUrl", client.BaseAddress!.ToString() }
                    })
                    .Build();

                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(config);
                services.AddCaseAggregationApiClient<ICasesClient, CasesClient>(config, client);
                var serviceProvider = services.BuildServiceProvider();

                fixture.Inject(factory);
                fixture.Inject(serviceProvider);
                fixture.Inject(client);
                fixture.Inject(serviceProvider.GetRequiredService<ICasesClient>());
                fixture.Inject(new List<Claim>());

                return factory;
            }));

        }

        static void LogEntriesChanged(object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine(e.NewItems);
        }
    }
}
