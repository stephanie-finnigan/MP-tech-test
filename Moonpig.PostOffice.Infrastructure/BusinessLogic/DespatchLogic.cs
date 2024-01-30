using Moonpig.PostOffice.Infrastructure.DataAccess;
using Moonpig.PostOffice.Model.Dto;

namespace Moonpig.PostOffice.Infrastructure.BusinessLogic
{
    public interface IDespatchLogic
    {
        Task<OrderResponseDto> GetDespatchDateAsync(OrderRequestDto request);
    }

    public class DespatchLogic : IDespatchLogic
    {
        private readonly IOrderQuery _orderQuery;

        public DespatchLogic(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        public async Task<OrderResponseDto> GetDespatchDateAsync(OrderRequestDto request)
        {

            var _mlt = request.OrderDate; // max lead time


            var lt = await _orderQuery.GetSupplierLeadTimeAsync(request.ProductIds);

            var date = CalculateDespatchDate(_mlt, lt);
            
            return new OrderResponseDto { Date = date };
        }

        private static DateTime CalculateDespatchDate(DateTime orderDate, int leadTime)
        {
            var d = orderDate;

            if (d.TimeOfDay > new TimeSpan(17, 30, 00) || 
                (d.DayOfWeek != DayOfWeek.Saturday || d.DayOfWeek != DayOfWeek.Sunday))
            {
                d = d.AddDays(1);
            }

            for (var i = 0; i < leadTime; i++)
            {
                while (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    d = d.AddDays(1);
                }
                d = d.AddDays(1);
            }

            while (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
            {
                d = d.AddDays(1);
            }
            return d;
        }
    }
}
