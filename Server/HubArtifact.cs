using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class HubArtifact
    {
        public static List<string> Messages { get; set; } = new List<string>();
        public static int OnlineUserCount { get; set; }
    }
}
