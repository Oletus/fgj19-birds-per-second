using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BirdhouseSegment : MonoBehaviour
{
    [SerializeField] private int _BaseColorIndex;
    public int BaseColorIndex { get { return _BaseColorIndex; } }
    [SerializeField] private ColorPalette BaseColorPalette;

    private Renderer Renderer;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Renderer.material.color = BaseColorPalette[BaseColorIndex];

        if ( !Application.isPlaying )
        {
            return;
        }
    }

    bool Matches(BirdhouseSegment other)
    {
        return _BaseColorIndex == other._BaseColorIndex;
    }
}
