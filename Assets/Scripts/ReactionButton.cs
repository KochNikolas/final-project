using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReactionButton : MonoBehaviour
{
    public ReactionManager reactionManager;

    public void OnPressed()
    {
        reactionManager.StopTimer();
        Destroy(gameObject);
    }
}