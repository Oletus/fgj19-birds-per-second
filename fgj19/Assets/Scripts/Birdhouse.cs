using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(AudioSource))]
public class Birdhouse : MonoBehaviour
{
    [SerializeField] private GridConfig Config;

    public enum State
    {
        Detached,
        PickedUp,
        AttachedToTree
    }

    private List<BirdhouseSegment> Segments;
    public int SegmentCount { get { return (Segments != null) ? Segments.Count : 0; } }
    private AudioSource AudioSource;

    private int PlacementGridY;
    private bool OnRightSide;

    // Start is called before the first frame update
    void Awake()
    {
        Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
        AudioSource = GetComponent<AudioSource>();
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
            if ( segment.GetComponent<SpriteRenderer>() )
            {
                segment.GetComponent<SpriteRenderer>().sortingOrder = i;
            }
            ++i;
        }

        if ( !Application.isPlaying )
        {
            return;
        }

        // Gameplay logic here.
    }

    public bool Intersects(int otherSegmentCount, int otherGridY, bool otherOnRightSide)
    {
        if ( otherOnRightSide != OnRightSide )
        {
            return false;
        }
        return (otherGridY + otherSegmentCount > PlacementGridY && PlacementGridY + SegmentCount > otherGridY);
    }

    public void SetAttachedTransform(Tree tree, int placementGridY, bool onRightSide, bool preview)
    {
        PlacementGridY = placementGridY;
        OnRightSide = onRightSide;
        transform.position = tree.GetAttachmentPosition(placementGridY, onRightSide, preview);
        transform.rotation = tree.GetAttachmentRotation(onRightSide);
    }

    public void OnAttached()
    {
        BeatSynchronizer.Instance.PlayOnNextBeat(AudioSource, true);
    }
}
