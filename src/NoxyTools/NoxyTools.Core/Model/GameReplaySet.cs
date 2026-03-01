using ReplayParser;
using System.IO;

namespace NoxyTools.Core.Model
{
    public class GameReplaySet
    {
        public ReplayInfoSet Replay { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}
