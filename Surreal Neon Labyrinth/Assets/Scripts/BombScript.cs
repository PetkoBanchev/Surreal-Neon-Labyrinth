using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private PlayerScript playerScript;
    [SerializeField]
    private GameObject firePrefab;
    [SerializeField, Range(0, 10)]
    private float fireDuration;
    private bool isDetonated = false;
    AudioSource boom;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        boom = GameObject.FindGameObjectWithTag("Boom").GetComponent<AudioSource>();
        Debug.Log(boom.gameObject.name);
        StartCoroutine("BombDetonation");
    }

    IEnumerator BombDetonation()
    {
        yield return new WaitForSeconds(3f);
        boom.Play();
        yield return new WaitForSeconds(0.5f);
        DetonateBomb();
    }
    
    public void DetonateBomb()
    {
        if (!isDetonated)
        {
            isDetonated = true; // Prevents an infinte loop when detonation is triggered by another bomb
            for(var angle = 0f; angle < Mathf.PI * 2; angle += Mathf.PI / 2) // Gets the 4 angles in radians (0, 90, 180, 270 degrees)
            {
                var dir = new Vector3(Mathf.Cos(angle), transform.position.y - 1, Mathf.Sin(angle)); // gets a direction to shoot the fire at
                ShootFire(transform.position, dir, playerScript.GetBombRange(), angle);
            }
            DestroyBomb();
        }
    }

    private void ShootFire(Vector3 Position, Vector3 Direction, int BombRange, float Angle)
    {
        RaycastHit hit;
        int br = BombRange;
        var firePos = new Vector3(Position.x, Position.y, Position.z);
        var fireDir = new Vector3(Direction.x * 90, 90, Direction.z * 90);

        if (Physics.Raycast(Position, Direction, out hit, BombRange)) //checks for hits
        {
            if (hit.collider != null && !hit.collider.gameObject.CompareTag("Indestructible")) //excludes indestructible walls
            {
                GameObject fire = Instantiate(firePrefab, firePos, Quaternion.Euler(fireDir));
                int laserLength = Mathf.CeilToInt(Vector3.Distance(transform.position, hit.point));
                fire.transform.localScale = new Vector3(.5f, laserLength, .5f);
                Destroy(fire, fireDuration);
                FireHit(hit);
            }
        }
        else
        {
            GameObject fire = Instantiate(firePrefab, firePos, Quaternion.Euler(fireDir));
            fire.transform.localScale = new Vector3(.5f, br, .5f);
            Destroy(fire, fireDuration);
        }
        #region oldShootFire
        //for(int br = 0; br <= BombRange; br++)
        //{
        //    var firePos = new Vector3(Position.x + Mathf.Cos(Angle) * br, Position.y, Position.z + Mathf.Sin(Angle) * br);
        //    var fireDir = new Vector3(Direction.x * 90, 90, Direction.z * 90);

        //    if(Physics.Raycast(Position, Direction, out hit, BombRange)) //checks for hits
        //    {
        //        //Debug.DrawLine(Position, hit.point, Color.red, 20f);
        //        if (hit.collider != null && !hit.collider.gameObject.CompareTag("Indestructible")) //excludes indestructible walls
        //        {
        //            GameObject fire = Instantiate(firePrefab, firePos, Quaternion.Euler(fireDir));
        //            Destroy(fire, fireDuration);
        //            FireHit(hit);
        //            return; //Stops the loop on hit
        //        }
        //    }
        //    else
        //    {
        //        GameObject fire = Instantiate(firePrefab, firePos, Quaternion.Euler(fireDir));
        //        Destroy(fire, fireDuration);
        //    }
        //}
        #endregion
    }
    private void FireHit(RaycastHit h)
    {
        GameObject hit = h.collider.gameObject;
        if (hit.CompareTag("Destructible"))
        {
            Destroy(hit);
        }
        if (hit.CompareTag("Bomb"))
        {
            hit.GetComponent<BombScript>().DetonateBomb();
        }
        if (hit.CompareTag("Enemy"))
        {
            hit.GetComponent<EnemyScript>().KillEnemy();
        }
    }

    private void DestroyBomb()
    {
        if (playerScript.GetCurrentNumberOfBombs() < playerScript.GetMaxNumberOfBombs())
        {
            playerScript.currentNumberOfBombs++; // refills the player's bombs
        }

        Destroy(gameObject);
    }
}
