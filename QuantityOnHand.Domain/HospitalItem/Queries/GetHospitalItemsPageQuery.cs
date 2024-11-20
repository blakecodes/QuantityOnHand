using MediatR;
using QuantityOnHand.Domain.Shared.Models;

namespace QuantityOnHand.Domain.HospitalItem.Queries;

public class GetHospitalItemsPageQuery : IRequest<PagedEntityResponse<Data.Entities.HospitalItem>>
{
    public string? ItemDescription { get; set; }
    public int? MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }

    // Pagination parameters
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}