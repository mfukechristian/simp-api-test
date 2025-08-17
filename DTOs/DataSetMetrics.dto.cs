namespace Sims.API.DTOs;

public class DataSetMetricsDto
{
    public int DataSetId { get; set; }
    public int RowCount { get; set; }
    public string? MostCommonAreaCode { get; set; }
    public int MsisdnsContaining123 { get; set; }
}
