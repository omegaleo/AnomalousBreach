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

    private bool CanOpen() => !_panel.activeSelf;

    public void Open(Node node)
    {
        if (CanOpen())
        {
            var nodeRect = node.gameObject.GetComponent<RectTransform>().rect;

            var nodePosition = node.transform.localPosition;
            
            var rect = gameObject.GetComponent<RectTransform>().rect;

            var infoPosition = new Vector3(nodePosition.x + nodeRect.width + (rect.width / 2), nodePosition.y, nodePosition.z);

            _panel.transform.localPosition = infoPosition;

            _infoText.text = $"<size=24>{node.name}</size>{Environment.NewLine}Breached Machines: {node.ComputersInRegion.Count(x => x.Breached)}/{node.ComputersInRegion.Count}{Environment.NewLine}";

            foreach (var stat in node.Stats)
            {
                _infoText.text += stat.ToString() + Environment.NewLine;
            }
            
            _panel.SetActive(true);
        }
    }

    public void Close()
    {
        _panel.SetActive(false);
    }
}
