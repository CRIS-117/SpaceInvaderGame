using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] float moveSpeedHorizontalMax;
    [SerializeField] float moveSpeedVertical1;
    [SerializeField] float moveSpeedVertical2;
    [SerializeField] float chasePosition;

    float moveSpeedHorizontal, moveSpeedVertical;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeedVertical = moveSpeedVertical1;
    }

    // Update is called once per frame
    void Update()
    {
        SetSpeed();
        Move();
        
    }

    private void Move()
    {
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

    private void SetSpeed()
    {
        if (transform.position.y < chasePosition)
        {
            moveSpeedVertical = moveSpeedVertical2;
            if(FindObjectOfType<PlayerShip>())
            {
                PlayerShip player = FindObjectOfType<PlayerShip>();
                float playerDistance = player.transform.position.x - transform.position.x;
                moveSpeedHorizontal = Mathf.Clamp(playerDistance, moveSpeedHorizontalMax * -1, moveSpeedHorizontalMax);
            }
            else
            {
                moveSpeedHorizontal = 0;
            }
        }
    } 
}
