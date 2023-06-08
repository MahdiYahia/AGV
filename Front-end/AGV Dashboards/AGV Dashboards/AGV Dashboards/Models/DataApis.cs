using System;
using System.Collections.Generic;
using System.Text;

namespace AGV_Dashboards.Models
{
    public class ResourcesApiData
    {
        public MemoryData Memory { get; set; }
        public CPUData CPU { get; set; }
    }

    public class MemoryData
    {
        public double Consumed { get; set; }
        public double Available { get; set; }
    }
    public class CPUData
    {
        public double Utilization { get; set; }
    }
}
