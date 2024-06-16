using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider; //reference to slider canvas component
    public Gradient gradient; //refernece to create a gradient
    public Image fill; //reference to the image fill


    public void SetMaxHealth(int health) //Method to set max health and slider to the same values
    {
        slider.maxValue = health; //set max value of slider
        slider.value = health; //set current value to the max
        fill.color = gradient.Evaluate(1f); //set colour to the furthers right

    }

    public void SetHealth(int health)
    {
        slider.value = health; 
        fill.color = gradient.Evaluate(slider.normalizedValue); //set health bar to the color that relattes to amount of health left
    }


}
