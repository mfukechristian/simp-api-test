namespace Sims.API.DTOs;

public class SimUploadDto
{
    public int? TenantId { get; set; }
    public string MSISDN { get; set; } = string.Empty;
    public long MSISDN_Int { get; set; }
    public long MSISDN_Num { get; set; }
    public string? IMEI { get; set; }
    public string? SIMNumber { get; set; }
    public DateTime? AddedDate { get; set; }
    public string? SIMStatus { get; set; }
    public bool? Locked { get; set; }
    public DateTime? LockDate { get; set; }
    public bool CanLock { get; set; }
}
