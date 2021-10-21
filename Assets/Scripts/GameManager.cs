using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(0,5)] float gameSpeed;
    [SerializeField] PlayerShip playerShipPrefab;
    [SerializeField] int lives;

    GameHUD gameHUD;

    int currentScore;

    void Start()
    {
        gameHUD = FindObjectOfType<GameHUD>();
        gameHUD.UpdateLives(lives);
        gameHUD.UpdateScore(currentScore);

    }

    // Update is called once per frame
    void Update()
    {
       Time.timeScale = gameSpeed; 
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddToScore(int points)
    {
        currentScore += points;
        gameHUD.UpdateScore(currentScore);
    }

     public void ProcessDeath()
    {
        if(lives > 0)
        {
            lives--;
            gameHUD.UpdateLives(lives);
            gameHUD.UpdateHealthBar(1,1);
            Instantiate(playerShipPrefab, new Vector2(0, -12f), Quaternion.identity);
        }
        else
        {
            //Game Over
        }
    }
}
