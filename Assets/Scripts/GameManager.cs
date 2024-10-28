using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton für globalen Zugriff
    public int score = 0;               // Punktestand
    public int lives = 4;               // Anzahl der Leben

    public GameObject spaceShip;
    public Text scoreText;              // UI-Text für Punkte
    public Text statusText;             // UI-Text für Status
    public GameObject explosionPrefab; // Optional: Ein Explosionseffekt

    void Awake()
    {
        // Singleton-Pattern, um eine einzige Instanz des GameManagers zu haben
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI(); // Initiale UI-Aktualisierung
    }

    // Methode, um den Punktestand zu ändern
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // Methode, um Leben zu reduzieren
    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // Aktualisiert die UI
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        statusText.text = "Lives: " + lives;
    }

    // Methode für Spielende
    void GameOver()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        //Debug.Log("Raumschiff zerstört!");
        Destroy(spaceShip); // Raumschiff zerstören
        //Debug.Log("Game Over!"); // Hier kannst du das Spielende implementieren
        // Optionale Logik für Spielende, wie z.B. eine Spiel-neu-starten-Schaltfläche
    }
}
