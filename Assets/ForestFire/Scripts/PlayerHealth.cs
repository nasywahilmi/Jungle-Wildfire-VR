using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar; //ref to the healthbar ui element
    public Image DamageOverlay; //ref to the damage overlay image 

    public int MaxHealth=100, CurrentHealth; //Create health varaibles for the player, default values
    public AudioSource DamageSound, DeathSound; //Create reference to audiosource
    bool CanTakeDamage = true; //Variable to enable damage control


    //Function to reduce a players health by a given value, and run any effects that relate to that
    void TakeDamage(int damagevalue)
    {
        if (CurrentHealth!=0)
        {
            CurrentHealth = CurrentHealth - damagevalue; //Reduce player health
            DamageSound.Play(); //Play damage sound, alerting the player
            healthBar.SetHealth(CurrentHealth);
        }
    }

    //Function to check if a players health is zero - player has therefore died (call each frame)
    void CheckDeath(int health)
    {
        if (health==0)
        {
            DeathSound.Play(); //Play death sound
            //Move player to the end grave scene
            SceneManager.LoadScene("Death");
        }
    }

    //Function to check the distance of a player to a fire, and return true or false if is close
    bool IsPlayerNearFire()
    {
        Vector3 playerPosition = this.gameObject.transform.position; //Get players current position

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, 4); //Create a physics sphere around the player, to check which objects are nearby, returns array of colliders found

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "tree") //validate if tree is found in any state, before attempting to reach forest fire script
            {
                GameObject tree = hitCollider.gameObject.transform.parent.gameObject; //reference to tree parent
                GameObject parentCell = hitCollider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject; //Create reference to the ForestFireCell prefab this instance is childed to - so that I can access the current state of the tree (alight, alive etc..)
                ForestFireCell activeCell = parentCell.GetComponent<ForestFireCell>();
                if (activeCell.cellState == ForestFireCell.State.Alight & tree.activeSelf) //Check status of the tree - look for fire,
                {
                    return true; //Player is close enough to fire
                }

            //Specific check for grass objects as they have no tag or visual cue.
            else if (hitCollider.gameObject.name == "ForestFireCell")
                {
                    if(hitCollider.gameObject.GetComponent<ForestFireCell>().currentFire != null)
                    {
                        return true; //Player is close enough to fire
                    } 
                }
            }
        }
        return false; //Player is too far
    }


    //coroutine Function to deal damage to player if next to the a fire for longer than specified time
    private IEnumerator FireDamage()
    {
        DamageOverlay.enabled = true;
        CanTakeDamage = false; //Stop the update function from being called, whilst evaluating players next status
        yield return new WaitForSeconds(1); //wait 1 second before continuing
    
        if (IsPlayerNearFire() == true) //Check if still next to fire, if so take damage
        {
            TakeDamage(10);
        }
        DamageOverlay.enabled = false; //Disable orange overlay
        CanTakeDamage = true; //Allow update function to continue checking each frame
    }




    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth; //Set current player health to full - start of game
        healthBar.SetMaxHealth(MaxHealth); //update UI
    }


    // Update is called once per frame
    void Update()
    {
        CheckDeath(CurrentHealth); //Check for death

        //If player can take damage and is near fire, run coroutine to determine if they stand next to fire for enough time to take damage
        if ((CanTakeDamage == true) & (IsPlayerNearFire() == true))
        {
            StartCoroutine(FireDamage());
        }

    }
}
