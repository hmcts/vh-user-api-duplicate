﻿using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;
using Testing.Common;
using Testing.Common.Helpers;
using UserApi.AcceptanceTests.Contexts;
using UserApi.AcceptanceTests.Helpers;
using UserApi.Contract.Responses;
using UserApi.Security;

namespace UserApi.AcceptanceTests.Steps
{
    [Binding]
    public class BaseSteps
    {
        [BeforeTestRun]
        public static void OneTimeSetup(TestContext context)
        {
            var azureAdConfiguration = TestConfig.Instance.AzureAd;
            context.TestSettings = TestConfig.Instance.TestSettings;

            context.ClientApiToken = new TokenProvider().GetClientAccessToken("", context.TestSettings.TestClientId, context.TestSettings.TestClientSecret,
                new string[] { "https://devhearingsreform.onmicrosoft.com/user-api-dev/.default" });

            context.GraphApiToken = new TokenProvider().GetClientAccessToken("", azureAdConfiguration.ClientId, azureAdConfiguration.ClientSecret,
                new string[] { "https://graph.microsoft.com/.default" });

            var apiTestsOptions = TestConfig.Instance.GetFromSection<AcceptanceTestConfiguration>("AcceptanceTestSettings");
            context.BaseUrl = apiTestsOptions.UserApiBaseUrl;
        }

        [BeforeTestRun]
        public static void CheckHealth(TestContext context)
        {
            var endpoint = new ApiUriFactory().HealthCheckEndpoints;
            context.Request = context.Get(endpoint.CheckServiceHealth());
            context.Response = context.Client().Execute(context.Request);
            context.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [BeforeTestRun]
        public static void CheckUserExistsWithCorrectGroups(TestContext context)
        {
            var endpoint = new ApiUriFactory().AccountEndpoints;
            context.Request = context.Get(endpoint.GetGroupsForUser(context.TestSettings.ExistingUserId));
            context.Response = context.Client().Execute(context.Request);
            context.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            var model = ApiRequestHelper.DeserialiseSnakeCaseJsonToResponse<List<GroupsResponse>>(context.Response.Content);
            var actualGroups = new List<Group>();

            foreach (var group in model)
            {
                actualGroups.Add(new Group()
                {
                    GroupId = group.GroupId,
                    DisplayName = group.DisplayName
                });
            }

            actualGroups.Should().BeEquivalentTo(context.TestSettings.ExistingGroups, opts => opts.WithoutStrictOrdering());
        }
    }
}
