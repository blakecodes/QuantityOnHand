using System.ComponentModel.DataAnnotations;

namespace QuantityOnHand.Data.Entities;

public class HospitalItem : IBaseEntity
{
    [Key]
    public Guid Id { get; set; }    
    
    [MaxLength(50)]
    public required string ItemNo { get; set; }
    
    [MaxLength(100)]
    public required string ItemDescription { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}