using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [ColoredHeader("Configurations")]
    [SerializeField] private Image _deffendedTexture;
    [SerializeField] private Image _attackedTexture;
    [SerializeField] private TMP_Text _attackedPercentageText;
    [SerializeField] private List<Node> _nodesInProximity = new List<Node>();
    
    private float _attackedAmount = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _deffendedTexture.color = GameManager.instance.GetDefendedColor();
        _attackedTexture.color = GameManager.instance.GetAttackedColor();
        
        SetNodesInProximity();
    }

    private void SetNodesInProximity()
    {
        // We need to check the positions in all directions
        var currentPosition = transform.localPosition;
        var rectTransform = GetComponent<RectTransform>();
        var height = rectTransform.rect.height;
        var width = rectTransform.rect.width;

        var positionsToCheck = new[]
        {
            new Vector3(currentPosition.x - width, currentPosition.y + height, currentPosition.z), // Top Left
            new Vector3(currentPosition.x, currentPosition.y + height, currentPosition.z), // Top
            new Vector3(currentPosition.x + width, currentPosition.y + height, currentPosition.z), // Top Right
            
            new Vector3(currentPosition.x - width, currentPosition.y, currentPosition.z), // Left
            new Vector3(currentPosition.x + width, currentPosition.y, currentPosition.z), // Right
            
            new Vector3(currentPosition.x - width, currentPosition.y - height, currentPosition.z), // Bottom Left
            new Vector3(currentPosition.x, currentPosition.y - height, currentPosition.z), // Bottom
            new Vector3(currentPosition.x + width, currentPosition.y - height, currentPosition.z), // Bottom Right
        };

        foreach (var pos in positionsToCheck)
        {
            var node = NodeManager.instance.GetNode(pos);

            if (node != null)
            {
                _nodesInProximity.Add(node.GetComponent<Node>());
            }
        }
    }

    public void IncreaseAttack()
    {
        _attackedAmount += 0.25f;

        if (_attackedAmount > 1f) _attackedAmount = 0f; // temporary for demo purposes

        _attackedTexture.fillAmount = _attackedAmount;
        _attackedPercentageText.text = $"{(_attackedAmount * 100f).RoundToInt()}%";
    }
}
