using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class BoardScript : MonoBehaviour
{

    ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //probably make these serializable
    static int rows = 8;
    static int columns = 8;
    int piecesSpawned;

    public Transform Tile;
    public Transform Piece;
    public GameObject redPrefab;
    public GameObject blackPrefab;
    public GameObject tilePrefab;
    public Piece[,] pieces = new Piece[8,8];
    public Vector2 activePiece;
    Vector2[] moves;
    public GameObject go1;
    public Animator anime;



    Vector2 touchPosition;


    
    public bool redTurn;

    int[,] boardcoords = new int[rows, columns];


    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        anime = new Animator();
    }
    private void Start()
    {
        redTurn = true;
        SpawnTheBoard();
        //set active piece to be extr
        activePiece.x = -1;
        activePiece.y = -1;

        moves = new Vector2[2];


        if (anime != null)
        {
            anime.applyRootMotion = false;
        }
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
    private void Update()
    {
        
        // check for touch on screen, spawn checker at location just for testing



        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
           
            if (Physics.Raycast(ray, out Hit))
            {
                Debug.Log("Raycast");
                if (activePiece.x == -1 && activePiece.y == -1)
                {



                    if (Hit.transform.gameObject == redPrefab || Hit.transform.gameObject == blackPrefab)
                    {

                        // take individual hit, change bool, then store in vec2 activepiece
                        Hit.transform.gameObject.GetComponent<Piece>().SetActive();
                        SetActivePiece();
                    }
                }
                else
                {
                    
                    if (Hit.transform.gameObject.tag == "Tile")
                    {
                        
                        //Piecemove((int)activePiece.x, (int)activePiece.y, (int)moves[0].x, (int)moves[0].y);
                        Vector2 temp;
                        temp.x = (int)Hit.transform.gameObject.GetComponent<Transform>().position.x;
                        temp.y = (int)Hit.transform.gameObject.GetComponent<Transform>().position.z;
                        
                        if (temp - activePiece == moves[0])
                        {
                            Piecemove((int)activePiece.x, (int)activePiece.y, (int)moves[0].x, (int)moves[0].y);

                            if (Mathf.Abs(moves[0].x) == 2)
                            {

                                StartCoroutine(DestroyJumpedPiece(0));
                            }

                            
                            
                        }
                               
                        if (temp - activePiece == moves[1])
                        {
                            
                            Piecemove((int)activePiece.x, (int)activePiece.y, (int)moves[1].x, (int)moves[1].y);

                            if (Mathf.Abs(moves[1].x) == 2)
                            {
                                StartCoroutine(DestroyJumpedPiece(1));
                            }

                            
                        }

                    }
                    RemoveActivePiece();
                    
                }
            }
        }



           
        if (Input.GetMouseButtonDown(0))
        {
            
            SetActivePiece();
           
        }
        

            // on click of piece, 
        if (Input.GetKeyDown("r"))
        {
            Vector2[] temp = ViableMoves(3, 2);
            
            
          

            
        }
    }

    IEnumerator DestroyJumpedPiece(int index)
    {
        Vector2 DestroyLoc = new Vector2((int)(activePiece.x + (moves[index].x / 2.0f)), (int)(activePiece.y + (moves[index].y / 2.0f)));
        pieces[(int)DestroyLoc.x, (int)DestroyLoc.y].DestroySelf();

        yield return new WaitForSeconds(4.0f);
        
        pieces[(int)DestroyLoc.x, (int)DestroyLoc.y].gameObject.SetActive(false);
        pieces[(int)DestroyLoc.x, (int)DestroyLoc.y] = null;
    }
     
    public void SetActivePiece()
    {
       
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (pieces[i,j] != null && pieces[i, j].activePiece)
                {
                    
                    activePiece.x = (int)i;
                    activePiece.y = (int)j;
                    
                    moves = ViableMoves(i, j);
                
                  
                }
                
            }
        }
        
    }
    public void RemoveActivePiece()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (pieces[i, j] != null)
                {
                    pieces[i,j].activePiece = false;
                    activePiece.x = -1;
                    activePiece.y = -1;
                    pieces[i,j].gameObject.SetActive(true);
                    
                    
                }

            }
        }
    }


    public void Piecemove(int coordx, int coordy, int x, int z)
    {
        Piece p = new Piece();
        p.test();

        // check to see if selected spot has piece, and if desired spot is empty
        if (pieces[coordx,coordy] != null && pieces[coordx + x, coordy + z] == null)
        {
            p = pieces[coordx, coordy];

            // change physical position
            Vector3 cur = p.transform.position;
            Vector3 pos = new Vector3(x, 0, z);
            p.transform.position = cur + pos;

            // move the piece from point a to point b in list
            pieces[coordx, coordy] = null;
            pieces[coordx + x, coordy + z] = p;
            
  
      
        }


        

        
    }

    public Vector2[] ViableMoves(int coordx, int coordy)
    {

        //do a boundary check on all if statements
        Vector2[] moves = new Vector2[2];
        

        //moves = list of possible moves, stored as +1 -1/+2 -2 from origin
        //
        // store as Vec2, with an x and y value ie. [(1,1), (-1,1)]
        //
        // means that right 1 and up 1 is viable
        // as well as left 1 and up 1

        Debug.Log("func check");
        if (pieces[coordx, coordy].colour == "red") {


            // still no check for if value is greater than or less than list length

            Debug.Log("red prefab check");
            if (pieces[coordx + 1, coordy + 1] == null)
            {
                moves[0].x = 1;
                moves[0].y = 1;

            }
            Debug.Log("red prefab check");
            if (pieces[coordx + 2, coordy + 2] == null && pieces[coordx + 1, coordy + 1] != null)
            {
                moves[0].x = 2;
                moves[0].y = 2;
            }
            if (pieces[coordx - 1, coordy + 1] == null)
            {
                moves[1].x = -1;
                moves[1].y = 1;
            }
            if (pieces[coordx - 2, coordy + 2] == null && pieces[coordx - 1, coordy + 1] != null)
            {
                moves[1].x = -2;
                moves[1].y = 2;
            }
        }
        if (pieces[coordx, coordy].colour == "black") {
            
            if (pieces[coordx + 1, coordy - 1] == null)
            {
                moves[0].x = 1;
                moves[0].y = -1;
            }
            if (pieces[coordx + 2, coordy - 2] == null && pieces[coordx + 1, coordy - 1] != null)
            {
                moves[0].x = 2;
                moves[0].y = -2;
            }
            if (pieces[coordx - 1, coordy - 1] == null)
            {
                moves[1].x = -1;
                moves[1].y = -1;
            }
            if (pieces[coordx - 2, coordy - 2] == null && pieces[coordx - 1, coordy - 1] != null)
            {
                moves[1].x = -2;
                moves[1].y = -2;
            }
        }

     

       
        Debug.Log(moves[0].x);
        Debug.Log(moves[0].y);
        return moves;

            // returns vec2 list, with the first value as the x movement
            // second value is the y movement
        
       

     }
    
    

    public void SpawnTheBoard()
    {
        for (int i = 0; i < rows; i++)
        {
            piecesSpawned++;
            for (int j = 0; j < columns; j++)
            {
                Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                tilePrefab.GetComponent<Renderer>().sharedMaterial.color = new Color(0.2f, 0.2f, 0.2f, 1);
                piecesSpawned++;



                if (piecesSpawned % 2 == 1 && j < 3)
                {

                    GameObject go = Instantiate(redPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);

                    pieces[i, j] = go.GetComponent<Piece>();
                    pieces[i, j].colour = "red";

                }
                if (piecesSpawned % 2 == 1 && j > 4)
                {

                    GameObject go = Instantiate(blackPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                    pieces[i, j] = go.GetComponent<Piece>();
                    pieces[i, j].colour = "black";

                }
            }
        }
    }
}

