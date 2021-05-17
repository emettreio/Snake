using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {
    public List<Color> colorOptions;

    public Color GetRandomColor()
    {
        int colorPicker = Random.Range(0, colorOptions.Count);
        return colorOptions[colorPicker];

    }
}
