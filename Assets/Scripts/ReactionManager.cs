using UnityEngine;
using TMPro;
using System.Collections;

public class ReactionManager : MonoBehaviour
{
    public GameObject targetPrefab; 
    public int anzahlZiele = 20;

    [Header("Abstand zum Spieler (Radius)")]
    public float minAbstand = 3f;  // Wie nah darf ein Ziel sein?
    public float maxAbstand = 8f;  // Wie weit darf es weg sein?

    [Header("Höhe")]
    public float hoeheMin = 1.0f;
    public float hoeheMax = 3.0f;

    public Transform player; // Should be the XR camera, not the XR Origin
    [Header("Room Settings")]
    public Renderer roomRenderer;

    [Header("Button Settings")]
    public GameObject buttonPrefab;

    [Header("Timer Settings")]
    public TextMeshPro timeText;

    private GameObject activeButton;
    private float reactionStartTime;
    private bool timerRunning = false;


    void Start()
    {
        StartTrial();
    }

    public void StartTrial()
    {
        // Reset room and text
        roomRenderer.sharedMaterial.color = Color.red;
        if (timeText != null)
            timeText.text = "Wait...";


        // Remove any existing button
        if (activeButton != null)
            Destroy(activeButton);

        // Start reaction routine
        StartCoroutine(WaitAndTurnGreen());
    }

    IEnumerator WaitAndTurnGreen()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        // Turn room green and start timer
        roomRenderer.sharedMaterial.color = Color.green;
        timeText.text = "Find the Button!";
        reactionStartTime = Time.time;
        timerRunning = true;

        // Spawn the button
        SpawnButton();
    }

    void SpawnButton()
    {
        for (int i = 0; i < anzahlZiele; i++)
        {
            // 1. Zufälliger Winkel im vollen Kreis (0 bis 360 Grad)
            float winkel = Random.Range(0f, 360f);

            // 2. Zufälliger Abstand zwischen Min und Max
            float aktuellerAbstand = Random.Range(minAbstand, maxAbstand);

            // 3. Position berechnen
            // Wir drehen den Vektor "Vorne" um den zufälligen Winkel und schieben ihn raus
            Vector3 spawnPosition = Quaternion.Euler(0, winkel, 0) * Vector3.forward * aktuellerAbstand;

            // 4. Zufällige Höhe hinzufügen
            spawnPosition.y = Random.Range(hoeheMin, hoeheMax);

            // 5. Ziel erstellen
            GameObject neuesZiel = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

            // 6. Zum Spieler drehen (damit die Scheibe dich immer anschaut)
            neuesZiel.transform.LookAt(Vector3.zero);
        }
    }

    Vector3 RandomCanvasPosition()
    {
        float radius = 1.2f; // distance from player

        // Random angle in 360° around player
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );

        // Base position relative to player
        Vector3 pos = player.position + offset;

        // Keep at eye height
        pos.y = player.position.y;

        // Clamp to room bounds (example: 10x3x10 room centered at origin)
        float halfRoomX = 5f;  // half width of room
        float halfRoomZ = 5f;  // half depth of room
        pos.x = Mathf.Clamp(pos.x, -halfRoomX + 0.5f, halfRoomX - 0.5f); // 0.5 margin from walls
        pos.z = Mathf.Clamp(pos.z, -halfRoomZ + 0.5f, halfRoomZ - 0.5f);

        return pos;
    }



    public void StopTimer()
    {
        if (!timerRunning) return;

        timerRunning = false;
        float reactionTime = (Time.time - reactionStartTime) * 1000f;
        if (timeText != null)
            timeText.text = reactionTime.ToString("F0") + " ms";

        if (activeButton != null)
            Destroy(activeButton);

        // Optionally reset room back to red immediately
        roomRenderer.sharedMaterial.color = Color.black;
    }
}