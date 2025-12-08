using UnityEngine;
using UnityEngine.SceneManagement; // Wichtig für Szenenwechsel

public class LobbyButtonVisualMemory : MonoBehaviour
{
    [Header("Welche Szene soll laden?")]
    // 1 = AimTrainer, 2 = Memory (je nach Build Settings)
    public int sceneIndex = 1; 

    // Diese Funktion wird gleich von deinem Controller aufgerufen
    public void OnHit()
    {
        Debug.Log("Button getroffen! Lade Szene " + sceneIndex);
        
        // Szene laden
        SceneManager.LoadScene(sceneIndex);
    }
}