using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BoardScript : MonoBehaviour
{
    ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static int rows = 8;
    static int columns = 8;
    int piecesSpawned;
    public Transform Tile, Piece;
    public GameObject redPrefab, blackPrefab, tilePrefab, go1;
    public Piece[,] pieces = new Piece[8,8];
    public Vector2 activePiece;
    Vector2[] moves;
    public Animator anime;
    Vector2 touchPosition;
    GameObject boardParent;
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
        activePiece.x = -1;
        activePiece.y = -1;
        moves = new Vector2[2];
        if (anime != null) anime.applyRootMotion = false;
    }

    bool GetTouchPosition(out Vector2 touchPosition)
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
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit Hit))
            {
                if (activePiece.x == -1 && activePiece.y == -1)
                {
                    if (Hit.transform.gameObject == redPrefab || Hit.transform.gameObject == blackPrefab)
                    {
                        Hit.transform.gameObject.GetComponent<Piece>().SetActive();
                        SetActivePiece();
                    }
                }
                else if (Hit.transform.gameObject.tag == "Tile")
                {
                    Vector3 relativePosition = gameObject.transform.InverseTransformPoint(Hit.transform.position);
                    Vector2 temp;
                    temp.x = Mathf.RoundToInt(relativePosition.x);
                    temp.y = Mathf.RoundToInt(relativePosition.z);


                    Debug.Log("Temp: " + temp);
                    if ((temp - activePiece) == moves[0])
                    {
                        Piecemove((int)activePiece.x, (int)activePiece.y, (int)moves[0].x, (int)moves[0].y);
                        if (Mathf.Abs(moves[0].x) == 2) StartCoroutine(DestroyJumpedPiece(0));
                    }
                    if (temp - activePiece == moves[1])
                    {
                        Piecemove((int)activePiece.x, (int)activePiece.y, (int)moves[1].x, (int)moves[1].y);
                        if (Mathf.Abs(moves[1].x) == 2) StartCoroutine(DestroyJumpedPiece(1));
                    }
                    RemoveActivePiece();
                }
            }
        }
        if (Input.GetMouseButtonDown(0)) SetActivePiece();
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
                    activePiece.x = i;
                    activePiece.y = j;
                    Debug.Log("Active Piece: " + activePiece);
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
    if (pieces[coordx, coordy] != null && pieces[coordx + x, coordy + z] == null)
    {
        Piece p = pieces[coordx, coordy];
        Vector3 moveVector = new Vector3(x, 0, z);  
        Vector3 scaledMoveVector = Vector3.Scale(moveVector, boardParent.transform.localScale);  
        p.transform.localPosition += scaledMoveVector;  
        pieces[coordx, coordy] = null;
        pieces[coordx + x, coordy + z] = p;
    }
}



    public Vector2[] ViableMoves(int coordx, int coordy)
{
    Vector2[] moves = new Vector2[2];
    if (pieces[coordx, coordy].colour == "red")
    {
        if (coordx + 1 < rows && coordy + 1 < columns && pieces[coordx + 1, coordy + 1] == null) 
        moves[0] = new Vector2(1, 1);
        if (coordx + 2 < rows && coordy + 2 < columns && pieces[coordx + 2, coordy + 2] == null && pieces[coordx + 1, coordy + 1] != null)
        moves[0] = new Vector2(2, 2);
        if (coordx - 1 >= 0 && coordy + 1 < columns && pieces[coordx - 1, coordy + 1] == null) 
        moves[1] = new Vector2(-1, 1);
        if (coordx - 2 >= 0 && coordy + 2 < columns && pieces[coordx - 2, coordy + 2] == null && pieces[coordx - 1, coordy + 1] != null) 
        moves[1] = new Vector2(-2, 2);
    }
    if (pieces[coordx, coordy].colour == "black")
    {
        if (coordx + 1 < rows && coordy - 1 >= 0 && pieces[coordx + 1, coordy - 1] == null)
        moves[0] = new Vector2(1, -1);
        if (coordx + 2 < rows && coordy - 2 >= 0 && pieces[coordx + 2, coordy - 2] == null && pieces[coordx + 1, coordy - 1] != null) 
        moves[0] = new Vector2(2, -2);
        if (coordx - 1 >= 0 && coordy - 1 >= 0 && pieces[coordx - 1, coordy - 1] == null) 
        moves[1] = new Vector2(-1, -1);
        if (coordx - 2 >= 0 && coordy - 2 >= 0 && pieces[coordx - 2, coordy - 2] == null && pieces[coordx - 1, coordy - 1] != null) 
        moves[1] = new Vector2(-2, -2);
    }
    return moves;
}


    public GameObject SpawnTheBoard()
    {
         boardParent = new GameObject("BoardMesh");
        for (int i = 0; i < rows; i++)
        {
            piecesSpawned++;
            for (int j = 0; j < columns; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                tilePrefab.GetComponent<Renderer>().sharedMaterial.color = new Color(0.2f, 0.2f, 0.2f, 1);
                piecesSpawned++;
                tile.transform.SetParent(boardParent.transform);
                if (piecesSpawned % 2 == 1 && j < 3)
                {
                    GameObject go = Instantiate(redPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                    pieces[i, j] = go.GetComponent<Piece>();
                    pieces[i, j].colour = "red";
                    go.transform.SetParent(boardParent.transform);
                }
                if (piecesSpawned % 2 == 1 && j > 4)
                {
                    GameObject go = Instantiate(blackPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                    pieces[i, j] = go.GetComponent<Piece>();
                    pieces[i, j].colour = "black";
                    go.transform.SetParent(boardParent.transform);
                }
            }
        }
        return boardParent;
    }
}
