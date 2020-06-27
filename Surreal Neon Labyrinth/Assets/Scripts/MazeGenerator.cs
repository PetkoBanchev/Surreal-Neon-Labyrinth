using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //Maze Elements
    [SerializeField]
    private GameObject IndCube;
    [SerializeField]
    private GameObject DesCube;
    [SerializeField]
    private GameObject FloorTile;
    [SerializeField]
    private GameObject Wall;
    [SerializeField]
    private GameObject Exit;
    [SerializeField]
    private GameObject Roof;
    [SerializeField]
    private Material exitOpened;
    private GameObject _exit;

    private MazeManager mazeManager;

    void Start()
    {
        mazeManager = GameObject.FindObjectOfType<MazeManager>();
        InstantiateMaze();
    }

    public void InstantiateMaze()
    {
        if(mazeManager != null)
        {
            foreach (Node n in mazeManager.maze)
            {
                switch (n.state)
                {
                    case 0: //Unwalkable
                        Instantiate(IndCube, new Vector3(n.worldPosition.x, 1.5f, n.worldPosition.z), Quaternion.identity, this.transform);
                        break;
                    case 1: //Obstructed
                        Instantiate(FloorTile, n.worldPosition, Quaternion.identity, this.transform);
                        Instantiate(DesCube, new Vector3(n.worldPosition.x, 1.5f, n.worldPosition.z), Quaternion.identity, this.transform);
                        break;
                    case 2: //Walkable
                        Instantiate(FloorTile, n.worldPosition, Quaternion.identity, this.transform);
                        break;
                }
            }
            CreateWalls();
            CreateExit();
        }
        
    }

    private void CreateExit()
    {
        Node n = mazeManager.GetRandomObstructedNode();
        _exit = Instantiate(Exit, n.worldPosition, Quaternion.identity, this.transform);
    }

    public void ChangeExitMaterial()
    {
        _exit.GetComponent<MeshRenderer>().material = exitOpened;
    }
    private void CreateWalls()
    {
        float x = mazeManager.maze.GetLength(0);
        float z = mazeManager.maze.GetLength(1);
        float centerX = (x - 1) / 2;
        float centerZ = (z - 1) / 2;

        var w1 = Instantiate(Wall, new Vector3(-0.5f, 1.5f, centerZ), Quaternion.Euler(0, 0, 0),this.transform);
        w1.transform.localScale = new Vector3(0.001f, 3, z);
        var w2 = Instantiate(Wall, new Vector3(x - 0.5f, 1.5f, centerZ), Quaternion.Euler(0, 0, 0), this.transform);
        w2.transform.localScale = new Vector3(0.001f, 3, z);
        var w3 = Instantiate(Wall, new Vector3(centerX, 1.5f, - 0.5f), Quaternion.Euler(0, 90, 0), this.transform);
        w3.transform.localScale = new Vector3(0.001f, 3, x);
        var w4 = Instantiate(Wall, new Vector3(centerX, 1.5f, z - 0.5f), Quaternion.Euler(0, 90, 0), this.transform);
        w4.transform.localScale = new Vector3(0.001f, 3, x);

        var roof = Instantiate(Roof, new Vector3(centerX,3.1f,centerZ), Quaternion.identity, this.transform);
        roof.transform.localScale = new Vector3(x, .1f, z);
    }
}
