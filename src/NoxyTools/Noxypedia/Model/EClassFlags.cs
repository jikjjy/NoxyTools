using System;

namespace Noxypedia.Model
{
    [Flags]
    public enum EClassFlags
    {
        Knight = 1 << 0,
        Wizard = 1 << 1,
        Priest = 1 << 2,
        Archer = 1 << 3,
        Druid = 1 << 4,
        Summoner = 1 << 5,

        Common = Knight | Wizard | Priest | Archer | Druid | Summoner
    }
}