using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private Color color;
    public new Renderer renderer;
    public Rigidbody checker;
    //this is which player owns the piece. May not make sense as a string
    string ownership;


    public void Awake()
    {
        // change color pased on player ownership
        if (this.transform.position.z < 3)
        {

            this.renderer.material.SetColor("_Color", Color.red);
        }
        else if (this.transform.position.z > 4)
        {
            this.renderer.material.SetColor("_Color", Color.black);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //use different constraint maybe
        checker.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void checkermove()
    {

    }
}
