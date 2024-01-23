using System.Collections.Generic;
using System.Linq;
using Enums;
using OmegaLeo.Toolbox.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeButton : Button
    {
        [ColoredHeader("Configurations")]
        [SerializeField] [TextArea] private string _description = "This is an example description for the upgrade in question";
        [SerializeField] private int _cost = 1;
        [SerializeField] private StatIdentifier _attackVector;
        [SerializeField] private int _increase = 1;
        [SerializeField] private List<UpgradeButton> _unlockNext = new List<UpgradeButton>();
        
        private bool _claimed = false;

        protected override void Start()
        {
            base.Start();
            onClick.AddListener(Claim);
        }

        public void Unlock()
        {
            interactable = true;
        }
        
        public void Claim()
        {
            if (Player.instance.UpgradePoints >= _cost)
            {
                Player.instance.LevelUpProfficiency(_attackVector, _increase);
                Player.instance.UpgradePoints -= _cost;
                _claimed = true;

                if (_unlockNext.Any())
                {
                    _unlockNext.ForEach(x => x.Unlock());
                }
            }
        }
    }
}