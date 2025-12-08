using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // WICHTIG für den Szenenwechsel

public class VisualMemoryManager : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> puzzleBlocks; 
    public Material defaultMaterial;
    public Material highlightMaterial;
    
    [Header("Feedback Farben")]
    public Material correctMaterial;
    public Material wrongMaterial;

    [Header("Einstellungen")]
    public float startWartezeit = 5f;
    public float leuchtDauer = 1.0f;
    public float pauseZwischenBlinks = 0.5f;
    public int lobbySceneIndex = 0; // HIER: Index der Lobby-Szene

    [HideInInspector]
    public List<GameObject> generatedSequence = new List<GameObject>();
    private int playerIndex = 0;
    public bool playerIsActive = false;

    void Start()
    {
        foreach (GameObject block in puzzleBlocks)
        {
            if(block.GetComponent<Renderer>() != null)
                block.GetComponent<Renderer>().material = defaultMaterial;
            
            if(block.GetComponent<Collider>() != null)
                block.GetComponent<Collider>().enabled = true;
        }

        StartCoroutine(PlayGameLoop());
    }

    IEnumerator PlayGameLoop()
    {
        playerIsActive = false; 
        Debug.Log("Warte auf Start...");
        yield return new WaitForSeconds(startWartezeit);

        GenerateRandomSequence();
        yield return StartCoroutine(ShowSequence());

        Debug.Log("DEIN ZUG!");
        playerIndex = 0;
        playerIsActive = true; 
    }

    void GenerateRandomSequence()
    {
        generatedSequence.Clear();
        List<GameObject> verfuegbareBloecke = new List<GameObject>(puzzleBlocks);

        for (int i = 0; i < 5; i++)
        {
            if (verfuegbareBloecke.Count == 0) break;
            int zufallsIndex = Random.Range(0, verfuegbareBloecke.Count);
            generatedSequence.Add(verfuegbareBloecke[zufallsIndex]);
            verfuegbareBloecke.RemoveAt(zufallsIndex);
        }
    }

    IEnumerator ShowSequence()
    {
        foreach (GameObject activeBlock in generatedSequence)
        {
            Renderer r = activeBlock.GetComponent<Renderer>();
            if (r != null) {
                r.material = highlightMaterial;
                yield return new WaitForSeconds(leuchtDauer);
                r.material = defaultMaterial;
                yield return new WaitForSeconds(pauseZwischenBlinks);
            }
        }
    }

    public void CheckPlayerInput(GameObject snappedObject)
    {
        if (!playerIsActive) return;

        GameObject expectedBlock = generatedSequence[playerIndex];

        if (snappedObject == expectedBlock)
        {
            Debug.Log("RICHTIG! (" + (playerIndex + 1) + "/5)");
            
            // GRÜNES FEEDBACK
            StartCoroutine(HandleFeedback(snappedObject, correctMaterial, true));

            playerIndex++; 
            
            // GEWONNEN-CHECK
            if (playerIndex >= generatedSequence.Count)
            {
                Debug.Log("GEWONNEN! Gehe zur Lobby...");
                playerIsActive = false;
                
                // Startet den Wechsel zur Lobby
                StartCoroutine(WinAndLoadLobby());
            }
        }
        else
        {
            Debug.Log("FALSCH!");
            StartCoroutine(HandleFeedback(snappedObject, wrongMaterial, false));
            playerIsActive = false; 
        }
    }

    IEnumerator HandleFeedback(GameObject target, Material feedbackMat, bool isCorrect)
    {
        Renderer r = target.GetComponent<Renderer>();
        if (r != null)
        {
            r.material = feedbackMat;
            yield return new WaitForSeconds(1.0f);
            target.SetActive(false);
        }

        if (!isCorrect)
        {
            // Bei Fehler: Level neu starten
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // NEU: Wartet kurz und lädt dann die Lobby
    IEnumerator WinAndLoadLobby()
    {
        // Wir warten 2 Sekunden, damit man das letzte grüne Licht noch sieht
        yield return new WaitForSeconds(2.0f);
        
        SceneManager.LoadScene(lobbySceneIndex);
    }
}