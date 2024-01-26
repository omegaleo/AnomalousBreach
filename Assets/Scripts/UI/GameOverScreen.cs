using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;

public class GameOverScreen : InstancedBehavior<GameOverScreen>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _text;
    
    public void Open(bool victory)
    {
        _text.text = (victory) ? "VICTORY" : "DEFEAT";
        _text.text += $"{Environment.NewLine}<Press the Power Button to go back to the title screen>";
        _panel.SetActive(true);
    }
}
