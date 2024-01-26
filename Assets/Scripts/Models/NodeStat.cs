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
            Health = 5f;

            if (status == StatStatus.Deffended)
            {
                Deffense = 10f;
            }
        }

        public void Attack(float attackForce)
        {
            if (GameManager.instance.WaitingForNextTurn) return;
            
            // Generate a random number between 0 and 1
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            
            switch (Status)
            {
                case StatStatus.Deffended:
                    // Check if the attack hits or misses based on missChance
                    if (randomValue > .75f)
                    {
                        Deffense -= attackForce /* * 0.125f // disabled for now since it was taking too long to attack normal computers */;
                    }

                    if (Deffense <= 0f)
                    {
                        Status = StatStatus.Normal;
                    }
                    
                    break;
                case StatStatus.Exploited:
                    break;
                case StatStatus.Normal:
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
                Health = 0f;
                Status = StatStatus.Exploited;
            }
        }
        
        public void Defend(float deffenseForce)
        {
            if (GameManager.instance.WaitingForNextTurn) return;
            
            // Generate a random number between 0 and 1
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            
            switch (Status)
            {
                case StatStatus.Deffended:
                    break;
                case StatStatus.Exploited:
                    // Check if the deffense hits or misses based on missChance
                    if (randomValue > .75f)
                    {
                        Health += deffenseForce /* * 0.125f // disabled for now since it was taking too long to defend vulnerable computers */;
                    }

                    if (Health >= 5f)
                    {
                        Status = StatStatus.Normal;
                    }
                    break;
                case StatStatus.Vulnerable:
                    // Check if the deffense hits or misses based on missChance
                    if (randomValue > .75f)
                    {
                        Deffense += deffenseForce /* * 0.125f // disabled for now since it was taking too long to defend vulnerable computers */;
                    }
                    break;
                case StatStatus.Normal:
                    if (Health < 5f)
                    {
                        Health += 0.1f;
                    }
                    
                    Deffense += deffenseForce;
                    break;
            }

            if (Deffense >= 10f)
            {
                Deffense = 10f;
                Status = StatStatus.Deffended;
            }
        }
        
        public override string ToString()
        {
            return $"<b>{Identifier.ToString().Replace('_',' ')}</b> - {Status.ToString()}";
        }
    }
}