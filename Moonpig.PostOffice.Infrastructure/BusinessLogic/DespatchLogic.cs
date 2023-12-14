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

            if (request.OrderDate.AddDays(lt) > _mlt)
                _mlt = request.OrderDate.AddDays(lt);

            var despatchDate = GetDate(_mlt);

            return new OrderResponseDto { Date = despatchDate };
        }

        private static DateTime GetDate(DateTime mlt)
        {
            return mlt.DayOfWeek switch
            {
                DayOfWeek.Saturday => mlt.AddDays(2),
                DayOfWeek.Sunday => mlt.AddDays(1),
                _ => mlt,
            };
        }
    }
}
