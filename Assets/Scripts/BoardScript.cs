using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject blockPrefab;
    public GameObject win_cond;
    public GameObject levelLoader;
    public Material[] colors = new Material[COLOR_NUMBER];

    [HideInInspector]
    public float CUBE_WIDTH = 1f;
    private const int BOARD_SIZE = 5;
    private const int COLOR_NUMBER = 3;

    private float upperLeftX = -2;
    private float upperLeftY = 2;

    private List<int> color_spawn_indices = new List<int>();
    private List<int> win_spawn_indices = new List<int>();
    private List<int> win_endgame_indices = new List<int>();

    // i means x, j means y
    [HideInInspector]
    public bool[,] isEmpty = new bool[BOARD_SIZE, BOARD_SIZE];
    [HideInInspector]
    public int[,] colorArr = new int[BOARD_SIZE, BOARD_SIZE];
    void Awake()
    {
        // colors to spawn cubes
        for (int i = 0; i < COLOR_NUMBER; i++)
        {
            win_spawn_indices.Add(i);
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                color_spawn_indices.Add(i);
            }
        }
    }

    void Start()
    {
        GenerateWinCondition();

        // board is empty at the beginning
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                isEmpty[i, j] = true;
                colorArr[i, j] = -1;
            }
        }

        SpawnObjects();
    }

    void FixedUpdate()
    {
        if (CheckWinCondition())
        {
            levelLoader.GetComponent<LevelLoader>().LoadNextLevel(1);
        }
    }

    void GenerateWinCondition()
    {
        for (int i = 0; i < BOARD_SIZE; i += 2)
        {
           SpawnWinCondition(upperLeftX + i, upperLeftY + 1.5f);
        }

    }

    public bool checkValidIndices(int i, int j)
    {
        if (i >= 0 && i < BOARD_SIZE && j >= 0 && j < BOARD_SIZE)
        {
            return true;
        }
        return false;
    }

    public Vector3 getCellCoords(int i, int j)
    {
        return new Vector3(upperLeftX + i, 0, upperLeftY - j);
    }

    public Vector2Int getCellIndices(float x, float y)
    {
        return new Vector2Int((int)(x - upperLeftX), (int)(upperLeftY - y));
    }

    void SpawnObjects()
    {
        // first column
        for (int j = 0; j < BOARD_SIZE; j++)
        {
            SpawnCube(0, j);
        }

        // third column
        for (int j = 0; j < BOARD_SIZE; j++)
        {
            SpawnCube(2, j);
        }

        // fifth column
        for (int j = 0; j < BOARD_SIZE; j++)
        {
            SpawnCube(4, j);
        }

        // spawn blocks
        for (int j = 0; j < BOARD_SIZE; j += 2)
        {
            SpawnBlock(1, j);
            SpawnBlock(3, j);
        }
    }

    void SpawnCube(int i, int j)
    {
        int index = Random.Range(0, color_spawn_indices.Count - 1);    
        int color_index = color_spawn_indices[index];
        color_spawn_indices.RemoveAt(index);
        Material color = colors[color_index];

        GameObject go = Instantiate(cubePrefab, getCellCoords(i, j), Quaternion.identity);
        go.GetComponent<Renderer>().material = color;

        isEmpty[i, j] = false;
        colorArr[i, j] = color_index;
    }

    void SpawnBlock(int i, int j)
    {
        GameObject go = Instantiate(blockPrefab, getCellCoords(i, j), Quaternion.identity);
        isEmpty[i, j] = false;
    }

    void SpawnWinCondition(float x, float y)
    {
        int index = Random.Range(0, win_spawn_indices.Count - 1);
        int color_index = win_spawn_indices[index];
        win_spawn_indices.RemoveAt(index);
        
        Material color = colors[color_index];
        win_endgame_indices.Add(color_index);

        GameObject go = Instantiate(win_cond, new Vector3(x, 0, y), Quaternion.identity);
        go.GetComponent<Renderer>().material = color;
    }

    bool CheckWinCondition()
    {
        for (int i = 0; i < COLOR_NUMBER; i++)
        {
            int color_index = win_endgame_indices[i];
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (colorArr[2*i, j] != color_index)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
}
