using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    // public GameObject obstaclePrefab;

    public void BeginSpawning()
    {
        InvokeRepeating(nameof(Spawn), 0, 2f);
    }

    void Spawn()
    {
        // 1.18 stop moving left when the game is over
        GameObject player = GameObject.Find("Player");
        bool isGameOver = player.GetComponent<PlayerController>().gameOver;
        if (isGameOver)
        {
            return;
        }

        int randomType = Random.Range(0, 3);
        var obstacle = ObstacleObjectPool.instance.Acquire(randomType);
        obstacle.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);

        StartCoroutine(ReleaseAfterTime(obstacle, randomType, 5f));
    }

    private IEnumerator ReleaseAfterTime(GameObject obstacle, int obstacleType, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObstacleObjectPool.instance.Release(obstacle, obstacleType);
    }
}
