﻿using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField]
    public List<GraphNode> Adjacent;
    public enum Attribute
    {
        Road,
        Residential,
        Commnerical,
        Industrial,
        Office,
        Millitary,
        Abstract,

    }
    public Attribute attribute;

    public int heuristicScore;
    public int distanceFromStart;

    private void OnDrawGizmos()
    {
        foreach (var node in Adjacent)
        {
            Debug.DrawLine(node.transform.position, transform.position, Color.red);
        }
    }
}
