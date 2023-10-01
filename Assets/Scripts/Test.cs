using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private GameObject spawnedObject;
    private GameObject boardMesh;
    // Start is called before the first frame update
    void Start()
    {
            //spawnedObject = Instantiate(prefabToSpawn, new Vector3(5.0f,0.0f,0.0f), Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t")){
            //spawnedObject.transform.position = new Vector3(5f,0.0f,0.0f);
            //spawnedObject = spawnedObject.GetComponent<BoardScript>().SpawnTheBoard();
        }  
        if(Input.GetKeyDown("a") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.1f,0.0f,0.0f);

         if(Input.GetKeyDown("d") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(-0.1f,0.0f,0.0f);

        

    }
}
