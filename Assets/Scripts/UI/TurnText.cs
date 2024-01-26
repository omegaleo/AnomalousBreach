using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnText : MonoBehaviour
{
    private TMP_Text _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.WaitingForNextTurn)
        {
            _text.text = $"Waiting for next wave.{Environment.NewLine}Please select a node in order to continue.";
        }
        else
        {
            _text.text = $"Wave {GameManager.instance.CurrentTurn} / {GameManager.instance.MaxWaves}{Environment.NewLine}Time Left: {GameManager.instance.SecondsLeft}";
        }

        _text.text += $"{Environment.NewLine}Upgrade Points Available: {Player.instance.UpgradePoints}";
    }
}
