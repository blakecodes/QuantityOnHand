using System.ComponentModel.DataAnnotations;

namespace QuantityOnHand.Data;

public class HospitalItem
{
    [Key]
    public Guid Id { get; set; }    
    public string ItemNo { get; set; }
    public string ItemDescription { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}