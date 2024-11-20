using QuantityOnHand.Data.Entities;

namespace QuantityOnHand.Data.Repositories.HospitalItems;

public interface IHospitalItemRepository : IBaseRepository<HospitalItem>
{
    /// <summary>
    ///     Retrieves a paginated list of hospital items based on provided filtering criteria.
    /// </summary>
    /// <param name="itemDescription">The description of the hospital item to filter by. Can be null to match any description.</param>
    /// <param name="minQuantity">The minimum quantity of the hospital item to filter by. Can be null to match any quantity.</param>
    /// <param name="maxQuantity">The maximum quantity of the hospital item to filter by. Can be null to match any quantity.</param>
    /// <param name="pageNumber">The number of the page to retrieve. Must be a positive integer.</param>
    /// <param name="pageSize">The size of the page to retrieve. Must be a positive integer.</param>
    /// <returns>A tuple containing a list of hospital items that match the criteria and the total count of matching items.</returns>
    Task<(List<HospitalItem> entities, int totalCount)> GetHospitalItemsPageAsync(string? itemDescription,
        int? minQuantity,
        int? maxQuantity, int pageNumber, int pageSize);
}