using Microsoft.EntityFrameworkCore;
using QuantityOnHand.Data.Entities;

namespace QuantityOnHand.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<HospitalItem> HospitalItems { get; set; }
}