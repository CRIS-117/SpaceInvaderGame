using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    
    [Header("Move")]
    [SerializeField] float moveSpeedVertical;
    [SerializeField] float boundaryLeft, boundaryRight, padding;
    string directionHorizontal;
    float moveSpeedHorizontal;

    [Header("Fire")]
    [SerializeField] GameObject arrow;
    [SerializeField] EnemyProyectile2 projectilePrefab;
    [SerializeField] float minTimeBetweenShots, maxTimeBetweenShots;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0,1)]float shootSFXVolume;

    void Start()
    {
       switch(Random.Range(1,3))
        {
            case 1:directionHorizontal = "Left"; moveSpeedHorizontal = 2; break;
            case 2:directionHorizontal = "Right"; moveSpeedHorizontal = -2; break;
        }
        StartCoroutine(ChangeDirection());
        StartCoroutine(FireContinuosly()); 
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Aim();
    }

    void Move()
    {
        switch (directionHorizontal)
        {
            case "Left": moveSpeedHorizontal -= 12f * Time.deltaTime; break;
            case "Right": moveSpeedHorizontal += 12f * Time.deltaTime; break;
        }
        moveSpeedHorizontal = Mathf.Clamp(moveSpeedHorizontal, -2f, 2f);
        float deltaX = moveSpeedHorizontal * Time.deltaTime;
        float deltaY = moveSpeedVertical * Time.deltaTime;
        float newPositionX = Mathf.Clamp(transform.position.x + deltaX, boundaryLeft + padding, boundaryRight - padding);
        float newPositionY = transform.position.y - deltaY;
        transform.position = new Vector2(newPositionX, newPositionY);
        if(transform.position.y < -11f)
        {
            Destroy(gameObject);
        }
    }

    void Aim()
    {
        if(FindObjectOfType<PlayerShip>())
        {
        PlayerShip player = FindObjectOfType<PlayerShip>();
        Vector3 PlayerPosition = player.transform.position;
        Vector3 enemyPosition = transform.position;
        Vector3 lookAtVector = PlayerPosition - enemyPosition;
        float angle = Mathf.Atan2(lookAtVector.x, lookAtVector.y) * Mathf.Rad2Deg;
        arrow.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        switch(directionHorizontal)
        {
            case "Right": directionHorizontal = "Left"; break;
            case "Left": directionHorizontal  = "Right"; break;
        }
        StartCoroutine(ChangeDirection());
        
    }

    IEnumerator FireContinuosly()
    {
        yield return new WaitForSeconds(Random.Range(minTimeBetweenShots, maxTimeBetweenShots));
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);

        if(FindObjectOfType<PlayerShip>())
        {
            var newProjectile = Instantiate(projectilePrefab, transform.position, arrow.transform.localRotation);
            newProjectile.transform.SetParent(FindObjectOfType<Instances>().projectiles);
            StartCoroutine(FireContinuosly()); 
        }  
        
    }
}
