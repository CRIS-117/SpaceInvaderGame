 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeedVertical;
    string directionHorizontal;
    float moveSpeedHorizontal;

    [Header("Fire")]
    [SerializeField] EnemyProyectile1 projectilePrefab;
    [SerializeField] float minTimeBetweenShots, maxTimeBetweenShots;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0,1)]float shootSFXVolume;

    void Start()
    {
        switch(Random.Range(1,3))
        {
            case 1:directionHorizontal = "Left"; moveSpeedHorizontal = 6; break;
            case 2:directionHorizontal = "Right"; moveSpeedHorizontal = -6; break;
        }
        StartCoroutine(ChangeDirection());
        StartCoroutine(FireContinuosly());
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (directionHorizontal)
        {
            case "Left": moveSpeedHorizontal -= 24f * Time.deltaTime; break;
            case "Right": moveSpeedHorizontal += 24f * Time.deltaTime; break;
        }
        moveSpeedHorizontal = Mathf.Clamp(moveSpeedHorizontal, -6f, 6f);
        float deltaX = moveSpeedHorizontal * Time.deltaTime;
        float deltaY = moveSpeedVertical * Time.deltaTime;
        float newPositionX = transform.position.x + deltaX;
        float newPositionY = transform.position.y - deltaY;
        transform.position = new Vector2(newPositionX, newPositionY);
        if(transform.position.y < -11f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(1f);
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
        var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        newProjectile.transform.SetParent(FindObjectOfType<Instances>().projectiles);
        StartCoroutine(FireContinuosly());
    }
}
 