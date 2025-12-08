using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabSceneLoader : MonoBehaviour
{
    [Header("Wohin soll es gehen?")]
    public int sceneIndexToLoad = 0; // 0 ist meistens die Lobby

    // Diese Funktion rufen wir gleich auf, wenn der Würfel gegriffen wird
    public void OnGrab()
    {
        Debug.Log("Objekt gegriffen! Wechsle zu Szene " + sceneIndexToLoad);
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}