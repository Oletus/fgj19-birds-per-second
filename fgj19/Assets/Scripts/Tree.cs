using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int _Height;

    private List<Birdhouse> Birdhouses;

    public bool HasBirdHouse { get { return Birdhouses.Count != 0; } }

    private void Awake()
    {
        Birdhouses = new List<Birdhouse>();
    }

    private float GetWidthAtHeight(int height)
    {
        return 1.0f;
    }

    private void SetAttachedObjectTransform(Birdhouse house, int placementHeight, bool onRightSide)
    {
        float horizontalOffset = GetWidthAtHeight(placementHeight) * 0.5f - 0.05f;
        if ( !onRightSide )
        {
            horizontalOffset = -horizontalOffset;
        }

        house.transform.position = transform.position + Vector3.up * GlobalConfig.Instance.GridConfig.SegmentHeight * placementHeight + Vector3.right * horizontalOffset + Vector3.back * 0.6f;
    }

    public void PreviewBirdhouse(Birdhouse house, int placementHeight, bool onRightSide)
    {
        SetAttachedObjectTransform(house, placementHeight, onRightSide);
    }

    public void AttachBirdhouse(Birdhouse house, int placementHeight, bool onRightSide)
    {
        SetAttachedObjectTransform(house, placementHeight, onRightSide);
        Birdhouses.Add(house);
        house.OnAttached();
    }
}
