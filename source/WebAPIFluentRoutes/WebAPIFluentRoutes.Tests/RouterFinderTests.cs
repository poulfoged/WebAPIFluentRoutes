using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

// ReSharper disable UnusedParameter.Local

namespace WebAPIFluentRoutes.Tests
{
    public class RouterFinderTests
    {
        [Test]
        public void Can_retrive_parameters()
        {
            ////Arrange

            ////Act
            var acutalUrl = RouteFinder.GetInvocation<TestClass>(c => c.Test("test"));

            ////Assert
            Assert.That(acutalUrl.ParameterValues["name"], Is.EqualTo("test"));
        }

        [Test]
        public void Can_retrive_parameters2()
        {
            ////Arrange
            var request = new Request { LastName = "tester", Name = "testsen" };
            ////Act
            var acutalUrl = RouteFinder.GetInvocation<TestClass>(c => c.Test(request));

            ////Assert
            Assert.That(acutalUrl.ParameterValues["request"], Is.EqualTo(request));
        }

        [Test]
        public void Can_retrive_parameters3()
        {
            ////Arrange
            var request = new Request { LastName = "tester", Name = "testsen" };

            ////Act
            var acutalUrl = RouteFinder.GetInvocation<TestClass>(c => c.Test3(request));

            ////Assert
            Assert.That(acutalUrl.ParameterValues["request"], Is.EqualTo(request));
        }

        private class TestClass
        {
            public void Test(string name) { }
            public void Test(Request request) { }

            public async Task<int> Test3(Request request)
            {
                return await Task.FromResult(123);
            }
        }

        private class Request
        {
            public string Name { get; set; }
            public string LastName { get; set; }
        }

    }
}

// ReSharper restore UnusedParameter.Local
