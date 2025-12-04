using UnityEngine;
using TMPro;
using System.Collections;

public class ReactionManager : MonoBehaviour
{
    [Header("Room Settings")]
    public Renderer roomRenderer;

    [Header("Button Settings")]
    public GameObject buttonPrefab;
    public Transform player; // XR Origin → Camera Offset

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
        Vector3 spawnPos = RandomPositionAroundPlayer();
        activeButton = Instantiate(buttonPrefab, spawnPos, Quaternion.identity);

        ReactionButton rb = activeButton.GetComponent<ReactionButton>();
        if (rb != null)
            rb.reactionManager = this;
    }

    Vector3 RandomPositionAroundPlayer()
    {
        float radius = 1.2f; // within arm’s reach
        float angle = Random.Range(0f, 360f);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );

        return player.position + offset + new Vector3(0, 0, 0);
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
        roomRenderer.sharedMaterial.color = Color.red;
    }
}
