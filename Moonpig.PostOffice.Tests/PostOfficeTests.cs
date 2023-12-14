using Moonpig.PostOffice.Infrastructure.BusinessLogic;
using Moonpig.PostOffice.Infrastructure.DataAccess;
using Moonpig.PostOffice.Model.Dto;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Moonpig.PostOffice.Tests
{
    public class PostOfficeTests
    {
        private readonly Mock<IOrderQuery> _orderQueryMock;
        private readonly DespatchLogic _logic;
        private OrderRequestDto _requestDto;
        private OrderResponseDto _responseDto;

        public PostOfficeTests()
        {
            _orderQueryMock = new Mock<IOrderQuery>();
            _logic = new DespatchLogic(_orderQueryMock.Object);
        }

        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            DespatchDateController controller = new DespatchDateController();
            var date = controller.Get(new List<int>() {1}, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(1));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            DespatchDateController controller = new DespatchDateController();
            var date = controller.Get(new List<int>() { 2 }, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(2));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            DespatchDateController controller = new DespatchDateController();
            var date = controller.Get(new List<int>() { 3 }, DateTime.Now);
            date.Date.Date.ShouldBe(DateTime.Now.Date.AddDays(3));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            DespatchDateController controller = new DespatchDateController();
            var date = controller.Get(new List<int>() { 1 }, new DateTime(2018,1,26));
            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            DespatchDateController controller = new DespatchDateController();
            var date = controller.Get(new List<int>() { 3 }, new DateTime(2018, 1, 25));
            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(4));
        }
    }
}
