using CowsAndChicken.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class StatsViewModel
    {
        public Stats SpeedStats { get; set; }

        public Dictionary<int, Stats> EfficiencyStats { get; set; }
    }
}
