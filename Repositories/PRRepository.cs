using Microsoft.EntityFrameworkCore;
using PRChecksheetApp.Data;
using PRChecksheetApp.Models;
using PRChecksheetApp.Repositories.Interfaces;

namespace PRChecksheetApp.Repositories;

public class PRRepository(ApplicationDbContext context) : IPRRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddGraphAsync(PurchaseRequisitionHeader header)
    {
        await _context.PurchaseRequisitionHeaders.AddAsync(header);
        await _context.SaveChangesAsync();
    }

    public async Task<long> GetNextPrSequenceAsync()
    {
        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

        await using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = "UPDATE PRNumberSequence SET NextNumber = LAST_INSERT_ID(NextNumber + 1) WHERE Id = 1;";
        await cmd1.ExecuteNonQueryAsync();

        await using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = "SELECT LAST_INSERT_ID();";
        var result = await cmd2.ExecuteScalarAsync();

        return Convert.ToInt64(result);
    }
}
