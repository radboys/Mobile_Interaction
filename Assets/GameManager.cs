using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private List<string> scenes = new List<string> { "Level1", "Level2", "Level3", "Level4" };
    private List<string> remainingScenes;

    public static GameManager Instance;

    public string level1Result;
    public string level2Result;
    public string level3Result;
    public string level4Result;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        remainingScenes = new List<string>(scenes);

        Cursor.lockState = CursorLockMode.None;

    }

    private void FixedUpdate()
    {

    }

    public void StoreResult(string result)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                level1Result = result;
                break;

            case "Level2":
                level2Result = result;
                break;

            case "Level3":
                level3Result = result;
                break;

            case "Level4":
                level4Result = result;
                break;
        }
    }

    public void ChangeScene()
    {
        if (remainingScenes.Count > 0)
        {
            int index = Random.Range(0, remainingScenes.Count);
            string sceneToLoad = remainingScenes[index];
            remainingScenes.RemoveAt(index);

            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
