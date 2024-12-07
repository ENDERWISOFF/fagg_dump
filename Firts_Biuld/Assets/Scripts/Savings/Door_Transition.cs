using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Transition : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private string spawnPointName;
    private bool isPlayerNearby = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Transition");
            
            Level_Manager.Instance.SetSpawnPoint(spawnPointName);
       
            Level_Manager.Instance.NextLevel(nextSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter_Player");
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

}
