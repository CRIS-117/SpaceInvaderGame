using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int healthMax;
    [SerializeField] int health;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)]float deathSFXVolume;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minX, maxX, minY, maxY, padding;
    [SerializeField] GameObject[] propulsionFire;

    [Header("Fire")]
    [SerializeField] float fireRate;
    [SerializeField] PlayerProyectile proyectilePrefab;
    [SerializeField] FireSpark[] fireSpark;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0,1)]float shootSFXVolume;

    GameManager gameManager;

    Coroutine fireCoroutine;

    bool isReady;
    bool isInvinsible;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        isInvinsible = true;
        StartCoroutine(RemoveInvicibility());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReady)
        {
            GetReady();
        }
        else
        {
            Move();
            Fire();
            PropulsionControl();
        }
    }

    void GetReady()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -6.5f), 5 * Time.deltaTime);
        if(transform.position.y == -6.5f)
        {
            isReady = true;
        }
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float deltaY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float newPosX = Mathf.Clamp(transform.position.x + deltaX, minX + padding, maxX - padding);
        float newPosY = Mathf.Clamp(transform.position.y + deltaY, minY + padding, maxY - padding);
        transform.position = new Vector2(newPosX,newPosY);
    }

    void PropulsionControl()
    {
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > .1f || Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            propulsionFire[0].SetActive(true);
            propulsionFire[1].SetActive(true);
        }
        else
        {
            propulsionFire[0].SetActive(false);
            propulsionFire[1].SetActive(false);
        }
    }

    void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireCoroutine = StartCoroutine(FireContinuosly());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            if(fireCoroutine != null)
            StopCoroutine(fireCoroutine);
        }
    }

    private void ShootProjectile(Vector2 projectilePosition)

    {
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position,shootSFXVolume);
        var newProjectile = Instantiate(proyectilePrefab, projectilePosition, Quaternion.identity);
        newProjectile.transform.SetParent(FindObjectOfType<Instances>().projectiles);
    }

    public void ProcessHit(int damage)
    {
        if(!isInvinsible)
        {
            health -= damage;
            FindObjectOfType<GameHUD>().UpdateHealthBar(healthMax, health);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        gameManager.ProcessDeath();
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position,deathSFXVolume);
        var newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newExplosion.transform.SetParent(FindObjectOfType<Instances>().explosions);
        Destroy(gameObject);
    }

    IEnumerator FireContinuosly()
    {
        fireSpark[0].ShowSpark();
        fireSpark[1].ShowSpark(); 
        ShootProjectile(fireSpark[0].transform.position);
        ShootProjectile(fireSpark[1].transform.position);
        yield return new WaitForSeconds(fireRate);
        fireCoroutine = StartCoroutine(FireContinuosly());
    }

    IEnumerator RemoveInvicibility()
    {
        yield return new WaitForSeconds(3f);
        Destroy(GetComponent<Blinker>());
        GetComponent<SpriteRenderer>().color = Color.white;
        isInvinsible = false;

    }
}
