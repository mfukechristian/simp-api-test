// Sim.Controller.cs
namespace Sims.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Sims.API.DTOs;
using Sims.API.Models;
using Sims.API.Repositories;

[ApiController]
[Route("api/[controller]")]
public class SIMController : ControllerBase
{
    private readonly ISimRepository _repo;

    public SIMController(ISimRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> GetDataSets()
    {
        var dataSets = await _repo.GetDataSetsAsync();
        var result = dataSets.Select(ds => new
        {
            ds.Id,
            ds.UploadedAtUtc
        });
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMetrics(int id)
    {
        var exists = await _repo.GetDataSetByIdAsync(id);
        if (exists is null) return NotFound();

        var metrics = await _repo.GetMetricsAsync(id);
        return Ok(metrics);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] SimUploadEnvelopeDto envelope)
    {
        if (envelope?.SIMs is null || envelope.SIMs.Count == 0)
            return BadRequest("No SIM records provided.");

        var sims = envelope.SIMs.Select(s => new Sim
        {
            TenantId = s.TenantId,
            MSISDN = s.MSISDN,
            MSISDN_Int = s.MSISDN_Int,
            MSISDN_Num = s.MSISDN_Num,
            IMEI = s.IMEI,
            SIMNumber = string.IsNullOrWhiteSpace(s.SIMNumber) ? null : s.SIMNumber,
            AddedDate = s.AddedDate,
            SIMStatus = s.SIMStatus,
            Locked = s.Locked,
            LockDate = s.LockDate,
            CanLock = s.CanLock
        }).ToList();

        var dataSetId = await _repo.AddDataSetAsync(sims);
        return CreatedAtAction(nameof(GetMetrics), new { id = dataSetId }, new { id = dataSetId });
    }
}
