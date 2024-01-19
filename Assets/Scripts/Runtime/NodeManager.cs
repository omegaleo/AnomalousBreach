using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class NodeManager : InstancedBehavior<NodeManager>
{
    [SerializeField] private List<GameObject> _nodes = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.HasComponent<Node>())
            {
                _nodes.Add(child.gameObject);
            }
        }
        
        _nodes.ForEach(x => x.GetComponent<Node>().Initialize());
    }

    public List<Vector3> GetNodePositions => _nodes.Select(x => x.transform.localPosition).ToList();
    
    public GameObject GetNode(Vector3 position) => _nodes.FirstOrDefault(x => Vector3.Distance(x.transform.localPosition, position) < 2f) ?? null;
}
