using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Test : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private GameObject spawnedObject;
    private GameObject boardMesh;
    // Start is called before the first frame update
    void Start()
    {
            spawnedObject = Instantiate(prefabToSpawn, new Vector3(5.0f,-3.0f,3.0f), Quaternion.identity);
            Debug.Log("spawnedObject.transform.position: " + spawnedObject.transform.position);
            boardMesh = spawnedObject.GetComponent<BoardScript>().SpawnTheBoard();
            boardMesh.transform.position = spawnedObject.transform.position;    
            boardMesh.transform.rotation = spawnedObject.transform.rotation;    
            boardMesh.transform.SetParent(spawnedObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t")){
            //spawnedObject.transform.position = new Vector3(5f,0.0f,0.0f);
            //spawnedObject = spawnedObject.GetComponent<BoardScript>().SpawnTheBoard();
        }  
        if(Input.GetKey("a") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.1f,0.0f,0.0f);

         if(Input.GetKey("d") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(-0.1f,0.0f,0.0f);
            
             if(Input.GetKey("w") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.0f,0.0f,0.1f);

            if(Input.GetKey("s") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.0f,0.0f,-0.1f);
        
        if(Input.GetKey("q") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.0f,0.1f,0.0f);

        if(Input.GetKey("e") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.0f,-0.1f,0.0f);

        if(Input.GetKey("left") && spawnedObject != null)
        spawnedObject.transform.Rotate(0.0f,0.1f,0.0f);
        
        if(Input.GetKey("right") && spawnedObject != null)
        spawnedObject.transform.Rotate(0.0f,-0.1f,0.0f);

        if(Input.GetKey("up") && spawnedObject != null)
        spawnedObject.transform.Rotate(0.1f,0.0f,0.0f);

        if(Input.GetKey("down") && spawnedObject != null)
        spawnedObject.transform.Rotate(-0.1f,0.0f,0.0f);

        if(Input.GetKey("z") && spawnedObject != null)
        spawnedObject.transform.localScale += new Vector3(0.1f,0.1f,0.1f);

        if(spawnedObject.transform.localScale.x > 0.1f)
        {if(Input.GetKey("x") && spawnedObject != null)
        spawnedObject.transform.localScale -= new Vector3(0.1f,0.1f,0.1f);}
        

    }
}
