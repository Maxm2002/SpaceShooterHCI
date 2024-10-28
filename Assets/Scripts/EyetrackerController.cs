using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class EyetrackerController : MonoBehaviour
{
    public List<GameObject> interactableObjects = new List<GameObject>(); // Liste aller Objekte, die hervorgehoben werden können
    public Vector2 gazePosition;
    public RectTransform crosshairUI;
    public Camera mainCamera; // Referenz zur Hauptkamera
    private SpaceshipController spaceshipController;

    public GameObject projectilePrefab; // Das Projektil-Objekt, das abgeschossen wird
    private GameObject currentTarget; // Das Objekt, auf das der Benutzer schaut
    private float gazeDuration; // Verweildauer auf dem aktuellen Ziel
    public float requiredGazeTime = 0.2f; // Zeit in Sekunden, die notwendig ist, um zu schießen

    // Start is called before the first frame update
    void Start()
    {
        spaceshipController = FindObjectOfType<SpaceshipController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 gazePosition = GetGazePosition(); //bsp: (-3.1, 5.1)
        //Debug.Log("GazePosition: " + gazePosition);

        //LogObjectPositions();
        crosshairUI.position = gazePosition;

        CheckGazeOnObjects();

        //HeadPose headPose = TobiiAPI.GetHeadPose();
        //Debug.Log("HeadPose:" + headPose.Rotation);

    }
    public void AddInteractableObject(GameObject obj)
    {
        // Fügt ein neu erstelltes Objekt zur Liste hinzu
        interactableObjects.Add(obj);
    }

    public void RemoveInteractableObject(GameObject obj)
    {
        // Fügt ein neu erstelltes Objekt zur Liste hinzu
        interactableObjects.Remove(obj);
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
    void CheckGazeOnObjects()
    {
        GameObject target = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null)
            {
                float distance = Vector2.Distance(gazePosition, obj.transform.position);
                Debug.Log("distance: " + distance);
                if (distance < 2 && distance < minDistance)
                {
                    Debug.Log("distance kleiner 2: ");
                    target = obj;
                    minDistance = distance;
                }
            }
        }

        if (target == currentTarget)
        {
            gazeDuration += Time.deltaTime;
            //Debug.Log("GazeDuration: " + gazeDuration);
            if (gazeDuration >= 0.01f)
            {
                //Debug.Log("Es wird geschossen");
                //spaceshipController.ShootDoubleLaser(gazePosition);
                gazeDuration = 0f; // Reset after shooting
            }
        }
        else
        {
            currentTarget = target;
            gazeDuration = 0f; // Reset if the target changes
        }
    }

    /*void LogObjectPositions()
    {
        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null)
            {
                Vector3 position = obj.transform.position;

                // Unterscheidung nach Tag
                if (obj.CompareTag("Asteroid"))
                {
                    float distance = Vector2.Distance(gazePosition, obj.transform.position);
                    if (distance < 0.8)
                    {
                        
                    }

                    Debug.Log($"Asteroid - Position (x, y): ({position.x}, {position.y})");
                }
                else if (obj.CompareTag("Satalite"))
                {
                    Debug.Log($"Satalite - Position (x, y): ({position.x}, {position.y})");
                }
            }
        }
    }*/
}
