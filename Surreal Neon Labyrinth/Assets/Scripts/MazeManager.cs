using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    
    //MazeProperties
    [SerializeField, ]
    private int width;  //It's advisable to be uneven, to ensure there is 1 block space between the end of the maze and the last indestructible cube
    [SerializeField]
    private int height;
    [Range(0, 100)]
    public int desCubeSpawnRate;

    //Pathfinding
    public Node[,] maze;
    public List<Node> walkableNodes;
    public List<Node> obstructedNodes;

    [SerializeField]
    private GameObject PlayerPrefab;
    public bool IsTest;
    //[SerializeField]
    //private MazeGenerator mazeGenerator;
    //[SerializeField]
    //private EnemyManager enemyManager;

    // Start is called before the first frame update
    private void Awake()
    {
        GenerateMaze(width, height);
    }

    private void GenerateMaze(int width, int height)
    {
        maze = new Node[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z< height; z++)
            {
                Vector3 _worldPosition = new Vector3(x,0,z);
                int state = 2; //walkable
                if (x % 2 == 1 && z % 2 == 1) //When uneven spawns indestructible cubes
                {
                    state = 0;//unwalkable
                }
                if ((x % 2 == 0 || z % 2 == 0) && (UnityEngine.Random.Range(0, 100) < desCubeSpawnRate)) //When one axis is even spawns a destructible cube. It is dependant on the spawn rate
                {
                    if ((x==0&&z==0) || (x == 1 && z == 0) || (x == 0 && z == 1)) // Ensures that the corner where the player spawns is clear
                    {
                        state = 2; //walkable
                    }
                    else
                    {
                        state = 1;//obstructed
                    }
                }
                maze[x, z] = new Node(_worldPosition, state);
            }  
        }
        PopulateNodeLists();
        if (!IsTest)
        {
            Instantiate(PlayerPrefab, new Vector3(0,1,0), Quaternion.identity);
        }
    }

    private void PopulateNodeLists()
    {
        walkableNodes = new List<Node>();
        obstructedNodes = new List<Node>();
        foreach(Node n in maze)
        {
            if(n.state == 2)
            {
                walkableNodes.Add(n);
            }
            else if (n.state == 1)
            {
                obstructedNodes.Add(n);
            }
            else
            {

            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 _worldPosition)
    {
        int x = Mathf.RoundToInt(_worldPosition.x);
        int z = Mathf.RoundToInt(_worldPosition.z);
        return maze[x, z];
    }

    public Node GetRandomNode()
    {
        int values = maze.GetLength(0) * maze.GetLength(1);
        int index = Random.Range(0, values);
        return maze[index / maze.GetLength(0), index % maze.GetLength(0)];
    }

    public Node GetRandomWalkableNode()
    {
        int index = Random.Range(0, walkableNodes.Count);
        return walkableNodes[index];
    }

    public Node GetRandomObstructedNode()
    {
        int index = Random.Range(0, obstructedNodes.Count);
        return obstructedNodes[index];
    }

    //public void DestroyMaze()
    //{
    //    maze = new Node[0,0];
    //    Destroy(MG.gameObject);
    //    Destroy(Player.gameObject);
    //}

    //void OnDrawGizmos()
    //{
    //    if (maze != null)
    //    {
    //        foreach (Node n in maze)
    //        {
    //            switch (n.state)
    //            {
    //                case 0:
    //                    Gizmos.color = Color.red;
    //                    break;
    //                case 1:
    //                    Gizmos.color = Color.gray;
    //                    break;
    //                case 2:
    //                    Gizmos.color = Color.white;
    //                    break;
    //            }
    //            Gizmos.DrawCube(n.worldPosition, new Vector3(0.8f, 0.8f, 0.8f));
    //        }
    //    }
    //}

}
