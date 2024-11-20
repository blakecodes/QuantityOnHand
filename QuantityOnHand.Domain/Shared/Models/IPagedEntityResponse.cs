namespace QuantityOnHand.Domain.Shared.Models;

/// <summary>
///     Represents a response for a paged entity request, containing a list of entities and the total count.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class PagedEntityResponse<T>
{
    public List<T> Entities { get; set; }
    public int TotalCount { get; set; }
}