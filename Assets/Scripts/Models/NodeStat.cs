using System;
using Enums;

namespace Models
{
    [Serializable]
    public class NodeStat
    {
        public StatIdentifier Identifier;
        public StatStatus Status;
        public float Health = 5f;

        public NodeStat(StatIdentifier identifier, StatStatus status = StatStatus.Normal)
        {
            Identifier = identifier;
            Status = status;
        }

        public void Attack(float attackForce)
        {
            switch (Status)
            {
                case StatStatus.Deffended:
                case StatStatus.Exploited:
                    break;
                case StatStatus.Normal:
                    Health -= attackForce * .5f;
                    break;
                case StatStatus.Vulnerable:
                    Health -= attackForce;
                    break;
            }

            if (Health <= 0f)
            {
                Status = StatStatus.Exploited;
            }
        }
        
        public override string ToString()
        {
            return $"<b>{Identifier.ToString().Replace('_',' ')}</b> - {Status.ToString()}";
        }
    }
}