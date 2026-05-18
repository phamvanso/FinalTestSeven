using UnityEngine;

public class AutoKickManager : MonoBehaviour
{
    public Transform player;

    public void AutoKick()
    {
        BallKick[] balls = FindObjectsOfType<BallKick>();

        BallKick farthestBall = null;

        float maxDistance = 0;

        foreach (BallKick ball in balls)
        {
            float distance = Vector3.Distance(
                player.position,
                ball.transform.position
            );

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestBall = ball;
            }
        }

        if (farthestBall != null)
        {
            //Debug.Log("farthestBall");
            farthestBall.KickBall();
        }
    }
}