using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private EnemyManager enemyManager;
    public float moveForce = 0f;
    private Rigidbody rb;
    public Vector3 moveDir;
    public LayerMask obstacle;
    public float maxDistanceFromObstacle = 0f;
    private bool IsRotationFrozen = false;
    [SerializeField]
    private GameObject KeyPrefab;
    public AudioClip yay;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        rb = GetComponent<Rigidbody>();
        moveDir = ChooseRandomDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDir * moveForce;
        int rnd = Random.Range(0, 100);
        if (rnd == 50 && !IsRotationFrozen)
        {
            moveDir = ChooseRandomDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
            IsRotationFrozen = true;
            StartCoroutine("FreezeDirectionChange");
        }
        if (Physics.Raycast(transform.position, transform.forward, maxDistanceFromObstacle, obstacle))
        {
            moveDir = ChooseRandomDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
        if (!Physics.BoxCast(transform.position, Vector3.one*0.35f,transform.right, Quaternion.identity,maxDistanceFromObstacle+2, obstacle) && rnd <25 && !IsRotationFrozen)
        {
            moveDir = transform.right;
            transform.rotation = Quaternion.LookRotation(moveDir);
            IsRotationFrozen = true;
            StartCoroutine("FreezeDirectionChange");
        }
        if (!Physics.BoxCast(transform.position, Vector3.one * 0.35f, -transform.right, Quaternion.identity, maxDistanceFromObstacle + 2, obstacle) && rnd > 75 && !IsRotationFrozen)
        {
            moveDir = -transform.right;
            transform.rotation = Quaternion.LookRotation(moveDir);
            IsRotationFrozen = true;
            StartCoroutine("FreezeDirectionChange");
        }
    }

    IEnumerator FreezeDirectionChange()
    {
        yield return new WaitForSeconds(5);
        IsRotationFrozen = false;

    }
    Vector3 ChooseRandomDirection()
    {
        int i = Random.Range(0, 3);
        Vector3 dir = new Vector3();
        switch (i)
        {
            case 0:
                dir = transform.forward;
                break;
            case 1:
                dir = -transform.forward;
                break;
            case 2:
                dir = transform.right;
                break;
            case 3:
                dir = -transform.right;
                break;
        }

        return dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            KillEnemy();
        }
    }
    public void KillEnemy()
    {
        AudioSource boom = GameObject.FindGameObjectWithTag("Boom").GetComponent<AudioSource>();
        boom.PlayOneShot(yay);
        if (!enemyManager.IsKeyDropped)
        {
            int rnd = Random.Range(0, 100);
            if(enemyManager.Enemies.Count == 1)
            {
                Instantiate(KeyPrefab, gameObject.transform.position, Quaternion.identity);
            }
            else if (rnd < 20)
            {
                Instantiate(KeyPrefab, gameObject.transform.position, Quaternion.identity);
                enemyManager.IsKeyDropped = true;
            }
        }
        enemyManager.Enemies.Remove(this.gameObject);
        
        Destroy(this.gameObject);
    }
}
