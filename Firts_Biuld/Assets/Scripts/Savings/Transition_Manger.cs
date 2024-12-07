using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Manger : MonoBehaviour
{
    public static Transition_Manger Instance;

    public Vector3 spawnPosition; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(string sceneName, Vector3 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
        SceneManager.LoadScene(sceneName); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
