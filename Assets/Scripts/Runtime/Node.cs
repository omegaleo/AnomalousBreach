using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Models;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Node : Button
{
    [ColoredHeader("Information")]
    public List<Computer> ComputersInRegion = new List<Computer>();
    
    public List<NodeStat> Stats
    {
        get
        {
            var allNodeStats = ComputersInRegion.SelectMany(computer => computer.Stats).ToList();

            var averageStatusByIdentifier = allNodeStats
                .GroupBy(stat => stat.Identifier)
                .Select(group => new
                {
                    Identifier = group.Key,
                    AverageStatus = (StatStatus)Math.Round(group.Average(stat => (int)stat.Status))
                })
                .ToList();

            return averageStatusByIdentifier.Select(x => new NodeStat(x.Identifier, x.AverageStatus)).ToList();
        }
    }
    
    [ColoredHeader("Configurations")]
    [SerializeField] private Image _deffendedTexture;
    [SerializeField] private Image _attackedTexture;
    [SerializeField] private TMP_Text _attackedPercentageText;
    [SerializeField] private List<Node> _nodesInProximity = new List<Node>();

    private bool _sfxPlayed = false;
    
    private float _attackedAmount
    {
        get
        {
            if (ComputersInRegion == null) return 0.0f;

            var attackedMachines = ComputersInRegion.Count(x => x.Breached);
            
            return (float)attackedMachines / (float)ComputersInRegion.Count;
        }
    }

    private float _deffendedAmount
    {
        get
        {
            if (ComputersInRegion == null) return 0.0f;

            var defendedMachines = ComputersInRegion.Count(x => x.Defended);
            
            return (float)defendedMachines / (float)ComputersInRegion.Count;
        }
    }

    public bool TakenOver => _attackedAmount == 1f;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        _nodesInProximity.Clear();
        ComputersInRegion.Clear();
        
        _deffendedTexture.color = GameManager.instance.GetDefendedColor();
        _attackedTexture.color = GameManager.instance.GetAttackedColor();

        int amountOfComputers = Random.Range(200, 1000);
        
        for (int i = 0; i < amountOfComputers; i++)
        {
            ComputersInRegion.Add(new Computer());
        }
        
        SetNodesInProximity();

        UpdateText();
    }

    private void SetNodesInProximity()
    {
        // We need to check the positions in all directions
        var currentPosition = transform.localPosition;
        var rectTransform = GetComponent<RectTransform>();
        var height = rectTransform.rect.height;
        var width = rectTransform.rect.width;

        var positionsToCheck = new[]
        {
            new Vector3(currentPosition.x - width, currentPosition.y + height, currentPosition.z), // Top Left
            new Vector3(currentPosition.x, currentPosition.y + height, currentPosition.z), // Top
            new Vector3(currentPosition.x + width, currentPosition.y + height, currentPosition.z), // Top Right
            
            new Vector3(currentPosition.x - width, currentPosition.y, currentPosition.z), // Left
            new Vector3(currentPosition.x + width, currentPosition.y, currentPosition.z), // Right
            
            new Vector3(currentPosition.x - width, currentPosition.y - height, currentPosition.z), // Bottom Left
            new Vector3(currentPosition.x, currentPosition.y - height, currentPosition.z), // Bottom
            new Vector3(currentPosition.x + width, currentPosition.y - height, currentPosition.z), // Bottom Right
        };

        foreach (var pos in positionsToCheck)
        {
            var node = NodeManager.instance.GetNode(pos);

            if (node != null)
            {
                _nodesInProximity.Add(node.GetComponent<Node>());
            }
        }
    }
    
    private void UpdateText()
    {
        _attackedTexture.fillAmount = _attackedAmount;
        _attackedPercentageText.text = $"{GetAttackedPercentage()}%";

        if (GetAttackedPercentage() > 100 && !_sfxPlayed)
        {
            AudioManager.instance.Play("NodeDeath");
        }
    }

    public float GetAttackedPercentage()
    {
        return (_attackedAmount * 100f).RoundToInt();
    }

    public float GetDeffendedPercentage()
    {
        return (_deffendedAmount * 100f).RoundToInt();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        RegionInfo.instance.Open(this);
        AudioManager.instance.Play("Click");
        base.OnSelect(eventData);
    }

    private void OnMouseOver()
    {
        RegionInfo.instance.Open(this);
    }

    private void OnMouseEnter()
    {
        RegionInfo.instance.Open(this);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        RegionInfo.instance.Close();
        base.OnDeselect(eventData);
    }

    private void OnMouseExit()
    {
        RegionInfo.instance.Close();
    }

    public void Attack(StatIdentifier stat, float attackForce = .5f, int iteration = 0)
    {
        StartCoroutine(PerformAttack(stat, attackForce));
        
        if (iteration == 0)
        {
            foreach (var nearbyNode in _nodesInProximity)
            {
                nearbyNode.Attack(stat, attackForce * 0.25f, iteration + 1); // Divide the force by 4 in nearby nodes
            }
        }
    }

    private IEnumerator PerformAttack(StatIdentifier stat,float attackForce)
    {
        var computersToTarget = ComputersInRegion.Select(x => x.Stats.FirstOrDefault(y => y.Identifier == stat));

        foreach (var target in computersToTarget)
        {
            if (GameManager.instance.WaitingForNextTurn)
            {
                StopAllCoroutines();
                break;
            }
            
            if (target != null)
            {
                target.Attack(attackForce);
                UpdateText();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    
    public void Defense(StatIdentifier stat, float defenseForce = .5f, int iteration = 0)
    {
        StartCoroutine(PerformDefense(stat, defenseForce));
        
        if (iteration == 0)
        {
            foreach (var nearbyNode in _nodesInProximity)
            {
                nearbyNode.Defense(stat, defenseForce * 0.25f, iteration + 1); // Divide the force by 4 in nearby nodes
            }
        }
    }

    private IEnumerator PerformDefense(StatIdentifier stat,float defenseForce)
    {
        var computersToTarget = ComputersInRegion.Select(x => x.Stats.FirstOrDefault(y => y.Identifier == stat));

        foreach (var target in computersToTarget)
        {
            if (GameManager.instance.WaitingForNextTurn)
            {
                StopAllCoroutines();
                break;
            }
            
            if (target != null)
            {
                target.Defend(defenseForce);
                UpdateText();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
