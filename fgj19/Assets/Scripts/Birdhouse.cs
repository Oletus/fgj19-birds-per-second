using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Birdhouse : MonoBehaviour
{
    [SerializeField] private GridConfig Config;

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
        if ( !Application.isPlaying )
        {
            Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
        }

        int i = 0;
        foreach (BirdhouseSegment segment in Segments)
        {
            segment.transform.localPosition = Vector3.zero + Vector3.up * i * Config.SegmentHeight;
            segment.GetComponent<SpriteRenderer>().sortingOrder = i;
            ++i;
        }

        if ( !Application.isPlaying )
        {
            return;
        }

        // Gameplay logic here.
    }

    public void OnAttached()
    {

    }
}
