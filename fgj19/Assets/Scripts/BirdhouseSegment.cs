using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdhouseSegment : MonoBehaviour
{
    public int BaseColorIndex;
    [SerializeField] private ColorPalette BaseColorPalette;
    [SerializeField] private bool IsRoof;

    private Renderer Renderer;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if ( BaseColorPalette != null )
        {
            Gizmos.color = BaseColorPalette[BaseColorIndex];
            Gizmos.DrawCube(transform.position + Vector3.up * 0.3f + Vector3.back * 0.3f, Vector3.one * 0.65f);
        }
        else
        {
            UnityEditor.Handles.Label(transform.position, "" + BaseColorIndex);
        }
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponentInChildren<BirdhouseVisual>().GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Would be nice to do this setting during edit mode as well, but it will create unwanted material instances in the scene.
        if ( BaseColorPalette != null )
        {
            Renderer.material.color = BaseColorPalette[BaseColorIndex];
        }
    }

    public bool Matches(BirdhouseSegment other)
    {
        return BaseColorIndex == other.BaseColorIndex;
    }
}
