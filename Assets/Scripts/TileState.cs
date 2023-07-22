using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileState : MonoBehaviour
{
     private Color color;
     public new Renderer renderer;

    public void Awake()
    {

        if (this.transform.position.x % 2 == 0 && this.transform.position.z % 2 != 0)
        {

            this.renderer.material.SetColor("_Color", new Color(1, 0.3f, 0.3f, 1));
        }
        if (this.transform.position.z % 2 == 0 && this.transform.position.x % 2 != 0)
        {

            this.renderer.material.SetColor("_Color", new Color(1, 0.3f, 0.3f, 1));
        }

    }
}
