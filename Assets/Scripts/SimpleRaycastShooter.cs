using UnityEngine;
using UnityEngine.XR;

public class SimpleRaycastShooter : MonoBehaviour
{
    [Header("Einstellungen")]
    // HIER: Jetzt kannst du im Inspector auswählen, welche Hand es ist!
    public XRNode controllerNode = XRNode.RightHand; 
    
    public float waffenReichweite = 100f;
    
    private bool triggerWarGedrueckt = false;

    void Update()
    {
        // Wir nutzen jetzt die Variable 'controllerNode' statt fest 'RightHand'
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.isValid)
        {
            bool triggerIstGedrueckt;

            // Trigger (Zeigefinger) abfragen
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerIstGedrueckt) && triggerIstGedrueckt)
            {
                if (!triggerWarGedrueckt)
                {
                    Feuer();
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
            // Prüfen ob es ein Ziel ist
            if (trefferInfo.transform.CompareTag("Target"))
            {
                Debug.Log("Treffer mit " + controllerNode.ToString());
                Destroy(trefferInfo.transform.gameObject);
            }
        }
    }
}