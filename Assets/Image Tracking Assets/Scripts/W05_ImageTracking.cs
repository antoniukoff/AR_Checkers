using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class W05_ImageTracking : MonoBehaviour
{
    ARTrackedImageManager arTrackedImageManager;

    public GameObject prefabToSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>(); 
    }

    public void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trackedImage in args.added)
        {
            Debug.Log(trackedImage.name);

            if (trackedImage.name == "Logo_NHL")
            {
                //prefabToSpawn.gameObject.GetComponent<BoardScript>().SpawnTheBoard();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
