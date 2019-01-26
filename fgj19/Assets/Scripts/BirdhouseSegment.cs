using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Would be nice to do this setting during edit mode as well, but it will create unwanted material instances in the scene.
        Renderer.material.color = BaseColorPalette[BaseColorIndex];
    }

    bool Matches(BirdhouseSegment other)
    {
        return _BaseColorIndex == other._BaseColorIndex;
    }
}
