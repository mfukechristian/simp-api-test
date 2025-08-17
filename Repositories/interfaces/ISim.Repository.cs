namespace Sims.API.Repositories;

using Sims.API.Models;
using Sims.API.DTOs;

public interface ISimRepository
{
    Task<int> AddDataSetAsync(List<Sim> sims);
    Task<List<SimDataSet>> GetDataSetsAsync();
    Task<SimDataSet?> GetDataSetByIdAsync(int id);
    Task<DataSetMetricsDto> GetMetricsAsync(int dataSetId);
}
