using UnityEngine;
using TMPro; 
// using UnityEngine.SceneManagement; // Nicht mehr nötig, da dieses Skript nicht mehr wechselt

public class GameLoop : MonoBehaviour
{
    [Header("Zuweisungen")]
    public TextMeshProUGUI timeDisplay; // Dein Text-Objekt
    public Transform playerHead;        // Deine VR-Kamera
    
    [Header("Einstellungen")]
    public float abstand = 2.0f;        // Wie weit weg schwebt der Text?
    // public int lobbySceneIndex = 0;  // Variable entfernt, da wir hier nicht mehr wechseln

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
        // Timer stoppen
        spielLaeuft = false;
        
        string endZeit = timer.ToString("F2");
        Debug.Log("Spiel beendet: " + endZeit + " s");

        // Finaler Text
        if (timeDisplay != null)
        {
            timeDisplay.text = endZeit + " s";
            timeDisplay.color = Color.green; // Farbe ändern auf Grün
            
            // ÄNDERUNG: Hier wurde der automatische Szenenwechsel entfernt.
            // Der Text bleibt stehen, der Timer steht still, aber die Szene bleibt offen.
        }
    }

    // Die Funktion "BackToLobbyAfterSeconds" wurde komplett gelöscht.
}