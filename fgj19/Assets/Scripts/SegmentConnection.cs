using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentConnection : MonoBehaviour
{
    [System.NonSerialized] public int GridY;
    [System.NonSerialized] public int GridXLeft;
    [System.NonSerialized] public int GridXRight;

    [System.NonSerialized] public BirdhouseSegment LeftEnd;
    [System.NonSerialized] public BirdhouseSegment RightEnd;
}
