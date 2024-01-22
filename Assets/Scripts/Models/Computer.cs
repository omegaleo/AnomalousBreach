using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using OmegaLeo.Toolbox.Attributes;
using Random = UnityEngine.Random;

namespace Models
{
    [Serializable]
    public class Computer
    {
        [ColoredHeader("Information")] 
        public List<NodeStat> Stats = new List<NodeStat>()
        {
            new NodeStat(StatIdentifier.Malware),
            new NodeStat(StatIdentifier.Phishing),
            new NodeStat(StatIdentifier.Authentication_Exploits),
            new NodeStat(StatIdentifier.Code_Injection),
            new NodeStat(StatIdentifier.Zombie_DDOS),
            new NodeStat(StatIdentifier.Social_Engineering),
        };
        
        public bool Breached => Stats.Any(x => x.Status == StatStatus.Exploited);

        public Computer()
        {
            Array values = Enum.GetValues(typeof(StatStatus));
            var random = new System.Random();
            
            foreach (var stat in Stats)
            {
                var status = (StatStatus)values.GetValue(random.Next(values.Length));

                if (status == StatStatus.Exploited) status = StatStatus.Vulnerable;

                stat.Status = status;
            }
        }
    }
}