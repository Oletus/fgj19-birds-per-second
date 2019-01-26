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

    private void Update()
    {
        if ( PlacedObject == null )
        {
            return;
        }

        RaycastHit hitInfo = new RaycastHit();
        if (!Physics.Raycast(Pointer.GetRay(Camera.main), out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("PlacementCollider")))
        {
            PlacedObject.gameObject.SetActive(false);
            return;
        }
        PlacedObject.gameObject.SetActive(true);

        Tree tree = hitInfo.collider.GetComponentInParent<Tree>();
        if ( !tree )
        {
            return;
        }
        float cursorWorldYOffset = hitInfo.point.y - tree.transform.position.y;
        int cursorGridHeight = Mathf.RoundToInt(cursorWorldYOffset / GridConfig.SegmentHeight);

        int placementHeight = Mathf.Max(GridConfig.MinPlacementHeight, Mathf.Min(MaxPlacementHeight, cursorGridHeight));

        bool onRightSide = hitInfo.point.x > tree.transform.position.x;

        if ( Input.GetMouseButtonDown(0) )
        {
            tree.AttachBirdhouse(PlacedObject, placementHeight, onRightSide);
            // TODO: Switch object from preview to placed mode.
            PlacedObject = null;
        }
        else
        {
            tree.PreviewBirdhouse(PlacedObject, placementHeight, onRightSide);
        }
    }
}
