using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedMinimapController : MonoBehaviour
{
    public Transform player;
    public Transform playerMinimapIcon;

    private void LateUpdate()
    {
        //Move minimap based on player position
        Vector3 newPoisition = player.position;
        newPoisition.y = transform.position.y;
        transform.position = newPoisition;

        //rotate the player sprite based on player rotation
        playerMinimapIcon.rotation = Quaternion.Euler(90f, 180+player.eulerAngles.y, 0f);
    }
}
