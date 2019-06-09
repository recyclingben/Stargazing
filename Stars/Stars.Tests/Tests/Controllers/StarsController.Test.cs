using NUnit.Framework;
using System;

using Stars.Controllers;
using Stars.StarManagement;
using System.Threading.Tasks;
using System.Web.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Stars.Tests.Stubs.StarManagement;

namespace Stars.Tests.Tests.Controllers
{
    [TestFixture]
    public class Test_StarsController
    {
        public DummyStarsTracker StarsTracker { get; set; }
        public StarsController Controller { get; set; }

        [SetUp]
        public async Task SetUp()
        {
            StarsTracker = new DummyStarsTracker();
            await StarsTracker.SetToRandom();

            Controller = new StarsController(StarsTracker);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }



        [TestCase]
        public async Task GETCount_ReturnsJObject_InCorrectFormat()
        {
            var result = (await Controller.GET_Count()).Value;
            Assert.IsNotNull(result?["data"]?["count"]);
        }

        [TestCase]
        public async Task GETCount_ReturnsJObject_WithCorrectValue()
        {
            var result = (await Controller.GET_Count()).Value;
            int? count = (int?)result?["data"]?["count"];
            Assert.AreEqual(count, await StarsTracker.GetCount());
        }


        [TestCase(1)]
        [TestCase(10)]
        [TestCase(1000)]
        public async Task POSTIncrement_IncrementsValue_ByCorrectAmount(int incrementationAmount)
        {
            long before = await StarsTracker.GetCount();
            await Controller.POST_Increment(incrementationAmount);
            long after = await StarsTracker.GetCount();

            Assert.AreEqual(incrementationAmount, after - before);
        }

        [TestCase]
        public async Task POSTIncrement_IncrementsValue_ByOneWhenCalledWithNoValues()
        {
            long before = await StarsTracker.GetCount();
            await Controller.POST_Increment();
            long after = await StarsTracker.GetCount();

            Assert.AreEqual(1, after - before);
        }

        [TestCase]
        public async Task POSTIncrement_ReturnsError422_WhenCalledWithValueBelowZero()
        {
            var response = await Controller.POST_Increment(-1);

            Assert.AreEqual(422, Controller.Response.StatusCode);
            Assert.IsNotNull(response.Value?["error"]);
        }
    }
}