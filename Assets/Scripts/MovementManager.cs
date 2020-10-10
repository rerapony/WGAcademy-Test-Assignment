using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class MovementManager : MonoBehaviour
{
    public AudioSource moveSound;

    private Camera mainCamera;
    private BoardScript board;

    private GameObject selectedObj;

    Vector3 curPos;
    Vector2Int curPosCell;

    Vector3 newPos;
    Vector2Int newPosCell;

    void Start()
    {
        mainCamera = Camera.main;
        board = mainCamera.GetComponent<BoardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                selectedObj = hit.transform.gameObject;
                if (selectedObj.CompareTag("UpClick") || selectedObj.CompareTag("DownClick") 
                    || selectedObj.CompareTag("RightClick") || selectedObj.CompareTag("LeftClick"))
                {
                    GameObject cube = selectedObj.transform.parent.gameObject;
                    calculateMovement(cube, selectedObj.tag);
                }
            }
        }
    }

    void calculateMovement(GameObject cube, string command)
    {
        curPos = cube.transform.position;
        curPosCell = board.getCellIndices(curPos.x, curPos.z);

        int d_x = 0;
        int d_y = 0;

        if (command == "UpClick")
        {
            d_y = 1;
        } else if (command == "DownClick")
        {
            d_y = -1;
        } else if (command == "RightClick")
        {
            d_x = 1;
        } else if (command == "LeftClick")
        {
            d_x = -1;
        }

        newPos = new Vector3(curPos.x + d_x, curPos.y, curPos.z + d_y);
        newPosCell = board.getCellIndices(newPos.x, newPos.z);

        if (board.checkValidIndices(newPosCell.x, newPosCell.y) && board.isEmpty[newPosCell.x, newPosCell.y]) {
            cube.transform.position = new Vector3(newPos.x, curPos.y, newPos.z);
            moveSound.Play(0);

            board.isEmpty[curPosCell.x, curPosCell.y] = true;
            board.isEmpty[newPosCell.x, newPosCell.y] = false;

            int color_index = board.colorArr[curPosCell.x, curPosCell.y];
            board.colorArr[curPosCell.x, curPosCell.y] = -1;
            board.colorArr[newPosCell.x, newPosCell.y] = color_index;
        }
    }
}
