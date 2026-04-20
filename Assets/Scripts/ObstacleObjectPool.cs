using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    public GameObject obstacleBarrelPrefab;
    public GameObject obstacleBarrierPrefab;
    public GameObject obstacleStoneWallPrefab;
    public int poolSize = 10;

    [SerializeField] private int initialPoolSize = 15;
    private List<GameObject> obstacleBarrelPool;
    private List<GameObject> obstacleBarrierPool;
    private List<GameObject> obstacleStoneWallPool;

    public static ObstacleObjectPool instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        obstacleBarrelPool = new List<GameObject>();
        obstacleBarrierPool = new List<GameObject>();
        obstacleStoneWallPool = new List<GameObject>();
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateObstacle(obstacleBarrelPrefab, obstacleBarrelPool);
            CreateObstacle(obstacleBarrierPrefab, obstacleBarrierPool);
            CreateObstacle(obstacleStoneWallPrefab, obstacleStoneWallPool);
            if (i % 20 == 0)
            {
                yield return null;
            }
        }

        FindFirstObjectByType<SpawnManager>().BeginSpawning();
    }

    private void CreateObstacle(GameObject prefab, List<GameObject> pool)
    {
        var go = Instantiate(prefab);
        go.SetActive(false);
        pool.Add(go);
    }

    public GameObject Acquire(int obstacleType)
    {
        var go = GetPool(obstacleType)[0];
        GetPool(obstacleType).RemoveAt(0);
        go.SetActive(true);
        return go;
    }

    public void Release(GameObject obstacle, int obstacleType)
    {
        GetPool(obstacleType).Add(obstacle);
        obstacle.SetActive(false);
    }

    private List<GameObject> GetPool(int obstacleType)
    {
        return obstacleType switch
        {
            0 => obstacleBarrelPool,
            1 => obstacleBarrierPool,
            2 => obstacleStoneWallPool,
            _ => null
        };
    }

    private GameObject GetPrefab(int obstacleType)
    {
        return obstacleType switch
        {
            0 => obstacleBarrelPrefab,
            1 => obstacleBarrierPrefab,
            2 => obstacleStoneWallPrefab,
            _ => null
        };
    }
}
