using System.Linq.Expressions;
using QuantityOnHand.Data.Entities;

namespace QuantityOnHand.Data.Repositories.HospitalItems;

public class HospitalItemRepository(ApplicationDbContext context)
    : BaseRepository<HospitalItem>(context), IHospitalItemRepository
{
    public async Task<IEnumerable<HospitalItem>> GetHospitalItemsPageAsync(string? itemDescription, int? minQuantity,
        int? maxQuantity, int pageNumber, int pageSize)
    {
        Expression<Func<HospitalItem, bool>>? filter = item =>
            (string.IsNullOrEmpty(itemDescription) || item.ItemDescription.Contains(itemDescription)) &&
            (!minQuantity.HasValue || item.Quantity >= minQuantity.Value) &&
            (!maxQuantity.HasValue || item.Quantity <= maxQuantity.Value);

        return await GetPageAsync(filter, OrderBy, pageNumber, pageSize);

        IOrderedQueryable<HospitalItem> OrderBy(IQueryable<HospitalItem> items)
        {
            return items.OrderBy(item => item.ItemNo);
        }
    }
}