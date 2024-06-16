using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhancedMinimapGUI : MonoBehaviour
{
    public Camera MinimapCamera; //ref to the minimap camera object
    public Slider slider; //ref to the gui slider on the minimap


    // Update is called once per frame
    void Update()
    {
        if (MinimapCamera.orthographicSize != slider.value)
        {
            MinimapCamera.orthographicSize = slider.value;
        }
    }
}
