using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using QFRMS.Data.DTOs;
using QFRMS.Data.Models;
using QFRMS.Data.ViewModels;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Utils;
using QFRMS.WebApp.Controllers;

namespace QFRMS.Test.Controllers
{
    public class AccountControllerTesting
    {
        private readonly Mock<UserManager<UserAccount>> _userManager;
        private readonly Mock<SignInManager<UserAccount>> _signInManager;
        private readonly Mock<IFileLogger> _fileLogger;
        private readonly Mock<ILogger<AccountController>> _logger;
        private readonly Mock<IUserAccountService> _service;
        private readonly AccountController _controller;

        public AccountControllerTesting()
        {
            _userManager = new Mock<UserManager<UserAccount>>(
                Mock.Of<IUserStore<UserAccount>>(),
                null, null, null, null, null, null, null, null);
            _signInManager = new Mock<SignInManager<UserAccount>>(
                _userManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<UserAccount>>(),
                null,null, null, null);
            _fileLogger = new Mock<IFileLogger>();
            _logger = new Mock<ILogger<AccountController>>();
            _service = new Mock<IUserAccountService>();

            _controller = new AccountController(
                _signInManager.Object,
                _userManager.Object,
                _fileLogger.Object,
                _logger.Object,
                _service.Object
                );
        }

        [Fact]
        public async Task IndexViewFail() //Provide no PagedList
        {
            //Arrange


            //Act
            var act = await _controller.Index(1);

            //Assert
            var assert = Assert.IsType<RedirectToActionResult>(act);
            Assert.Equal("Index", assert.ActionName);
            Assert.Equal("Home", assert.ControllerName);

        }


    }
}