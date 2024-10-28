using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class SpaceController3 : MonoBehaviour
{
    public GameObject laserPrefab;        // Prefab für den Laserstrahl
    public float cooldownDuration = 100f;   // Cooldown-Zeit zwischen den Schüssen
    private float cooldownTimer;
    public float laserSpeed = 10f; // Geschwindigkeit des Lasers
    public RectTransform crosshairUI;
    private float gazeTimer = 0f;             // Timer für den Blick auf das aktuelle Ziel
    public float laserOffset = 0.5f; // Versatz für die beiden Laser

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer für Cooldown herunterzählen
        cooldownTimer -= Time.deltaTime;
        //Debug.Log("Cooldown: " + cooldownTimer);

        // Holen der Gaze-Position und Konvertierung in Weltkoordinaten
        Vector2 gazePosition = GetGazePosition();
        crosshairUI.position = gazePosition;

        HeadPose headPose = TobiiAPI.GetHeadPose();
        Debug.Log("HeadPose:" + headPose.Rotation);

        // Prüfen, ob ein Objekt im Blickfeld ist und der Cooldown-Timer abgelaufen ist
        RaycastHit2D hit = Physics2D.Raycast(gazePosition, Vector2.zero);
        if (hit.collider != null &&
            (hit.collider.CompareTag("Asteroid") || hit.collider.CompareTag("Satalite")))
        {
            if (gazeTimer > 1.5f)
            {
                // Laser in Richtung des Ziels schießen
                //Debug.Log(gazePosition);
                ShootLaserAtTarget(hit.collider.transform.position);
                gazeTimer = 0f;
            }
            gazeTimer += Time.deltaTime;
        }
    }

    Vector2 GetGazePosition()
    {
        // Holt die Gaze-Koordinaten vom Tobii SDK und konvertiert sie in Weltkoordinaten
        var gazePoint = TobiiAPI.GetGazePoint();

        if (!gazePoint.IsValid)
        {
            return Vector2.zero;
        }

        Vector3 screenPosition = gazePoint.Screen;
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    void ShootLaserAtTarget(Vector2 targetPosition)
    {
        // Richtung zum Ziel berechnen und den Laser in die Richtung drehen
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Positionen für die beiden Laser berechnen
        Vector3 leftLaserPosition = transform.position + new Vector3(-laserOffset, 0, 0);
        Vector3 rightLaserPosition = transform.position + new Vector3(laserOffset, 0, 0);

        // Laser instanziieren
        GameObject leftLaser = Instantiate(laserPrefab, leftLaserPosition, Quaternion.identity);
        GameObject rightLaser = Instantiate(laserPrefab, rightLaserPosition, Quaternion.identity);

        leftLaser.transform.right = direction;  // Laser auf das Ziel ausrichten
        rightLaser.transform.right = direction;  // Laser auf das Ziel ausrichten

        // Laser-Bewegung initialisieren
        leftLaser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
        rightLaser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;

        // Zielobjekt beim Treffer zerstören
        StartCoroutine(DestroyTargetOnHit(leftLaser, targetPosition));
        StartCoroutine(DestroyTargetOnHit(rightLaser, targetPosition));
    }

    IEnumerator DestroyTargetOnHit(GameObject laser, Vector2 targetPosition)
    {
        yield return new WaitForSeconds(Vector2.Distance(laser.transform.position, targetPosition) / laserSpeed);

        // Raycast-Hit-Ziel zerstören, wenn getroffen
        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero);
        if (hit.collider != null && (hit.collider.CompareTag("Asteroid") || hit.collider.CompareTag("Satalite")))
        {
            // Zielobjekt zerstören
            Destroy(hit.collider.gameObject); 
            
            // Laser selbst zerstören
            Destroy(laser);
        }
    }
}
