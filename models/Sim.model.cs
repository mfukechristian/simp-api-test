using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sims.API.Models
{
    [Index(nameof(MSISDN), IsUnique = true)]
    [Index(nameof(SIMNumber), IsUnique = true)]
    public class Sim
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        [Required]
        [MaxLength(32)]
        public string MSISDN { get; set; } = string.Empty;

        public long MSISDN_Int { get; set; }
        public long MSISDN_Num { get; set; }

        [MaxLength(32)]
        public string? IMEI { get; set; }

        [MaxLength(64)]
        public string? SIMNumber { get; set; }

        public DateTime? AddedDate { get; set; }

        [MaxLength(32)]
        public string? SIMStatus { get; set; }

        public bool? Locked { get; set; }
        public DateTime? LockDate { get; set; }

        [Required]
        public bool CanLock { get; set; }

        [ForeignKey(nameof(DataSet))]
        public int DataSetId { get; set; }
        public SimDataSet DataSet { get; set; } = null!;
    }
}
