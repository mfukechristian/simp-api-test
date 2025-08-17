namespace Sims.API.Repositories;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sims.API.Data;
using Sims.API.DTOs;
using Sims.API.Models;

public class SimRepository : ISimRepository
{
    private readonly AppDbContext _context;

    public SimRepository(AppDbContext context) => _context = context;


public async Task<int> AddDataSetAsync(List<Sim> sims)
{
    static string? Key(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        var digits = new string(s.Where(char.IsDigit).ToArray());
        return string.IsNullOrEmpty(digits) ? null : digits;
    }

    var existing = await _context.Sims
        .AsNoTracking()
        .Select(s => new { s.MSISDN, s.SIMNumber })
        .ToListAsync();

    var seenMsisdn = new HashSet<string>();
    var seenSimnum = new HashSet<string>();
    foreach (var e in existing)
    {
        var k1 = Key(e.MSISDN);
        var k2 = Key(e.SIMNumber);
        if (k1 != null) seenMsisdn.Add(k1);
        if (k2 != null) seenSimnum.Add(k2);
    }

    var distinct = sims
        .Select(s => new { S = s, K1 = Key(s.MSISDN), K2 = Key(s.SIMNumber) })
        .Where(x => x.K1 != null || x.K2 != null)
        .GroupBy(x => (x.K1, x.K2))
        .Select(g => g.First())
        .Where(x => (x.K1 == null || seenMsisdn.Add(x.K1)) &&
                    (x.K2 == null || seenSimnum.Add(x.K2)))
        .Select(x =>
        {
            if (x.K1 != null) x.S.MSISDN = x.K1;
            x.S.SIMNumber = x.K2; // set to null if blank to avoid UNIQUE on ""
            return x.S;
        })
        .ToList();

    var dataSet = new SimDataSet { UploadedAtUtc = DateTime.UtcNow, Sims = distinct };
    _context.DataSets.Add(dataSet);
    await _context.SaveChangesAsync();
    return dataSet.Id;
}

    public Task<List<SimDataSet>> GetDataSetsAsync() =>
        _context.DataSets
            .AsNoTracking()
            .OrderByDescending(ds => ds.UploadedAtUtc)
            .ToListAsync();

    public Task<SimDataSet?> GetDataSetByIdAsync(int id) =>
        _context.DataSets
            .Include(ds => ds.Sims)
            .FirstOrDefaultAsync(ds => ds.Id == id);

    public async Task<DataSetMetricsDto> GetMetricsAsync(int dataSetId)
    {
        var sims = _context.Sims
            .AsNoTracking()
            .Where(s => s.DataSetId == dataSetId);

        var rowCount = await sims.CountAsync();
        var msisdnsContaining123 = await sims.CountAsync(s => s.MSISDN.Contains("123"));

        var top = await sims
            .Where(s => s.MSISDN.Length >= 3)
            .Select(s => s.MSISDN.Substring(0, 3))
            .GroupBy(ac => ac)
            .Select(g => new { AreaCode = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.AreaCode)
            .FirstOrDefaultAsync();

        return new DataSetMetricsDto
        {
            DataSetId = dataSetId,
            RowCount = rowCount,
            MostCommonAreaCode = top?.AreaCode,
            MsisdnsContaining123 = msisdnsContaining123
        };
    }
}
