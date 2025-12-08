using UnityEngine;
using TMPro; 
using System.Collections;
using UnityEngine.SceneManagement; // WICHTIG: Das hier wurde hinzugefügt für den Szenenwechsel

public class GameLoop : MonoBehaviour
{
    [Header("Zuweisungen")]
    public TextMeshProUGUI timeDisplay; // Dein Text-Objekt
    public Transform playerHead;        // Deine VR-Kamera
    
    [Header("Einstellungen")]
    public float abstand = 2.0f;        // Wie weit weg schwebt der Text?
    public int lobbySceneIndex = 0;     // HIER: Die Nummer der Lobby-Szene in den Build Settings

    private float timer = 0f;
    private bool spielLaeuft = true;

    void Update()
    {
        // 1. HUD-Logik: Text immer vor das Gesicht bewegen
        UpdateTextPosition();

        if (spielLaeuft)
        {
            // 2. Zeit zählen
            timer += Time.deltaTime;

            // 3. Zeit live anzeigen
            if (timeDisplay != null)
            {
                timeDisplay.text = timer.ToString("F2");
            }

            // 4. Prüfen: Sind alle Ziele weg?
            if (timer > 1.0f && GameObject.FindGameObjectsWithTag("Target").Length == 0)
            {
                GameOver();
            }
        }
    }

    void UpdateTextPosition()
    {
        if (timeDisplay != null && playerHead != null)
        {
            Transform canvasTransform = timeDisplay.transform.parent; // Wir bewegen das ganze Canvas

            // Position: Genau vor dem Kopf in Blickrichtung
            Vector3 zielPosition = playerHead.position + (playerHead.forward * abstand);
            
            // Setzen
            canvasTransform.position = zielPosition;

            // Rotation: Text soll den Spieler anschauen
            canvasTransform.LookAt(playerHead);
            
            // Korrektur: Da UI sonst spiegelverkehrt ist, drehen wir es um 180 Grad
            canvasTransform.Rotate(0, 180, 0);
        }
    }

    void GameOver()
    {
        spielLaeuft = false;
        
        string endZeit = timer.ToString("F2");
        Debug.Log(endZeit + " s");

        // Finaler Text
        if (timeDisplay != null)
        {
            timeDisplay.text = endZeit + " s"; // Optional: "Endzeit" hinzugefügt für Klarheit
            timeDisplay.color = Color.green; // Farbe ändern (optional)
            
            // Startet den Countdown zum Szenenwechsel
            StartCoroutine(BackToLobbyAfterSeconds(5));
        }
    }

    IEnumerator BackToLobbyAfterSeconds(float sekunden)
    {
        // Wartet 5 Sekunden, während der Text weiter dem Kopf folgt (weil Update weiterläuft)
        yield return new WaitForSeconds(sekunden);
        
        // HIER passiert der Wechsel:
        SceneManager.LoadScene(lobbySceneIndex);
    }
}