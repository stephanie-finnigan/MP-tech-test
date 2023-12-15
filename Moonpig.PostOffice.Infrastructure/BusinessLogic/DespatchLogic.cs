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

            if (_mlt.AddDays(lt) > _mlt)
                _mlt = _mlt.AddDays(lt);

            _mlt = CalculateDespatch(_mlt);
            
            return new OrderResponseDto { Date = _mlt };
        }

        private static DateTime CalculateDespatch(DateTime date)
        {
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }

            return date;

            //return date.DayOfWeek switch
            //{
            //    DayOfWeek.Friday => date.AddDays(3),
            //    DayOfWeek.Saturday => date.AddDays(2),
            //    DayOfWeek.Sunday => date.AddDays(1),
            //    _ => date
            //};
        }
    }
}
