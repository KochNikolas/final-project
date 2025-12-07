using UnityEngine;

public class TargetLogic : MonoBehaviour
{
    public void OnHit()
    {
        Debug.Log("TREFFER! Ziel wurde getroffen.");
        
        // Optional: Zerstört den Würfel sofort (für das Aim-Trainer-Feeling)
        // Destroy(gameObject); 
    }
}