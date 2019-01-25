using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdbox : MonoBehaviour
{
    public enum State
    {
        Detached,
        PickedUp,
        AttachedToTree
    }

    public bool OnLeftSideOfTree { set; get; }

    private List<BirdboxSegment> Segments;

    // Start is called before the first frame update
    void Awake()
    {
        Segments = new List<BirdboxSegment>(GetComponentsInChildren<BirdboxSegment>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
