using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;
    private MazeManager mazeManager;

    public List<GameObject> Enemies;
    public int numberOfEnemies;

    public bool IsKeyDropped = false;

    void Start()
    {
        mazeManager = GameObject.FindObjectOfType<MazeManager>();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Enemies = new List<GameObject>();
        for(int i = 0; i < numberOfEnemies; i++)
        {
            var e = Instantiate(EnemyPrefab, GetSpawnPosition(), Quaternion.identity);
            Enemies.Add(e);
        }
      
    }

    private Vector3 GetSpawnPosition()
    {
        Node n = mazeManager.GetRandomWalkableNode();
        n.worldPosition.y = 1;
        return n.worldPosition;
    }

    public void DestroyAllEnemie()
    {
        foreach(GameObject e in Enemies)
        {
            Destroy(e);
        }
    }
}
