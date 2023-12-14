using Moonpig.PostOffice.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Moonpig.PostOffice.Infrastructure.DataAccess
{
    public interface IOrderQuery
    {
        Task<int> GetOrderSupplierLeadTime(int productId);
    }

    public class OrderQuery : IOrderQuery
    {
        private readonly IDbContext _dbContext;

        public OrderQuery(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> GetOrderSupplierLeadTime(int productId)
        {
            var query = (from p in _dbContext.Products
                         join s in _dbContext.Suppliers
                             on p.SupplierId equals s.SupplierId
                         where p.ProductId == productId
                         select new
                         {
                             s.LeadTime
                         }).SingleOrDefault();

            return Task.FromResult(query.LeadTime);
        }
    }
}
