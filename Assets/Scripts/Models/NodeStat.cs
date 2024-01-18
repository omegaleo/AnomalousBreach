using System;
using Enums;

namespace Models
{
    [Serializable]
    public class NodeStat
    {
        public StatIdentifier Identifier;
        public StatStatus Status;

        public NodeStat(StatIdentifier identifier, StatStatus status = StatStatus.Normal)
        {
            Identifier = identifier;
            Status = status;
        }
        
        public override string ToString()
        {
            return $"<b>{Identifier.ToString().Replace('_',' ')}</b> - {Status.ToString()}";
        }
    }
}