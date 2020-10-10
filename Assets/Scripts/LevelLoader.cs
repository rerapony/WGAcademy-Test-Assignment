using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private float transitionTime = 1f;

    public void LoadNextLevel(int scene_index)
    {
        StartCoroutine(loadScene(scene_index));
    }

    IEnumerator loadScene(int scene_index)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(scene_index);
    }
}
