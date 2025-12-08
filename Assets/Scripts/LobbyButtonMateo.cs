using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButtonMateo : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}