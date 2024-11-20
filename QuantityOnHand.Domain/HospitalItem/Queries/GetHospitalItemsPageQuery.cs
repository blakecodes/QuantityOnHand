using MediatR;

namespace QuantityOnHand.Domain.HospitalItem.Queries;

public abstract class GetHospitalItemsPageQuery : IRequest<Data.Entities.HospitalItem>,
    IRequest<IEnumerable<Data.Entities.HospitalItem>>
{
    public string? ItemDescription { get; set; }
    public int? MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }


    // TODO: Implement base pagination command
    // Pagination parameters
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}