using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class GameManager : InstancedBehavior<GameManager>
{
    [ColoredHeader("Colors")]
    [SerializeField] private Color _attackedColor;
    [SerializeField] private Color _defendedColor;
    
    [SerializeField] private Color _attackedColorAlt;
    [SerializeField] private Color _defendedColorAlt;

    public Color GetAttackedColor(bool alt = false) => (alt) ? _attackedColorAlt : _attackedColor;
    public Color GetDefendedColor(bool alt = false) => (alt) ? _defendedColorAlt : _defendedColor;
}
