using QuantityOnHand.Data.Entities;

namespace QuantityOnHand.Data.Repositories.HospitalItems;

public interface IHospitalItemRepository : IBaseRepository<HospitalItem>
{
    Task<IEnumerable<HospitalItem>> GetHospitalItemsPageAsync(string? itemDescription, int? minQuantity,
        int? maxQuantity, int pageNumber, int pageSize);
}