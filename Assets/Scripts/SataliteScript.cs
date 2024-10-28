using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SataliteScript : MonoBehaviour
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
            GameManager.instance.AddScore(-100); // Spieler verliert 100 Punkte für einen Satelliten
            Destroy(other.gameObject); // Laser zerstören
            DestroyObject(); // Zerstöre den Satelliten
        }
    }

    void DestroyObject()
    {
        if (eyeTrackingController != null)
        {
            eyeTrackingController.RemoveInteractableObject(gameObject);
        }
        Destroy(gameObject); // Zerstört den Satelliten
        //Debug.Log("Stalite wurde zerstört!");
    }
}
