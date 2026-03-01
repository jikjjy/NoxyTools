using System;

namespace Noxypedia.Model
{
    [Serializable]
    public class LocationSet : BaseModel
    {
        public RegionSet Region { get; set; } = new RegionSet();
    }
}