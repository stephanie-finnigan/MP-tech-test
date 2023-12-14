using Moonpig.PostOffice.Model.Dto;

namespace Moonpig.PostOffice.Infrastructure.BusinessLogic
{
    public interface IDespatchLogic
    {
        Task<DespatchDateResponseDto> GetDespatchDateAsync(DespatchDateRequestDto request);
    }

    public class DespatchLogic : IDespatchLogic
    {


        public async Task<DespatchDateResponseDto> GetDespatchDateAsync(DespatchDateRequestDto request)
        {
            var _mlt = request.OrderDate; // max lead time
            foreach (var ID in request.ProductIds)
            {
                DbContext dbContext = new DbContext();
                var s = dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                var lt = dbContext.Suppliers.Single(x => x.SupplierId == s).LeadTime;
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
