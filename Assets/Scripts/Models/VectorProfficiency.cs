using System;
using Enums;

namespace Models
{
    [Serializable]
    public class VectorProfficiency
    {
        public StatIdentifier Identifier;
        public int Level = 0;

        public VectorProfficiency(StatIdentifier identifier, int level = 0)
        {
            Identifier = identifier;
            Level = level;
        }
    }
}