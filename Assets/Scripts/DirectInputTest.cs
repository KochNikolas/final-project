using UnityEngine;
using UnityEngine.XR; // Nötig für den Zugriff auf den Controller

public class DirectInputTest : MonoBehaviour
{
    // Speichert, ob der Knopf im letzten Frame schon gedrückt war
    private bool warGedrueckt = false;

    void Update()
    {
        // 1. Wir holen uns das Gerät "Rechte Hand"
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (device.isValid)
        {
            bool istGedrueckt;

            // 2. Prüfen: Ist der "Trigger" (Zeigefinger) gedrückt?
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out istGedrueckt) && istGedrueckt)
            {
                // Nur ausführen, wenn wir gerade frisch draufdrücken (nicht gedrückt halten)
                if (!warGedrueckt)
                {
                    // A) Konsolenausgabe (sichtbar im Android Logcat)
                    Debug.Log("TREFFER! Der Würfel wurde zerstört.");

                    // B) Würfel zerstören (Er verschwindet aus der Szene)
                    Destroy(gameObject); 
                    
                    // (Hinweis: Nach Destroy läuft dieses Skript nicht mehr weiter, 
                    // weil es zusammen mit dem Würfel weg ist. Das ist genau richtig so.)
                }
                
                // Wir merken uns, dass gedrückt ist
                warGedrueckt = true;
            }
            else
            {
                // Wenn losgelassen wird, setzen wir den Status zurück
                warGedrueckt = false;
            }
        }
    }
}