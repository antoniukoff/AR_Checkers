using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class W05_TapToPlaceObject : MonoBehaviour
{
    public GameObject prefabToSpawn;

    GameObject spawnedObject;
    ARRaycastManager arRaycastManager;
    Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool GetTouchPosition(out Vector2 touchPosition)
    {
        // Check if there are touches on Screen
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if there was no Touch event on Screen
        if (!GetTouchPosition(out touchPosition))
            return;

        // Touch event occured
        //https://docs.unity3d.com/2019.2/Documentation/ScriptReference/Experimental.XR.TrackableType.html
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
                spawnedObject = Instantiate(prefabToSpawn, hitPose.position, hitPose.rotation);
            else
                spawnedObject.transform.position = hitPose.position;
        }
    }
}