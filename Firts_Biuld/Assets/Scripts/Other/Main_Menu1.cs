using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu1 : MonoBehaviour
{
   public void Play_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }


    public void Quit_Game()
    {
        Application.Quit();

    }
}
