using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;

public class RegionInfo : InstancedBehavior<RegionInfo>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private GameObject _button;

    private bool CanOpen() => !_panel.activeSelf;
    public bool IsOpen() => _panel.activeSelf;
    
    private Node _node;

    public void Open(Node node)
    {
        _node = node;
            
        var nodeRect = node.gameObject.GetComponent<RectTransform>().rect;

        var nodePosition = node.transform.localPosition;
            
        var rect = gameObject.GetComponent<RectTransform>().rect;

        var infoPosition = new Vector3(nodePosition.x + nodeRect.width + (rect.width / 2), nodePosition.y, nodePosition.z);

        _panel.transform.localPosition = infoPosition;
            
        _panel.SetActive(true);
    }

    private void Update()
    {
        if (IsOpen())
        {
            _infoText.text = $"<size=24>{_node.name}</size>{Environment.NewLine}Breached Machines: {_node.ComputersInRegion.Count(x => x.Breached)}/{_node.ComputersInRegion.Count}{Environment.NewLine}";

            foreach (var stat in _node.Stats)
            {
                _infoText.text += stat.ToString() + Environment.NewLine;
            }
            
            _button.SetActive(GameManager.instance.WaitingForNextTurn);
        }
    }

    public void Close()
    {
        if (!GameManager.instance.WaitingForNextTurn)
        {
            _panel.SetActive(false);
        }
    }

    public void SelectPlayerNode()
    {
        AudioManager.instance.Play("Click");
        Player.instance.SetWorkingOnNode(_node);
    }
}
