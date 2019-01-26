using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GridConfig", menuName ="Custom/GridConfig")]
public class GridConfig : ScriptableObject
{
    [SerializeField] private int _MinPlacementHeight = 3;
    public int MinPlacementHeight { get { return _MinPlacementHeight; } }

    [SerializeField] private float _SegmentHeight = 0.2f;
    public float SegmentHeight { get { return _SegmentHeight; } }
}
