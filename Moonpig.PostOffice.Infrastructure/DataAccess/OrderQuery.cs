using Moonpig.PostOffice.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moonpig.PostOffice.Infrastructure.DataAccess
{
    public interface IOrderQuery
    {
        Task<int> GetSupplierLeadTimeAsync(List<int> ids);
    }

    public class OrderQuery : IOrderQuery
    {
        private readonly IDbContext _dbContext;

        public OrderQuery(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> GetSupplierLeadTimeAsync(List<int> ids)
        {
            var query = (from p in _dbContext.Products
                         join s in _dbContext.Suppliers
                             on p.SupplierId equals s.SupplierId
                         where ids.Contains(p.ProductId)
                         select new
                         {
                             s.LeadTime
                         }).ToList();

            var lt = query.Max(q => q.LeadTime);

            return Task.FromResult(lt);
        }
    }
}
