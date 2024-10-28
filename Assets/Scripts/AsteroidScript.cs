using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    private EyetrackerController eyeTrackingController;
    // Start is called before the first frame update
    void Start()
    {
        eyeTrackingController = FindObjectOfType<EyetrackerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject); // Zerstört das GameObject
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Überprüfen, ob das Objekt vom Laser getroffen wurde
        if (other.CompareTag("Laser"))
        {
            GameManager.instance.AddScore(20); // Spieler bekommt 20 Punkte für einen Asteroiden
            Destroy(other.gameObject); // Laser zerstören
            DestroyObject(); // Zerstöre den Asteroiden
        }
    }

    void DestroyObject()
    {
        if (eyeTrackingController != null)
        {
            eyeTrackingController.RemoveInteractableObject(gameObject);
        }
        Destroy(gameObject); // Zerstört den Asteroiden
        //Debug.Log("Asteroid wurde zerstört!");
    }
}
