using UnityEngine;
using UnityEngine.XR;

public class SimpleRaycastShooter : MonoBehaviour
{
    [Header("Einstellungen")]
    public XRNode controllerNode = XRNode.RightHand; 
    public float waffenReichweite = 100f;
    
    private bool triggerWarGedrueckt = false;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.isValid)
        {
            bool triggerIstGedrueckt;
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerIstGedrueckt) && triggerIstGedrueckt)
            {
                if (!triggerWarGedrueckt)
                {
                    Feuer(); // Schuss!
                    triggerWarGedrueckt = true;
                }
            }
            else
            {
                triggerWarGedrueckt = false;
            }
        }
    }

    void Feuer()
    {
        RaycastHit trefferInfo;        
        // Strahl schießen
        if (Physics.Raycast(transform.position, transform.forward, out trefferInfo, waffenReichweite))
        {
            // --- Fall A: Es ist ein Ziel (Aim Trainer) ---
            if (trefferInfo.transform.CompareTag("Target"))
            {
                Destroy(trefferInfo.transform.gameObject);
            }
            else
            if (trefferInfo.transform.CompareTag("AimTrainer"))
            {
                LobbyButtonAimTrainer button = trefferInfo.transform.GetComponent<LobbyButtonAimTrainer>();
                button.OnHit();
            }
            else
            if (trefferInfo.transform.CompareTag("VisualMemory"))
            {
                LobbyButtonVisualMemory button = trefferInfo.transform.GetComponent<LobbyButtonVisualMemory>();
                button.OnHit();
            }
            else
            if (trefferInfo.transform.CompareTag("ReactionTime"))
            {
                LobbyButtonReactionTime button = trefferInfo.transform.GetComponent<LobbyButtonReactionTime>();
                button.OnHit();
            }
            else
            if (trefferInfo.transform.CompareTag("ReactionTarget"))
            {
                ReactionManager manager = FindObjectOfType<ReactionManager>();

                if (manager != null)
                {
                    manager.StopTimer();
                    
                    Destroy(trefferInfo.transform.gameObject); 
                }
                else
                {
                    Debug.LogError("Fehler: Kein ReactionManager in der Szene gefunden!");
                }
            }
        }
    }
}