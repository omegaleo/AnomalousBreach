using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _title;
    
    public bool IsOpen() => _panel.activeSelf;

    public void Open()
    {
        _panel.SetActive(true);
    }

    private void Update()
    {
        _title.text = $"Upgrades ({Player.instance.UpgradePoints} points available)";
    }

    public void Close()
    {
        _panel.SetActive(false);
    }
}
