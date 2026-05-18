using UnityEngine;

public class PlayerKickDetector : MonoBehaviour
{
    public GameObject kickButton;

    public float detectDistance = 1f;

    private BallKick currentBall;

    void Update()
    {
        BallKick[] balls = FindObjectsOfType<BallKick>();

        currentBall = null;

        foreach (BallKick ball in balls)
        {
            float distance = Vector3.Distance(
                transform.position,
                ball.transform.position
            );

            if (distance <= detectDistance)
            {
                currentBall = ball;
                break;
            }
        }

        kickButton.SetActive(currentBall != null);
    }

    public void Kick()
    {
        if (currentBall != null)
        {
            //Debug.Log("currentBall");
            currentBall.KickBall();
        }
    }
}