using FluentAssertions;
using Moonpig.PostOffice.Infrastructure.BusinessLogic;
using Moonpig.PostOffice.Infrastructure.DataAccess;
using Moonpig.PostOffice.Model.Dto;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task OneProductWithLeadTimeOfOneDay()
        {
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int>() { 1 },
                OrderDate = DateTime.Now
            };

            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Date.Should().Be(DateTime.Now.Date.AddDays(1));
        }

        [Fact]
        public async Task OneProductWithLeadTimeOfTwoDay()
        {
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int>() { 2 },
                OrderDate = DateTime.Now
            };

            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Date.Should().Be(DateTime.Now.Date.AddDays(2));
        }

        [Fact]
        public async Task OneProductWithLeadTimeOfThreeDay()
        {
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int>() { 3 },
                OrderDate = DateTime.Now
            };

            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Date.Should().Be(DateTime.Now.Date.AddDays(3));
        }

        [Fact]
        public async Task SaturdayHasExtraTwoDays()
        {
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int>() { 1 },
                OrderDate = new DateTime(2018, 1, 26)
            };

            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Should().Be(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public async Task SundayHasExtraDay()
        {
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int>() { 3 },
                OrderDate = new DateTime(2018, 1, 25)
            };

            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Should().Be(new DateTime(2018, 1, 25).Date.AddDays(4));
        }
    }
}
