using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour, ISceneLoader
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}