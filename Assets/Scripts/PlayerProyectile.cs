using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectile : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0,1)]float hitSFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaY = moveSpeed * Time.deltaTime;
        float newPosY = transform.position.y + deltaY;
        transform.position = new Vector2(transform.position.x, newPosY);
        if(transform.position.y > 10.6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D enemy)
    {
        AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitSFXVolume);
        var newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        newExplosion.transform.SetParent(FindObjectOfType<Instances>().explosions);
        enemy.GetComponent <Destroyable>().ProcessHit(damage);
        Destroy(gameObject);
    }
}
