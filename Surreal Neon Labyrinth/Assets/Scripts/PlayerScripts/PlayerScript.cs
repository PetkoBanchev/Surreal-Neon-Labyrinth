using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public CharacterController Controller;
    public MazeManager MazeManager;
    public GameManagerCustom GMC;



    public float speed = 5f;
    public int bombRange = 1;
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
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && currentNumberOfBombs > 0)
        {
            PlaceBomb();
        }

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
            GameObject.FindObjectOfType<MazeGenerator>().ChangeExitMaterial();
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
