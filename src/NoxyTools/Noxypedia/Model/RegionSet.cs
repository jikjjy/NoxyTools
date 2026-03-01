using System;
using System.Collections.Generic;

namespace Noxypedia.Model
{
    [Serializable]
    public class RegionSet : BaseModel
    {
        public List<CreepSet> Creeps { get; set; } = new List<CreepSet>();
        public List<LocationSet> Locations { get; set; } = new List<LocationSet>();
    }
}