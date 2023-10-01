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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t")){
            spawnedObject = Instantiate(prefabToSpawn, new Vector3(), Quaternion.identity);
            spawnedObject = spawnedObject.GetComponent<BoardScript>().SpawnTheBoard();
        }  
        if(Input.GetKeyDown("a") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(0.1f,0.0f,0.0f);

         if(Input.GetKeyDown("d") && spawnedObject != null)
        spawnedObject.transform.position -= new Vector3(-0.1f,0.0f,0.0f);

        

    }
}
