using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
// Handles the activation and deactivation of rooms, including enemies.
public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;  // List of enemies within the room
    private Vector3[] initialPosition;  // Store initial positions of enemies to reset upon activation

    private void Awake()
    {
        // Store initial positions of all enemies
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPosition[i] = enemies[i].transform.position;
            }
        }
    }

    // Activate or deactivate all enemies in the room
    public void ActivateRoom(bool _status)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);  // Activate or deactivate the enemy
                enemies[i].transform.position = initialPosition[i];  // Reset enemy to initial position
            }
        }
    }
}
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPosition;

    private void Awake()
    {
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPosition[i] = enemies[i].transform.position;
            }
        }
    }

    // Made public to allow external access
    public void ActivateRoom(bool _status)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }
}
