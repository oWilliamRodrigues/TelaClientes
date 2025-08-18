using System;

namespace Europa.Domain.Shared
{
    public class ProjectPropertyDTO
    {
        public string Key { get; set; }
        public DateTime LastUpdate { get; set; }
        public object Value { get; set; }
        public int TimeToLive { get; set; }
    }
}
