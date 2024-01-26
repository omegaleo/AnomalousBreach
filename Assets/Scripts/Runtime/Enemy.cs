using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Models;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _image;
    
    public PlayerSide Side;

    [SerializeField] private Node _workingOn; // The node our enemy is currently protecting/attacking

    // For now this will be used for both attacking and defending
    [SerializeField] private List<VectorProfficiency> _profficiencies = new List<VectorProfficiency>();
    
    private void Start()
    {
        var identifiers = Enum.GetValues(typeof(StatIdentifier));
        Side = (Player.instance.Side == PlayerSide.Attack) ? PlayerSide.Defend : PlayerSide.Attack;

        var turnBoost = Mathf.RoundToInt(GameManager.instance.CurrentTurn * 0.25f);
        
        foreach (var identifier in identifiers)
        {
            var level = UnityEngine.Random.Range(2, 2 + turnBoost);
            _profficiencies.Add(new VectorProfficiency((StatIdentifier)identifier, level));
        }

        _image.GetComponent<Image>().color = (Side == PlayerSide.Attack) ? Color.red : Color.blue;
    }
    
    public void SetWorkingOnNode(Node node)
    {
        _workingOn = node;
        
        _image.SetActive(true);
        
        var workingOnRect = _workingOn.GetComponent<RectTransform>().rect;

        var offset = new Vector3(workingOnRect.width / 2 - 25f, workingOnRect.height / 2 - 25f, 0f);
        
        _image.transform.localPosition = _workingOn.transform.localPosition + offset;
        
        StartCoroutine(HandleEffect());
    }

    private IEnumerator HandleEffect()
    {
        while (!GameManager.instance.WaitingForNextTurn)
        {
            var profficientStats = _profficiencies.Where(x => x.Level > 0);
            
            switch (Side)
            {
                case PlayerSide.Attack:
                    foreach (var profficiency in profficientStats)
                    {
                        _workingOn.Attack(profficiency.Identifier, profficiency.Level * 0.125f);
                    }
                    break;
                case PlayerSide.Defend:
                    foreach (var profficiency in profficientStats)
                    {
                        _workingOn.Defense(profficiency.Identifier, profficiency.Level * 0.125f);
                    }
                    break;
            }
            
            yield return new WaitForSeconds(10f); // Wait 10 seconds before applying the attack/defense again
        }
    }

    public Node GetWorkingOnNode() => _workingOn;
}
