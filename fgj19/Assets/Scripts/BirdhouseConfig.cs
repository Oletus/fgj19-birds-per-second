using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BirdhouseConfig", menuName ="Custom/BirdhouseConfig")]
public class BirdhouseConfig : ScriptableObject
{
    [SerializeField] private float _SegmentHeight = 0.2f;
    public float SegmentHeight { get { return _SegmentHeight; } }
}
