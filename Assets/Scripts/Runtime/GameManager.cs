using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
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
    private List<Enemy> _enemies = new List<Enemy>();
    
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

        var turnBoost = Mathf.RoundToInt(CurrentTurn * 0.25f);
        
        var nodes = NodeManager.instance.GetNodes();
        
        var numberToSpawn = Random.Range(1 + turnBoost, Mathf.Clamp(3 + turnBoost, 3, nodes.Count));
        
        _enemies.Clear();
        
        for (int i = 0; i < numberToSpawn; i++)
        {
            var enemy = Instantiate(_enemyPrefab, _enemyParent);

            var workingOnNode = nodes
                .Where(x => _enemies
                    .TrueForAll(y =>
                        y.GetWorkingOnNode().gameObject != x)
                )
                .ToList()
                .Random()!
                .GetComponent<Node>();

            if (workingOnNode != null)
            {
                enemy.GetComponent<Enemy>().SetWorkingOnNode(workingOnNode);
            }
            
            _enemies.Add(enemy.GetComponent<Enemy>());
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

        Player.instance.StopAllCoroutines();
        _enemies.ForEach(x => x.StopAllCoroutines());
        
        var nodes = NodeManager.instance.GetNodes().Select(x => x.GetComponent<Node>());

        if (nodes.Count(x => x.TakenOver) == nodes.Count())
        {
            GameOverScreen.instance.Open(Player.instance.Side == PlayerSide.Attack);
        }
        
        if (CurrentTurn < MaxWaves)
        {
            Player.instance.UpgradePoints += 2; // TODO: Change this
            AudioManager.instance.Play("Alert");
            WaitingForNextTurn = true;
        }
        else
        {
            GameOverScreen.instance.Open(true);
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
