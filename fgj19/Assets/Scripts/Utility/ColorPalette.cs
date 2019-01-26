using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Custom/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    [SerializeField] private List<Color> _Colors;
    //public List<Color> Colors { get { return _Colors; } }

    public Color this[int key]
    {
        get
        {
            if (_Colors == null || _Colors.Count == 0)
            {
                return Color.white;
            }
            return _Colors[Mathf.Clamp(key, 0, _Colors.Count - 1)];
        }
    }
}
