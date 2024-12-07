using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";
    public static Level_Manager Instance { get; private set; }

    [SerializeField] public int CurrentLevel { get; private set; } = 1;

    private string currentSpawnPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpawnPoint(string spawnPointName)
    {
        currentSpawnPoint = spawnPointName;
    }

    public void NextLevel(string nextSceneName = null)
    {
        if (!string.IsNullOrEmpty(nextSceneName) && nextSceneName == mainSceneName)
        {
            Debug.Log($"Transitioning to main scene: {mainSceneName}. Level will not increase.");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        CurrentLevel++;
        Debug.Log($"Level increased to: {CurrentLevel}");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    
        if (!string.IsNullOrEmpty(currentSpawnPoint))
        {
            GameObject player = GameObject.FindWithTag("Player");
            GameObject spawnPoint = GameObject.Find(currentSpawnPoint);

            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning("Player or SpawnPoint not found in the scene!");
            }
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
