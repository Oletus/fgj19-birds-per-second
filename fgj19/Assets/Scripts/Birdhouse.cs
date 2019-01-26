using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdhouse : MonoBehaviour
{
    public enum State
    {
        Detached,
        PickedUp,
        AttachedToTree
    }

    public bool OnLeftSideOfTree { set; get; }

    private List<BirdhouseSegment> Segments;

    // Start is called before the first frame update
    void Awake()
    {
        Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
