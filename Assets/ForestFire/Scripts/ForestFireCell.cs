using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// class that reperesents a single cell in the cellular automaton
// contains data about the cell and methods to change its visual appearance
public class ForestFireCell : MonoBehaviour
{
    public SpriteRenderer cellMinimapSprite; //reference to the spriterenderer for this cell
    public Sprite TreeSprite, FireSprite, RockSprite, DeadSprite; //Reference to all minimap sprites

    public int numberOfAliveNeighbours; // integer to store the number of alive neighbour cells
    public State cellState; // this variable stores the state of the cell as an enum defined below 
    public enum State
    {
        Tree,
        Grass,
        Alight,
        Rock,
        Burnt,
    }

    public int cellFuel; // integer to store the amount of fuel in the cell

    // references to materials used for visual appearance
    public Material groundMaterialBurnt;
    public Material groundMaterialGrass;
    public Material groundMaterialRock;
    public Material groundMaterialTree;
    private MeshRenderer groundMeshRenderer; // reference to this cell's mesh renderer, used when changing material

    public GameObject treeObject; // reference to tree visual object
    public GameObject leaves; // reference to leaves visual object
    public GameObject rockObject; // reference to rock visual object
    public GameObject axeObject; //Reference to axe object

    public GameObject treeFireFVX; // reference to tree fire vfx
    public GameObject grassFireFVX; // reference to grass fire vfx

    public GameObject currentFire; // if the cell is on fire, the reference to the fire vfx is stored here
    public GameObject playerCamera; // reference to player camera
    public float fireVFXDistance; // float to set the maximum distance a fire vfx will be rendered at. this is used to improve rendering performance. 

    private VisualEffect _fireVisualEffect; // reference to the fire vfx on the current fire object.

    // Awake is a built-in Unity function that is only called once, before the Start function
    private void Awake()
    {
        groundMeshRenderer = GetComponent<MeshRenderer>(); // get reference to this cell's mesh renderer
    }

    // Update is a built-in Unity function that is called once per frame 
    private void Update()
    {
        // check whether current fire variable has been assigned 
        if (currentFire != null)
        {
            // enable vfx effect if player is within specified distance
            if (Vector3.Distance(transform.position, playerCamera.transform.position) > fireVFXDistance) 
            {
                if (_fireVisualEffect.enabled == true)
                    _fireVisualEffect.enabled = false;
            }
            else
            {
                if (_fireVisualEffect.enabled == false)
                    _fireVisualEffect.enabled = true;
            }
        }
    }

    // change cell state to tree
    public void SetTree()
    {
        cellState = State.Tree;
        groundMeshRenderer.material = groundMaterialTree;
        cellMinimapSprite.sprite = TreeSprite;
        treeObject.SetActive(true);

        if (currentFire != null)
        {
            Destroy(currentFire);
        }
    }

    // change cell state to grass    
    public void SetGrass()
    {
        cellMinimapSprite.sprite = null;
        cellState = State.Grass;
        groundMeshRenderer.material = groundMaterialGrass;
        treeObject.SetActive(false);
        rockObject.SetActive(false);

        if (currentFire != null)
        {
            Destroy(currentFire);
        }
    }

    // change cell state to rock
    public void SetRock()
    {
        if (cellState != State.Rock)
        {
            cellState = State.Rock;
            cellFuel = 0;
            groundMeshRenderer.material = groundMaterialRock;
            cellMinimapSprite.sprite = RockSprite;
            rockObject.SetActive(true);
        }
    }

    // set cell alight
    public void SetAlight()
    {
        cellState = State.Alight;
        cellMinimapSprite.sprite = FireSprite;

        if (currentFire == null)
        {
            // check whether tree object is active on this cell. if so assign the fire vfx as the current fire object
            if (treeObject.activeInHierarchy)
            {
                currentFire = Instantiate(treeFireFVX);
                currentFire.transform.SetParent(gameObject.transform, true);
                currentFire.transform.localPosition = Vector3.zero;
            }
            else // if the tree is not active, assign the grass vfx as the current fire object
            {
                currentFire = Instantiate(grassFireFVX);
                currentFire.transform.SetParent(gameObject.transform, true);
                currentFire.transform.localPosition = Vector3.zero;
            }
            // get a reference to the vfx component on the current fire object
            _fireVisualEffect = currentFire.GetComponent<VisualEffect>();
        }
    }

    // set cell burnt
    public void SetBurnt()
    {
        // if the cell has a fire, destroy it
        if (currentFire != null)
        {
            Destroy(currentFire);
        }

        // if there are leaves active in the hierarchy of this cell, disable them as if they have been burnt 
        if (treeObject.activeInHierarchy)
        {
            cellMinimapSprite.sprite = DeadSprite; //set sprite
            leaves.SetActive(false);
            
        }

        //If the game object is not a rock, and the sprite is not blank (It must be a burnt piece of grass - so remove the flame sprite)
        else if (rockObject.activeInHierarchy == false & cellMinimapSprite.sprite != null)
        {
            cellMinimapSprite.sprite = null;
        }
            

        cellState = State.Burnt;
        groundMeshRenderer.material = groundMaterialBurnt;
        
    }
}