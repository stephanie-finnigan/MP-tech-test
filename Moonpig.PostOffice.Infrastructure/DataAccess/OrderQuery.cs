using Moonpig.PostOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            DbContext dbContext = new DbContext();
            var s = dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
            var lt = dbContext.Suppliers.Single(x => x.SupplierId == s).LeadTime;

            return null;
        }
    }
}
