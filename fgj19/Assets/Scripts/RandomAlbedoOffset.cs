using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAlbedoOffset : MonoBehaviour
{
    void Awake()
    {
        RandomizeTiling();
    }

    void RandomizeTiling()
    {
        Renderer renderer = GetComponent<Renderer>();

        //Tiling
        int x_Tiling = 1;
        float random_Y_Tiling = Random.Range(0.6f, 1.0f) * transform.localScale.y;

        //Offset
        int x_Offset = 0;
        int random_Y_Offset = Random.Range(1, 1000);

        renderer.material.mainTextureOffset = new Vector2(x_Offset, random_Y_Offset);
        renderer.material.mainTextureScale = new Vector2(x_Tiling, random_Y_Tiling);
    }
}
