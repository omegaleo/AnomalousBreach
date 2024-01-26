using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : InstancedBehavior<GameManager>
{
    [ColoredHeader("Waves Related")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private float _secondsPerWave = 60f;
    public int MaxWaves = 10;
    
    [ColoredHeader("News")]
    [SerializeField] private List<string> _newsMessages = new List<string>();

    [SerializeField] private float _minutesBetweenMessages = 1.5f;
    
    [ColoredHeader("Colors")]
    [SerializeField] private Color _attackedColor;
    [SerializeField] private Color _defendedColor;
    
    [SerializeField] private Color _attackedColorAlt;
    [SerializeField] private Color _defendedColorAlt;

    public Color GetAttackedColor(bool alt = false) => (alt) ? _attackedColorAlt : _attackedColor;
    public Color GetDefendedColor(bool alt = false) => (alt) ? _defendedColorAlt : _defendedColor;

    public bool WaitingForNextTurn = true;
    public float SecondsLeft = 0f;
    public int CurrentTurn = 0;
    
    private void Start()
    {
        WaitingForNextTurn = true;

        StartCoroutine(HandleMessages());
    }

    public void NextTurn()
    {
        if (!WaitingForNextTurn) return;
        
        WaitingForNextTurn = false;
        
        CurrentTurn++;
        _enemyParent.DestroyChildren();

        var numberToSpawn = Random.Range(1, 3);

        var nodes = NodeManager.instance.GetNodes();
        
        for (int i = 0; i < numberToSpawn; i++)
        {
            var enemy = Instantiate(_enemyPrefab, _enemyParent);
            enemy.GetComponent<Enemy>().SetWorkingOnNode(nodes.Random().GetComponent<Node>());
        }

        StartCoroutine(CountdownToNextTurn());
    }

    private IEnumerator CountdownToNextTurn()
    {
        SecondsLeft = _secondsPerWave;

        while (SecondsLeft > 0f)
        {
            SecondsLeft -= 1f;
            yield return new WaitForSeconds(1f);
        }

        if (CurrentTurn < MaxWaves)
        {
            WaitingForNextTurn = true;
        }
        else
        {
            // Handle End of Game
        }
    }

    private IEnumerator HandleMessages()
    {
        yield return new WaitForSeconds(_minutesBetweenMessages * 60f);
        
        while (true)
        {
            if (NewsPanel.instance == null) yield return new WaitForSeconds(0.1f);

            var message = _newsMessages.Random();
            
            NewsPanel.instance.Show(message);

            while (NewsPanel.instance.showing)
            {
                yield return new WaitForSeconds(1f);
            }
            
            yield return new WaitForSeconds(_minutesBetweenMessages * 60f);
        }
    }
    
    #region Input Events

    public Action<InputAction.CallbackContext> OnMouseDrag;

    public void HandleMouseDrag(InputAction.CallbackContext value)
    {
        OnMouseDrag.Invoke(value);
    }
    
    #endregion
}
