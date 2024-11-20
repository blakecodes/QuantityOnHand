using Microsoft.EntityFrameworkCore;

namespace QuantityOnHand.Data.Utilities;

public class DatabaseSeeder(ApplicationDbContext context, string filePath)
{
    /// <summary>
    ///     Seeds the database with data from the specified file.
    ///     This method will only seed the database if there is no existing data.
    /// </summary>
    /// <remarks>
    ///     The file should be a pipe-delimited text file with the following columns:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>ItemNo</term>
    ///             <description>The item number.</description>
    ///         </item>
    ///         <item>
    ///             <term>ItemDescription</term>
    ///             <description>The item description.</description>
    ///         </item>
    ///         <item>
    ///             <term>Quantity</term>
    ///             <description>The quantity of the item.</description>
    ///         </item>
    ///         <item>
    ///             <term>Price</term>
    ///             <description>The price per unit of the item.</description>
    ///         </item>
    ///     </list>
    ///     The first line of the file should contain the header row.
    /// </remarks>
    public async Task SeedAsync()
    {
        if (await context.HospitalItems.AnyAsync()) return;

        // Read and import data
        var lines = await File.ReadAllLinesAsync(filePath);

        // Skip the header line and process the rest
        var items = lines.Skip(1).Select(line =>
        {
            var parts = line.Split('|');
            return new HospitalItem
            {
                ItemNo = parts[0],
                ItemDescription = parts[1],
                Quantity = int.Parse(parts[2]),
                Price = decimal.Parse(parts[3])
            };
        });

        await context.HospitalItems.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}