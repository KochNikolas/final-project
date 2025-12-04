using UnityEngine;

public class RestartButtonScript : MonoBehaviour
{
    public ReactionManager reactionManager;

    public void OnRestartPressed()
    {
        reactionManager.StartTrial();
    }
}
