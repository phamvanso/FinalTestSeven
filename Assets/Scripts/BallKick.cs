using System.Collections;
using UnityEngine;

public class BallKick : MonoBehaviour
{
    [Header("Ball Settings")]
    public float flySpeed = 15f;

    [Header("Effect")]
    public ParticleSystem confetti;

    private bool isFlying = false;

    public void KickBall()
    {
        if (!isFlying)
        {
            StartCoroutine(FlyToNearestGoal());
        }
    }

    IEnumerator FlyToNearestGoal()
    {
        isFlying = true;

        // Disable player movement
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerMovement playerMovement = null;

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.canMove = false;
            }
        }

        // Find nearest goal
        Transform nearestGoal = FindNearestGoal();

        if (nearestGoal == null)
        {
            yield break;
        }

        // Camera follow ball
        CameraFollow.Instance.SetTarget(transform);

        // Keep ball on same Y
        Vector3 targetPos = new Vector3(
            nearestGoal.position.x,
            transform.position.y,
            nearestGoal.position.z
        );

        // Move ball
        while (Vector3.Distance(transform.position, targetPos) > 0.2f)
        {
            Vector3 nextPos = Vector3.MoveTowards(
                transform.position,
                targetPos,
                flySpeed * Time.deltaTime
            );

            nextPos.y = transform.position.y;

            transform.position = nextPos;

            yield return null;
        }

        // Snap to goal
        transform.position = targetPos;

        // Play effect
        if (confetti != null)
        {
            Debug.Log("PLAY CONFETTI");
            confetti.transform.position =
                transform.position + Vector3.up * 2f;

            confetti.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            confetti.Play();
        }

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Camera back to player
        if (player != null)
        {
            CameraFollow.Instance.SetTarget(player.transform);

            if (playerMovement != null)
            {
                playerMovement.canMove = true;
            }
        }

        // Destroy ball
        Destroy(gameObject);
    }

    Transform FindNearestGoal()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        Transform nearestGoal = null;

        float minDistance = Mathf.Infinity;

        foreach (GameObject goal in goals)
        {
            float distance = Vector3.Distance(
                transform.position,
                goal.transform.position
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestGoal = goal.transform;
            }
        }

        return nearestGoal;
    }
}