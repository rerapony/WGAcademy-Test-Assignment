using System.Collections;
using UnityEngine;

public class WinSceneManager : MonoBehaviour
{
    public GameObject levelLoader;
    private float winScreenTime = 6f;
    void Start()
    {
        StartCoroutine(winScreen());
    }

    // Update is called once per frame
    IEnumerator winScreen()
    {
        yield return new WaitForSeconds(winScreenTime);
        levelLoader.GetComponent<LevelLoader>().LoadNextLevel(0);
    }
}
