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
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 1 },
                OrderDate = new DateTime(2018, 01, 01)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(1);
            
            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            //Then
            _responseDto.Date.Date.Should().Be(_requestDto.OrderDate.AddDays(1));
        }

        [Fact]
        public async Task OneProductWithLeadTimeOfTwoDay()
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 2 },
                OrderDate = new DateTime(2018, 01, 01)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(2);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Date.Should().Be(_requestDto.OrderDate.AddDays(2));
        }

        [Fact]
        public async Task OneProductWithLeadTimeOfThreeDay()
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 3 },
                OrderDate = new DateTime(2018, 01, 01)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(3);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Date.Should().Be(_requestDto.OrderDate.AddDays(3));
        }

        [Fact]
        public async Task SaturdayHasExtraTwoDays()
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 1 },
                OrderDate = new DateTime(2018, 1, 26)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(1);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            _responseDto.Date.Should().Be(_requestDto.OrderDate.AddDays(3));
        }

        [Fact]
        public async Task SundayHasExtraDay()
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 3 },
                OrderDate = new DateTime(2018, 1, 25)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(3);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            // Then
            _responseDto.Date.Should().Be(_requestDto.OrderDate.AddDays(4));
        }

        [Fact]
        public async Task TwoProductsWithLeadTimeOfThreeDaysTotal()
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 2, 4 },
                OrderDate = new DateTime(2018, 01, 01)
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(2);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            // Then
            _responseDto.Date.Should().Be(_requestDto.OrderDate.AddDays(2));
        }

        [Theory]
        [InlineData("2018-01-05", "2018-01-08")]
        [InlineData("2018-01-06", "2018-01-09")]
        [InlineData("2018-01-07", "2018-01-09")]
        public async Task LeadTimeNotCountedOverWeekend(DateTime orderDate, DateTime expOrderDate)
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { 1 },
                OrderDate = orderDate
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(1);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            // Then
            _responseDto.Date.Should().Be(expOrderDate);
        }

        [Theory]
        [InlineData(9, "2018-01-05", 6, "2018-01-15")]
        [InlineData(10, "2018-01-05", 11,  "2018-01-22")]
        public async Task LeadTimeOverMultipleWeeks(int prodId, DateTime orderDate, int lt, DateTime expOrderDate)
        {
            // Given
            _requestDto = new OrderRequestDto
            {
                ProductIds = new List<int> { prodId },
                OrderDate = orderDate
            };
            _orderQueryMock.Setup(q => q.GetSupplierLeadTimeAsync(It.IsAny<List<int>>())).ReturnsAsync(lt);

            // When
            _responseDto = await _logic.GetDespatchDateAsync(_requestDto);

            // Then
            _responseDto.Date.Should().Be(expOrderDate);
        }
    }
}
