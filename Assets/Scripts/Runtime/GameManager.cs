using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : InstancedBehavior<GameManager>
{
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
    public int CurrentTurn = 1;
    
    private void Start()
    {
        WaitingForNextTurn = true;

        StartCoroutine(HandleMessages());
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
