using UnityEngine;

public class UI_Manadger : MonoBehaviour
{
    private static UI_Manadger instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }
}
