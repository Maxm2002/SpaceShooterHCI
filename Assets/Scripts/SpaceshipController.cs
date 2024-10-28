using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject laserPrefab; // Hier referenzierst du das Laserobjekt
    public float laserSpeed = 10f; // Geschwindigkeit des Lasers
    public bool canShoot = true;   // Status, ob geschossen werden kann
    public float laserOffset = 0.5f; // Versatz für die beiden Laser
    private float lastShotTime;

    public int lives = 4; // Anzahl der Leben
    public GameObject explosionPrefab; // Optional: Ein Explosionseffekt

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn die linke Maustaste gedrückt wird und der Spieler schießen darf
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            //ShootDoubleLaser();
        }
    }

    public void ShootDoubleLaser()
    {
        /*// Mausposition erfassen
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Berechne die Richtung, in die geschossen werden soll
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Positionen für die beiden Laser berechnen
        Vector3 leftLaserPosition = transform.position + Vector3.left * laserOffset;
        Vector3 rightLaserPosition = transform.position + Vector3.right * laserOffset;

        // Linken Laser instanziieren und bewegen
        GameObject leftLaser = Instantiate(laserPrefab, leftLaserPosition, Quaternion.identity);
        leftLaser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;

        // Rechten Laser instanziieren und bewegen
        GameObject rightLaser = Instantiate(laserPrefab, rightLaserPosition, Quaternion.identity);
        rightLaser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            GameManager.instance.LoseLife(); // Spieler verliert ein Leben bei Kollision mit einem Asteroiden
            Destroy(collision.gameObject);   // Asteroid wird zerstört
        }

        if (collision.gameObject.CompareTag("Satalite"))
        {
            Debug.Log("Satalite passiert das Raumschiff.");
            // Der Satellit passiert das Raumschiff und wird nicht zerstört
        }
    }
    void ResetShoot()
    {
        canShoot = true; // Nach 2 Sekunden kann wieder geschossen werden
    }

}
