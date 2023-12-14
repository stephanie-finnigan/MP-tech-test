using System.Linq;
using Moonpig.PostOffice.Data.Entities;

namespace Moonpig.PostOffice.Data
{
    public interface IDbContext
    {
        IQueryable<Supplier> Suppliers { get; }

        IQueryable<Product> Products { get; }
    }
}
