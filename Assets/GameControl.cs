using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public Slider slider; //reference to game speed slider
    public ForestFire3D forestFire3D; //reference to the base controlling script
    

    public void StartGame()
    {

        forestFire3D.updateRate = slider.value; //set update rate that player chose
        forestFire3D.PauseGame(false); //Unpause - Start the game

        //Make the GameStart Plynth disappear
        this.gameObject.SetActive(false);

    }




}
