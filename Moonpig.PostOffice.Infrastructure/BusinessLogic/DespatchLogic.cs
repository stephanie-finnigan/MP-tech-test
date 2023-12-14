using Moonpig.PostOffice.Infrastructure.DataAccess;
using Moonpig.PostOffice.Model.Dto;

namespace Moonpig.PostOffice.Infrastructure.BusinessLogic
{
    public interface IDespatchLogic
    {
        Task<DespatchDateResponseDto> GetDespatchDateAsync(DespatchDateRequestDto request);
    }

    public class DespatchLogic : IDespatchLogic
    {
        private readonly IOrderQuery _orderQuery;

        public DespatchLogic(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        public async Task<DespatchDateResponseDto> GetDespatchDateAsync(DespatchDateRequestDto request)
        {
            var _mlt = request.OrderDate; // max lead time
            foreach (var id in request.ProductIds)
            {
                var lt = await _orderQuery.GetOrderSupplierLeadTime(id);

                if (request.OrderDate.AddDays(lt) > _mlt)
                    _mlt = request.OrderDate.AddDays(lt);
            }

            if (_mlt.DayOfWeek == DayOfWeek.Saturday)
            {
                return new DespatchDateResponseDto { Date = _mlt.AddDays(2) };
            }
            else if (_mlt.DayOfWeek == DayOfWeek.Sunday) 
                return new DespatchDateResponseDto { Date = _mlt.AddDays(1) };
            else 
                return new DespatchDateResponseDto { Date = _mlt };
        }
    }
}
