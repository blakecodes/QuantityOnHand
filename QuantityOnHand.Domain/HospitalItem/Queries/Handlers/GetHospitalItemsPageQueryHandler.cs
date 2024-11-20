using MediatR;
using QuantityOnHand.Data.Repositories.HospitalItems;
using QuantityOnHand.Domain.Shared.Models;

namespace QuantityOnHand.Domain.HospitalItem.Queries.Handlers;

public class GetHospitalItemsPageQueryHandler(IHospitalItemRepository hospitalItemRepository)
    : IRequestHandler<GetHospitalItemsPageQuery, PagedEntityResponse<Data.Entities.HospitalItem>>
{
    public async Task<PagedEntityResponse<Data.Entities.HospitalItem>> Handle(GetHospitalItemsPageQuery request,
        CancellationToken cancellationToken)
    {
        var data = await hospitalItemRepository.GetHospitalItemsPageAsync(
            request.ItemDescription,
            request.MinQuantity,
            request.MaxQuantity,
            request.PageNumber,
            request.PageSize);

        return new PagedEntityResponse<Data.Entities.HospitalItem>
        {
            Entities = data.entities,
            TotalCount = data.totalCount
        };
    }
}