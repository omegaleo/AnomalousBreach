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
        public float Deffense = 0f;

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
                    // Generate a random number between 0 and 1
                    float randomValue = UnityEngine.Random.Range(0f, 1f);

                    // Check if the attack hits or misses based on missChance
                    if (randomValue > .75f)
                    {
                        Health -= attackForce /* * 0.125f // disabled for now since it was taking too long to attack normal computers */;
                    }
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
        
        public void Defend(float deffenseForce)
        {
            switch (Status)
            {
                case StatStatus.Deffended:
                case StatStatus.Exploited:
                    break;
                case StatStatus.Vulnerable:
                    // Generate a random number between 0 and 1
                    float randomValue = UnityEngine.Random.Range(0f, 1f);

                    // Check if the deffense hits or misses based on missChance
                    if (randomValue > .75f)
                    {
                        Deffense += deffenseForce /* * 0.125f // disabled for now since it was taking too long to defend vulnerable computers */;
                    }
                    break;
                case StatStatus.Normal:
                    Deffense += deffenseForce;
                    break;
            }

            if (Deffense >= 10f)
            {
                Status = StatStatus.Deffended;
            }
        }
        
        public override string ToString()
        {
            return $"<b>{Identifier.ToString().Replace('_',' ')}</b> - {Status.ToString()}";
        }
    }
}