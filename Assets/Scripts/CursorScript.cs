using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CursorScript : MonoBehaviour
{
    //make sure to change this

    public GameObject cursorChildobject; // this represents specifucally the sprite that is not yet attached to this cursor
    public Checker checker;
    public ARRaycastManager raycastmanager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.ViewportToScreenPoint(new Vector2((0.5f), (0.5f)));
        //this is a list because we are reciving a queue of inputs, instead of just 1
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastmanager.Raycast(pos, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            Checker.Instantiate(checker, transform.position, transform.rotation);
        }
    }
}
