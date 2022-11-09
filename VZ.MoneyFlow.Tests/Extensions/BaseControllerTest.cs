using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VZ.MoneyFlow.Tests.Extensions
{
    public class BaseControllerTest : BaseTest
    {
        protected static readonly string UserId = Guid.NewGuid().ToString();
        protected static readonly Mock<HttpContext> _httpContext;

        static BaseControllerTest()
        {
            _httpContext = SetupHttpContext(UserId);
        }

        private static Mock<HttpContext> SetupHttpContext(string userId)
        {
            Mock<HttpContext> _httpContext = new();
            _httpContext.Setup(u => u.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            })));
            return _httpContext;
        }
    }
}
