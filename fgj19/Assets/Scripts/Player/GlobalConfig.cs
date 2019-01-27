using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    public static GlobalConfig Instance { get; private set; }

    [SerializeField] private GridConfig _GridConfig;
    public GridConfig GridConfig { get { return _GridConfig; } }

    [SerializeField] private AudioClipPicker _BirdhouseAudioClipPicker;
    public AudioClipPicker BirdhouseAudioClipPicker { get { return _BirdhouseAudioClipPicker; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Two GlobalConfig objects detected!");
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
