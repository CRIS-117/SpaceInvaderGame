﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectile1 : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0,1)]float hitSFXVolume;

    // Update is called once per frame
    void Update()
    {
        float deltaY = moveSpeed * Time.deltaTime;
        float newPosY = transform.position.y + (deltaY * -1f);
        transform.position = new Vector2(transform.position.x, newPosY);
        if(transform.position.y < -10.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitSFXVolume);
        var newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newExplosion.transform.SetParent(FindObjectOfType<Instances>().explosions);
        player.GetComponent<PlayerShip>().ProcessHit(damage);
        Destroy(gameObject);
    }
}