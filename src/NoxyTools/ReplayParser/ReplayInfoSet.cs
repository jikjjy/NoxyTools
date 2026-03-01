using System;
using System.Collections.Generic;

namespace ReplayParser
{
    [Serializable]
    public class ReplayInfoSet
    {
        public bool IsReplay { get; set; } = false;
        public bool IsSupport { get; set; } = false;
        public bool IsReforged => W3VersionNo > 10000;
        public string W3VersionID { get; set; } = string.Empty;
        public uint W3VersionNo { get; set; } = 0;
        public ushort W3BuildNo { get; set; } = 0;
        public TimeSpan ReplayLength { get; set; } = TimeSpan.Zero;
        public string GameRoomName { get; set; } = string.Empty;
        public string MapFileName { get; set; } = string.Empty;
        public string GameRoomCreationPlayerName { get; set; } = string.Empty;
        public List<string> PlayerNames { get; } = new List<string>();
    }
}
