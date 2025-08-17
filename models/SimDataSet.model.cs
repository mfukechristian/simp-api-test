using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sims.API.Models
{
    public class SimDataSet
    {
        public int Id { get; set; }

        [Required]
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<Sim> Sims { get; set; } = new List<Sim>();
    }
}
