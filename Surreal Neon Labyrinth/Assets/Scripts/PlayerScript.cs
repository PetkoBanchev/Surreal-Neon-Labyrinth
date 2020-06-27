using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public CharacterController Controller;
    public MazeManager MazeManager;

    public float speed = 5f;
    public float gravity = 9.8f;
    private float vForce = 0; // current vertical speed
    public int bombRange = 3;
    public int maxNumberOfBombs = 1;
    public int currentNumberOfBombs;
    public GameObject Bomb;

    private Canvas canvas;
    private GameObject gameOverScreen;
    [SerializeField]
    private bool hasKey = false;

    private AudioSource sound;
    public AudioClip dead;
    public AudioClip win;

    public bool IsTest;
    public Material exitOpened;
    // Start is called before the first frame update
    void Start()
    {
        currentNumberOfBombs = maxNumberOfBombs;
        MazeManager = GameObject.FindObjectOfType<MazeManager>();
        canvas = GameObject.FindObjectOfType<Canvas>();
        gameOverScreen = canvas.GetComponentInChildren<Image>().gameObject;
        gameOverScreen.SetActive(false);
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        MovePlayer(x, z);

        if (Input.GetKeyDown(KeyCode.E) && currentNumberOfBombs > 0)
        {
            PlaceBomb();
        }

    }

    private void MovePlayer(float x, float z)
    {
        Vector3 move = transform.right * x +  transform.forward * z;
        vForce -= gravity * Time.deltaTime;
        move.y = vForce;

        Controller.Move(move * speed * Time.deltaTime);
    }

    private void PlaceBomb()
    {
        GameObject bomb = Instantiate(Bomb, GetBombPosition(), Quaternion.identity);
        currentNumberOfBombs--;
    }

    private Vector3 GetBombPosition()
    {
        Vector3 bombPos = MazeManager.GetNodeFromWorldPosition(this.transform.position).worldPosition;
        bombPos.y = 1;
        return bombPos;
    }

    public void KillPlayer()
    {
        sound.PlayOneShot(dead);
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponentInChildren<Text>().text = "GAME OVER";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        speed = 0;
    }

    private void Win()
    {
        sound.PlayOneShot(win);
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponentInChildren<Text>().text = "YOU WON";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        speed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fire")
        {
            KillPlayer();
        }
        if (other.tag == "Enemy")
        {
            KillPlayer();
        }
        if (other.tag == "Key")
        {
            hasKey = true;
            if (!IsTest)
            {
                GameObject.FindObjectOfType<MazeGenerator>().ChangeExitMaterial();
            }
            else
            {
                GameObject.FindGameObjectWithTag("Exit").GetComponent<MeshRenderer>().material = exitOpened;
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "Exit" && hasKey)
        {
            Win();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bomb")
        {
            other.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    public int GetBombRange()
    {
        return bombRange;
    }

    public int GetCurrentNumberOfBombs()
    {
        return currentNumberOfBombs;
    }

    public int GetMaxNumberOfBombs()
    {
        return maxNumberOfBombs;
    }
}
