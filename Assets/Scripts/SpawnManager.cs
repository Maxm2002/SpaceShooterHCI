using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameObject satellitePrefab;
    public GameObject asteroidPrefab;
    private float timer = 0f;

    private EyetrackerController eyeTrackingController;

    // Start is called before the first frame update
    void Start()
    {
        eyeTrackingController = FindObjectOfType<EyetrackerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5) // Intervall, in dem Satelliten und Asteroiden erscheinen
        {
            SpawnObject();
            timer = 0f;
        }
    }
    void SpawnObject()
    {
        // Wähle zufällig aus, ob ein Satellit oder ein Asteroid erscheint
        GameObject objToSpawn = (Random.Range(0, 2) == 0) ? satellitePrefab : asteroidPrefab;

        // Zufällige Position am oberen Bildschirmrand
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0f);

        // Objekt instanziieren
        GameObject spawnedObject = Instantiate(objToSpawn, spawnPosition, Quaternion.identity);

        // Richtung zum Raumschiff berechnen (es wird angenommen, dass das Raumschiff bei (0,0) ist)
        Vector3 direction = (Vector3.zero - spawnPosition).normalized;

        // Objekt in Richtung des Raumschiffs bewegen
        spawnedObject.GetComponent<Rigidbody2D>().velocity = direction * 0.8f; // 1.2 = Geschwindigkeit der Objekte

        if (eyeTrackingController != null)
        {
            eyeTrackingController.AddInteractableObject(spawnedObject);
        }
    }

}
