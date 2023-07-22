using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update

    public string str;
    public bool activePiece;
    public Animator anime;
    public Renderer myRenderer;

    // colour so that boardscript can differentiate during runtime
    public string colour;
    void Start()
    {
        GetComponent<Animator>().applyRootMotion = true;
        str = "test";
        activePiece = false;
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetMouseButtonDown(0))
        // {
        //     activePiece = false;
        // }
    }

    
    public void OnMouseDown()
    {



        
        GetComponent<Animator>().SetTrigger("Tapped");
        GetComponent<Animator>().SetTrigger("Return");
        Debug.Log("pressed mouse");
        activePiece = true;
    }

    public void SetActive()
    {

        GetComponent<Animator>().SetTrigger("Tapped");
        GetComponent<Animator>().SetTrigger("Return");
        activePiece =true;





    }

    public void DestroySelf()
    {
        GetComponent<Animator>().SetTrigger("Destroyed");
        GetComponent<Animator>().SetTrigger("Return");
       



    }
    public void OnMouseUp()
    {
        
    }
    public void test()
    {
        Debug.Log("hello!!!!!");
    }
}
