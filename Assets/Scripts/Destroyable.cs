using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int healthMax;
    [SerializeField] int damage; 
    [SerializeField] GameObject explosionPrefab;

    [Header("SFX")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)]float deathSFXVolume;

    public void ProcessHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<GameManager>().AddToScore(100);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position,deathSFXVolume);
        var newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newExplosion.transform.SetParent(FindObjectOfType<Instances>().explosions);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerShip>())
        {
            other.GetComponent<PlayerShip>().ProcessHit(damage);
            Die();
        }
    }
}
