using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI elements like Image

public class Healthbar : MonoBehaviour
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI elements like Image

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthbar;
    [SerializeField] private Image currenthealthbar;

    private void Start()
    {
        // Initialize total health bar if needed
        totalhealthbar.fillAmount = playerHealth.currentHealth / 10f; // Full health initially
    }
        private void Update()
    { 
        // Update the current health bar fill amount based on player's health
        currenthealthbar.fillAmount = playerHealth.currentHealth / 10f; // Adjust the divisor (10) to match your health logic
    }
}    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthbar;
    [SerializeField] private Image currenthealthbar;

    private void Start()
    {
        // Initialize total health bar if needed
        totalhealthbar.fillAmount = playerHealth.currentHealth / 10f; // Full health initially
    }
        private void Update()
    { 
        // Update the current health bar fill amount based on player's health
        currenthealthbar.fillAmount = playerHealth.currentHealth / 10f; // Adjust the divisor (10) to match your health logic
    }
}