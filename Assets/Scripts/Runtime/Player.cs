using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Models;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class Player : InstancedBehavior<Player>
{
    [SerializeField] private GameObject _image;
    
    public PlayerSide Side;

    [SerializeField] private Node _workingOn; // The node our player is currently protecting/attacking

    // For now this will be used for both attacking and defending
    [SerializeField] private List<VectorProfficiency> _profficiencies = new List<VectorProfficiency>();

    public int UpgradePoints = 0;
    
    private void Start()
    {
        var identifiers = Enum.GetValues(typeof(StatIdentifier));

        foreach (var identifier in identifiers)
        {
            _profficiencies.Add(new VectorProfficiency((StatIdentifier)identifier));
        }
    }

    public void SetSide(PlayerSide side)
    {
        Side = side;
        UpgradePoints = (Side == PlayerSide.Attack) ? 1 : 5;
    }

    public void LevelUpProfficiency(StatIdentifier identifier, int increase = 1) =>
        _profficiencies.FirstOrDefault(x => x.Identifier == identifier)!.Level += increase;
    
    public void SetWorkingOnNode(Node node)
    {
        _workingOn = node;
        
        GameManager.instance.NextTurn();
        
        RegionInfo.instance.Close();
        
        _image.SetActive(true);

        var workingOnRect = _workingOn.GetComponent<RectTransform>().rect;

        var offset = new Vector3(workingOnRect.width / 2 - 25f, workingOnRect.height / 2 - 25f, 0f);
        
        _image.transform.localPosition = _workingOn.transform.localPosition - offset;
        
        StartCoroutine(HandleEffect());
    }

    private IEnumerator HandleEffect()
    {
        while (!GameManager.instance.WaitingForNextTurn)
        {
            if (GameManager.instance.WaitingForNextTurn) break;
            
            var profficientStats = _profficiencies.Where(x => x.Level > 0);
            
            switch (Side)
            {
                case PlayerSide.Attack:
                    foreach (var profficiency in profficientStats)
                    {
                        _workingOn.Attack(profficiency.Identifier, profficiency.Level /* * 0.125f*/);
                    }
                    break;
                case PlayerSide.Defend:
                    foreach (var profficiency in profficientStats)
                    {
                        _workingOn.Defense(profficiency.Identifier, profficiency.Level /* * 0.125f */);
                    }
                    break;
            }
            
            yield return new WaitForSeconds(10f); // Wait 10 seconds before applying the attack/defense again
        }
    }
}
