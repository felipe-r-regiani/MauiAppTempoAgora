using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppTempoAgora.Models
{
    public class Tempo
    {
        public double? lon { get; set; }
        public double? lat { get; set; }
        public double? temp_min { get; set; }
        public double? temp_max { get; set; }
        public double? temp { get; set; }
        public double? feels_like { get; set; }
        public int? visibility { get; set; }
        public int? timezone { get; set; }
        public DateTime? sunrise { get; set; }
        public DateTime? sunset { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
    }
}
