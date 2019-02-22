﻿using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UserApi.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Graph;
using UserApi.Controllers;
using UserApi.Contract.Responses;
using List = Microsoft.Graph.List;
using System.Collections.Generic;
using System.Linq;

namespace UserApi.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private Mock<IUserAccountService> _userAccountService;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _userAccountService = new Mock<IUserAccountService>();
            _controller = new AccountController(_userAccountService.Object, new TelemetryClient());
        }

        [Test]
        public async Task Should_get_group_by_name_from_api()
        {
            const string groupName = "VirtualRoomAdministrator";
            var response =  new Group();
            var groupResponse = new GroupsResponse();

            _userAccountService.Setup(x => x.GetGroupByName(groupName)).Returns(Task.FromResult(response));

            var actionResult = (OkObjectResult)(await _controller.GetGroupByName(groupName));
            var actualResponse = (GroupsResponse)actionResult.Value;
            actualResponse.DisplayName.Should().BeSameAs(groupResponse.DisplayName);
            actualResponse.GroupId.Should().BeSameAs(groupResponse.GroupId);
        }
        
        [Test]
        public async Task Should_get_group_by_id_from_api()
        {
            const string groupId = "123";
            var response = new Group()
            {
                DisplayName = "External",
            };
            var groupResponse = new GroupsResponse()
            {
                DisplayName = "External",
                GroupId = "123"
            };

            _userAccountService.Setup(x => x.GetGroupById(groupId)).Returns(Task.FromResult(response));

            var actionResult = (OkObjectResult)(await _controller.GetGroupById(groupId));
            var actualResponse = (GroupsResponse)actionResult.Value;
            actualResponse.DisplayName.Should().BeSameAs(groupResponse.DisplayName);
        }

        [Test]
        public async Task Should_get_groups_for_user_by_user_id_from_api()
        {
            const string userId = "123";
            var group = new Group()
            {
                DisplayName = "External",
            };
            var response = new List<Group>
            {
                group
            };

            IEnumerable<GroupsResponse> groupResponseList = new[]
            {
                new GroupsResponse() { DisplayName = "External" }
            };

            _userAccountService.Setup(x => x.GetGroupsForUser(userId)).Returns(Task.FromResult(response));

            var actionResult = (OkObjectResult)(await _controller.GetGroupsForUser(userId));
            var actualResponse = (IEnumerable<GroupsResponse>)actionResult.Value;
            actualResponse.FirstOrDefault().DisplayName.Should().BeSameAs(groupResponseList.FirstOrDefault().DisplayName);
        }
    }
}