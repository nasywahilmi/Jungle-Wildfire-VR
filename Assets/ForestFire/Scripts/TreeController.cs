using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    //Variables creation
    GameObject myTree;
    public int treeHealth=3;
    private bool fallen = false;
    public AudioSource TreeFallenAudio;
    public SpriteRenderer minimapcell;

    // Start is called before the first frame update
    void Start()
    {
        myTree = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (treeHealth <=0 & fallen==false)
        {
            TreeFallenAudio.Play();
            GameObject parentCell = myTree.transform.parent.gameObject.transform.parent.gameObject; //Create reference to the ForestFireCell prefab this instance is childed to - so that I can access the current state of the tree (alight, alive etc..)
            ForestFireCell activeCell = parentCell.GetComponent<ForestFireCell>();
            activeCell.SetBurnt();
            minimapcell.sprite = null;

            Rigidbody treeRB = myTree.AddComponent<Rigidbody>();
            treeRB.isKinematic = false;
            treeRB.useGravity = true;

            StartCoroutine(removeTree(treeRB));
            fallen = true;

        }
    }

    //Create corutine
    private IEnumerator removeTree(Rigidbody treeRB)
    {
        yield return new WaitForSeconds(2);
        treeRB.AddForce(Vector3.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(3);
        //Destroy(myTree);
        myTree.SetActive(false);

    }




}
