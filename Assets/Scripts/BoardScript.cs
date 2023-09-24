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
    static int rows = 8;
    static int columns = 8;
    
    public GameObject redPrefab;
    public GameObject blackPrefab;
    public GameObject tilePrefab;
    public GameObject parent;
    public Piece[,] pieces = new Piece[8,8];
    public Vector2 activePiece;
    Vector2[] moves;
    public Animator anime;
    public bool redTurn;

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
       // SpawnTheBoard(new Vector3(0,5,-15), Quaternion.identity);
        if (anime != null)
        {
            anime.applyRootMotion = false;
        }
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
            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit))
            {
                if (activePiece.x == -1 && activePiece.y == -1)
                {
                    if (Hit.transform.gameObject == redPrefab || Hit.transform.gameObject == blackPrefab)
                    {
                        Hit.transform.gameObject.GetComponent<Piece>().SetActive();
                        SetActivePiece();
                    }
                }
                else
                {
                    if (Hit.transform.gameObject.tag == "Tile")
                    {
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
        if (pieces[coordx,coordy] != null && pieces[coordx + x, coordy + z] == null)
        {
            p = pieces[coordx, coordy];
            Vector3 cur = p.transform.position;
            Vector3 pos = new Vector3(x, 0, z);
            p.transform.position = cur + pos;
            pieces[coordx, coordy] = null;
            pieces[coordx + x, coordy + z] = p;
        }
    }

    public Vector2[] ViableMoves(int coordx, int coordy)
    {
        Vector2[] moves = new Vector2[2];
        if (pieces[coordx, coordy].colour == "red")
        {
            if (pieces[coordx + 1, coordy + 1] == null)
            {
                moves[0].x = 1;
                moves[0].y = 1;
            }
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
        if (pieces[coordx, coordy].colour == "black")
        {
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
        return moves;
    }

    public GameObject SpawnTheBoard(Vector3 position, Quaternion rotation)
    {
        GameObject boardParent = new GameObject("Board");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                tile.transform.SetParent(boardParent.transform);
                if ((i + j) % 2 == 0)
                {
                    if (j < 3)
                    {
                        GameObject piece = Instantiate(redPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                        piece.GetComponent<Piece>().colour = "red";
                        piece.transform.SetParent(boardParent.transform);
                        pieces[i, j] = piece.GetComponent<Piece>();
                    }
                    else if (j > 4)
                    {
                        GameObject piece = Instantiate(blackPrefab, new Vector3(i, 0.5f, j), Quaternion.identity);
                        piece.GetComponent<Piece>().colour = "black";
                        piece.transform.SetParent(boardParent.transform);
                        pieces[i, j] = piece.GetComponent<Piece>();
                    }
                }
            }
        }
        boardParent.transform.position = position;
        boardParent.transform.rotation = rotation;
        return boardParent;
    }
}
