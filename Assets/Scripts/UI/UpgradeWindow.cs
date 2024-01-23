using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    public bool IsOpen() => _panel.activeSelf;

    public void Open()
    {
        _panel.SetActive(true);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }
}
