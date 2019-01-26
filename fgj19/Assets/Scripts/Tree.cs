using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int _Height;

    public float GetWidthAtHeight(int height)
    {
        return 1.0f;
    }
}
