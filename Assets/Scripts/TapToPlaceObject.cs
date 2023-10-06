using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class TapToPlaceObject : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float scaleSpeed = 0.01f; // Speed for scaling the object

    GameObject spawnedObject;
    GameObject boardMesh;

    ARRaycastManager arRaycastManager;
    Vector2 touchPosition;
    private float initialDistance;
    private Vector3 initialScale;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    bool moveBoard = false;

    
    private Vector2 initialTouchAvgPosition; 
    private Vector3 initialObjectPosition; 
    

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private bool GetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

    private void Update()
    {
        // Two-finger scaling
        if (spawnedObject && Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = spawnedObject.transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float scaleFactor = currentDistance / initialDistance;
                spawnedObject.transform.localScale = initialScale * scaleFactor;
            }
            return;
        }

        // Three-finger movement
        if (spawnedObject && Input.touchCount == 3)
        {
            moveBoard = !moveBoard;
            return;
        }

        // Placement
        if (!GetTouchPosition(out touchPosition))
            return;

        // Two-finger scaling
        if (spawnedObject && Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = spawnedObject.transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float scaleFactor = currentDistance / initialDistance;
                spawnedObject.transform.localScale = initialScale * scaleFactor;
            }
            return;
        }
        // Three-finger movement
        if (spawnedObject && Input.touchCount == 3)
        {
           moveBoard = !moveBoard;
        }
        // Touch event occured
        //https://docs.unity3d.com/2019.2/Documentation/ScriptReference/Experimental.XR.TrackableType.html
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(prefabToSpawn, hitPose.position, hitPose.rotation);
                Debug.Log("spawnedObject.transform.position: " + spawnedObject.transform.position);
                boardMesh = spawnedObject.GetComponent<BoardScript>().SpawnTheBoard();
                boardMesh.transform.position = spawnedObject.transform.position;    
                boardMesh.transform.rotation = spawnedObject.transform.rotation;    
                boardMesh.transform.SetParent(spawnedObject.transform);
            }
            else if(moveBoard == true)
                spawnedObject.transform.position = hitPose.position;
        }
    }
}