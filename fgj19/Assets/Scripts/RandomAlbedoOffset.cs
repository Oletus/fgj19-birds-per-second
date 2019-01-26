using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAlbedoOffset : MonoBehaviour
{
    //public Texture main_Texture;
    Renderer main_Renderer;

    int random_Y_Tiling;
    int x_Tiling;

    int random_Y_Offset;
    int x_Offset;

    void Start()
    {
        main_Renderer = GetComponent<Renderer>();

        //Tiling
        x_Tiling = 1;
        random_Y_Tiling = Random.Range(16, 30);

        //Offset
        x_Offset = 0;
        random_Y_Offset = Random.Range(1, 1000);

        //Enable keywords
        //main_Renderer.material.EnableKeyword("_ALBEDO");

        //Setting texture or albedo
        //main_Renderer.material.SetTexture("_MainTex", main_Texture);

        main_Renderer.material.mainTextureOffset = new Vector2(x_Offset, random_Y_Offset);
        main_Renderer.material.mainTextureScale = new Vector2(x_Tiling, random_Y_Tiling);
    }
}
