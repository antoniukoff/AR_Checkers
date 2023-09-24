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

    private GameObject spawnedObject;
    private ARRaycastManager arRaycastManager;
    private Vector2 touchPosition;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private float initialDistance;
    private Vector3 initialScale;
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
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Touch touch3 = Input.GetTouch(2);
            
            Vector2 currentAvgPosition = (touch1.position + touch2.position + touch3.position) / 3;

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began || touch3.phase == TouchPhase.Began)
            {
                initialTouchAvgPosition = currentAvgPosition;
                initialObjectPosition = spawnedObject.transform.position;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved || touch3.phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = currentAvgPosition - initialTouchAvgPosition;
                Vector3 newPosition = initialObjectPosition + new Vector3(deltaPosition.x, 0, deltaPosition.y) * 0.01f;
                spawnedObject.transform.position = newPosition;
            }
            return;
        }

        // Placement
        if (!GetTouchPosition(out touchPosition))
            return;

        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                // Spawn the board on the instantiated object
                spawnedObject = prefabToSpawn.GetComponent<BoardScript>().SpawnTheBoard(hitPose.position, hitPose.rotation);
            }
            //else
            //{
            //    spawnedObject.transform.position = hitPose.position;
            //}
        }
    }
}

