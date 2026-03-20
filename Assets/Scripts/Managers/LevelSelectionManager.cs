// *************************************************************** //
// Script done by Alvaro
// Loads and unloads the minigames scenes
// In progress
// The unload is left
// *************************************************************** //
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    public string[] m_MinigamesSceneNames;
    private void Awake()
    {
        // This manager will stay on all scenes, so we can call the chooseRandomMinigame function from any scene
        DontDestroyOnLoad(transform.gameObject);

        DirectoryInfo dir = new DirectoryInfo("Assets/Scenes/TestingScenes");
        FileInfo[] info = dir.GetFiles("*.unity");

        m_MinigamesSceneNames = new string[info.Length];

        int i = 0;
        foreach (FileInfo f in info)
        {
            m_MinigamesSceneNames[i] = f.Name.Split(".")[0];
            i++;
        }
    }

    public void chooseRandomMinigame()
    {
        // Must be defined the inital scenes
        int randomIndex = Random.Range(0, UnityEngine.SceneManagement.SceneManager.sceneCount);
        UnityEngine.SceneManagement.SceneManager.LoadScene(randomIndex);
    }

    public void chooseMinigame(int index)
    {
        // Must be defined the scenes
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
}
