using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AxeController : MonoBehaviour
{
    //Variable creation --------------------------------------------------------------------------------------------------

        public AudioSource TreeChopSound1;
        public AudioSource RockHitSound;
        GameObject Axe;
        bool canHit = true;
        bool shownTooltip = false;
        bool axeStart = false;
        public GameObject AxeMinimap;

    //--------------------------------------------------------------------------------------------------------------------


    private void Start()
    {
        Axe = this.gameObject;
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(StartAxe);
        grabInteractable.onSelectExit.AddListener(ShowAxeIcon);
        grabInteractable.onFirstHoverEnter.AddListener(StartTooltip);
    }


    //Function to create a popup tooltip on first hover - instructing the user
    public void StartTooltip(XRBaseInteractor interactor)
    {
        if (shownTooltip == false)
        {
            GameObject tooltip = Axe.transform.Find("Tooltip").gameObject;
            tooltip.SetActive(true);
            shownTooltip = true;
        }

    }



    //Function to enable to gravity on the rigid body, but only once the first grab has occured - allows axe to stay stuck into tree at the start of the scene
    public void StartAxe(XRBaseInteractor interactor)
    {
        AxeMinimap.SetActive(false); //Hide minimap icon for axe on grab

        if (axeStart == false) //Check if this is the first time picking up the axe
        {
            Rigidbody axeRB = Axe.GetComponent<Rigidbody>();
            axeRB.useGravity = true;
            axeRB.isKinematic = false;
            GameObject tooltip = Axe.transform.Find("Tooltip").gameObject;
            tooltip.SetActive(false);
        }
    }


    //Function to enable to axe minimap icon
    public void ShowAxeIcon(XRBaseInteractor interactor)
    {
        AxeMinimap.SetActive(true);
    }


        //Method to call on collision of Axe
        private void OnCollisionEnter(Collision collision)
        {
        //Wrap in a try-catch incase the axe hits more than 3 times, and occurs faster than the script processes // stops the null exception gameobject error
        try
        {
            GameObject parentCell = collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject; //Create reference to the ForestFireCell prefab this instance is childed to - so that I can access the current state of the tree (alight, alive etc..)
            ForestFireCell activeCell = parentCell.GetComponent<ForestFireCell>();

            if ((collision.gameObject.tag == "tree") & (canHit==true) & (activeCell.cellState == ForestFireCell.State.Tree))
            {
                canHit = false;
                TreeChopSound1.Play(); //Play the sound provided
                GameObject parentTree = collision.gameObject.transform.parent.gameObject; //Create a gameobject variable to store the parent of the gameobject it collided with - where treecontroller is located
                TreeController activeTree = parentTree.GetComponent<TreeController>(); //Find Tree controller component, to allow access to variables
                activeTree.treeHealth--; //Decrement tree health by 1
            }

            if ((collision.gameObject.tag == "rock") & (canHit==true))
            {
                canHit = false;
                RockHitSound.Play(); //Play the sound provided
            }

        }
        catch (System.Exception)
        {
        }
    }

    //Detemrine when axe leaves the tree collider, allow hitbox detection to turn back on
    private void OnCollisionExit(Collision collision)
    {

        try
        {
            GameObject parentCell = collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject; //Create reference to the ForestFireCell prefab this instance is childed to - so that I can access the current state of the tree (alight, alive etc..)
            ForestFireCell activeCell = parentCell.GetComponent<ForestFireCell>();
            if ((collision.gameObject.tag == "tree") & (activeCell.cellState == ForestFireCell.State.Tree))
            {
                StartCoroutine(hitDetectionState());
            }
            if (collision.gameObject.tag == "rock")
            { 
                StartCoroutine(hitDetectionState());
            }
        }
        catch (System.Exception)
        {
        }
    }


    //Create corutine to delay time before hit detection is enabled
    private IEnumerator hitDetectionState()
    {
        yield return new WaitForSeconds(0.3f);
        canHit = true;
    }


}
