using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;

public class TreePositionSelect : MonoBehaviour
{
    public int MaxPlacementHeight = 10;  // Inclusive.

    public GridConfig GridConfig;

    public Birdhouse PlacedObject;

    private Pointer Pointer;

    private void Awake()
    {
        Pointer = new Pointer();
    }

    // Returns true if preview object should be visible.
    private bool TryPlaceAtCursor()
    {
        RaycastHit hitInfo = new RaycastHit();
        if ( !Physics.Raycast(Pointer.GetRay(Camera.main), out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("PlacementCollider")) )
        {
            return false;
        }

        Tree tree = hitInfo.collider.GetComponentInParent<Tree>();
        if ( !tree )
        {
            return false;
        }
        float cursorWorldYOffset = hitInfo.point.y - tree.transform.position.y;
        int cursorGridHeight = Mathf.RoundToInt(cursorWorldYOffset / GridConfig.SegmentHeight);

        int placementGridY = Mathf.Max(GridConfig.MinPlacementHeight, Mathf.Min(MaxPlacementHeight, cursorGridHeight));

        bool onRightSide = hitInfo.point.x > tree.transform.position.x;

        if ( tree.CanBeAttached(PlacedObject.SegmentCount, placementGridY, onRightSide) )
        {
            if ( Input.GetMouseButtonDown(0) )
            {
                PlacedObject.gameObject.SetActive(true);
                tree.AttachBirdhouse(PlacedObject, placementGridY, onRightSide);                
                PlacedObject = null;
                return false;
            }
            else
            {
                tree.PreviewBirdhouse(PlacedObject, placementGridY, onRightSide);
            }
            return true;
        }
        return false;
    }

    private void Update()
    {
        if ( PlacedObject == null )
        {
            return;
        }

        bool previewActive = TryPlaceAtCursor();
        if ( PlacedObject != null )
        {
            PlacedObject.gameObject.SetActive(previewActive);
        }
    }
}
