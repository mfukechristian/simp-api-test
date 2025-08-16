namespace Sims.API.Models;

public class SimDataSet
{
    public int Id { get; set; }
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<Sim> Sims { get; set; } = new List<Sim>();
}
