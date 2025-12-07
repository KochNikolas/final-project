using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Was soll gespawnt werden?")]
    public GameObject targetPrefab; 
    public int anzahlZiele = 20;

    [Header("Abstand zum Spieler (Radius)")]
    public float minAbstand = 3f;  // Wie nah darf ein Ziel sein?
    public float maxAbstand = 8f;  // Wie weit darf es weg sein?

    [Header("Höhe")]
    public float hoeheMin = 1.0f;
    public float hoeheMax = 3.0f;

    void Start()
    {
        SpawnTargets();
    }

    void SpawnTargets()
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
}