using MediatR;
using QuantityOnHand.Data.Repositories.HospitalItems;

namespace QuantityOnHand.Domain.HospitalItem.Queries.Handlers;

public class GetHospitalItemsPageQueryHandler(IHospitalItemRepository hospitalItemRepository)
    : IRequestHandler<GetHospitalItemsPageQuery, IEnumerable<Data.Entities.HospitalItem>>
{
    public async Task<IEnumerable<Data.Entities.HospitalItem>> Handle(GetHospitalItemsPageQuery request,
        CancellationToken cancellationToken)
    {
        return await hospitalItemRepository.GetHospitalItemsPageAsync(
            request.ItemDescription,
            request.MinQuantity,
            request.MaxQuantity,
            request.PageNumber,
            request.PageSize);
    }
}