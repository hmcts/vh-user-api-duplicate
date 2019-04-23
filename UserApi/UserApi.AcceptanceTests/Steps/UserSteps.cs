﻿using Faker;
using FluentAssertions;
using TechTalk.SpecFlow;
using Testing.Common.ActiveDirectory;
using Testing.Common.Helpers;
using UserApi.AcceptanceTests.Contexts;
using UserApi.Contract.Requests;
using UserApi.Contract.Responses;
using UserApi.Services.Models;

namespace UserApi.AcceptanceTests.Steps
{
    [Binding]
    public sealed class UserSteps : BaseSteps
    {
        private readonly AcTestContext _acTestContext;
        private readonly UserEndpoints _endpoints = new ApiUriFactory().UserEndpoints;

        public UserSteps(AcTestContext acTestContext)
        {
            _acTestContext = acTestContext;
        }

        [Given(@"I have a new hearings reforms user account request with a valid email")]
        public void GivenIHaveANewHearingsReformsUserAccountRequestWithAValidEmail()
        {
            var createUserRequest = new CreateUserRequest
            {
                RecoveryEmail = Internet.Email(),
                FirstName = Name.First(),
                LastName = Name.Last()
            };
            _acTestContext.Request = _acTestContext.Post(_endpoints.CreateUser, createUserRequest);
        }

        [Given(@"I have a get user by AD user Id request for an existing user")]
        public void GivenIHaveAGetUserByAdUserIdRequestForAnExistingUser()
        {
            _acTestContext.Request = _acTestContext.Get(_endpoints.GetUserByAdUserId(_acTestContext.TestSettings.ExistingUserId));
        }

        [Given(@"I have a get user by user principal name request for an existing user principal name")]
        public void GivenIHaveAGetUserByUserPrincipalNameRequestForAnExistingUserPrincipalName()
        {
            _acTestContext.Request = _acTestContext.Get(_endpoints.GetUserByAdUserName(_acTestContext.TestSettings.ExistingUserPrincipal));
        }

        [Given(@"I have a get user profile by email request for an existing email")]
        public void GivenIHaveAGetUserProfileByEmailRequestForAnExistingEmail()
        {
            _acTestContext.Request = _acTestContext.Get(_endpoints.GetUserByEmail(_acTestContext.TestSettings.ExistingEmail));
        }

        [Then(@"the user should be added")]
        public void ThenTheUserShouldBeAdded()
        {
            var model = ApiRequestHelper.DeserialiseSnakeCaseJsonToResponse<NewUserResponse>(_acTestContext.Json);
            model.Should().NotBeNull();
            model.OneTimePassword.Should().NotBeNullOrEmpty();
            model.UserId.Should().NotBeNullOrEmpty();
            model.Username.Should().NotBeNullOrEmpty();
            _acTestContext.NewUserId = model.UserId;
        }

        [Then(@"the user details should be retrieved")]
        public void ThenTheUserDetailsShouldBeRetrieved()
        {
            var model = ApiRequestHelper.DeserialiseSnakeCaseJsonToResponse<UserProfile>(_acTestContext.Json);
            model.Should().NotBeNull();
            model.DisplayName.Should().NotBeNullOrEmpty();
            model.Email.Should().NotBeNullOrEmpty();
            model.FirstName.Should().NotBeNullOrEmpty();
            model.LastName.Should().NotBeNullOrEmpty();
            model.UserId.Should().NotBeNullOrEmpty();
            model.UserName.Should().NotBeNullOrEmpty();
        }

        [AfterScenario]
        public void NewUserClearUp()
        {
            if (string.IsNullOrWhiteSpace(_acTestContext.NewUserId)) return;
            var userDeleted = ActiveDirectoryUser.DeleteTheUserFromAd(_acTestContext.NewUserId, _acTestContext.GraphApiToken);
            userDeleted.Should().BeTrue($"New user with ID {_acTestContext.NewUserId} is deleted");
            _acTestContext.NewUserId = null;
        }
    }
}