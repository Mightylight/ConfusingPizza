using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ToppingDisplayPlanet : MonoBehaviour
{
    [SerializeField] private GameObject toppingDisplay;
    [SerializeField] private SpriteRenderer toppingXRenderer;
    [SerializeField] private SpriteRenderer toppingZRenderer;
    [SerializeField] private float centerThreshold = 0.15f;
    private Planet planetScript;
    private Sprite toppingSprite;
    bool explored;


    private void Start()
    {
        // Get the Planet script component attached to this GameObject
        planetScript = GetComponent<Planet>();
    }

    private void ApplySpriteToRenderer(SpriteRenderer renderer)
    {
        // Check if the SpriteRenderer component exists
        if (renderer != null)
        {
            // Apply the sprite to the SpriteRenderer
            renderer.sprite = toppingSprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer component is not assigned in the inspector.");
        }
    }

    void Update()
    {        
        explored = planetScript.isExplored;

        // Get the Topping script component from the Planet script
        Topping topping = planetScript.GetTopping();

        // Get the topping sprite from the Topping script
        toppingSprite = topping.toppingSprite;

        // Apply the sprite to the SpriteRenderer components
        ApplySpriteToRenderer(toppingXRenderer);
        ApplySpriteToRenderer(toppingZRenderer);


        // Get the viewport position of the object
        UnityEngine.Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // Check if the object is in the center of the screen
        if (viewportPos.x >= 0.5f - centerThreshold && viewportPos.x <= 0.5f + centerThreshold &&
            viewportPos.y >= 0.5f - centerThreshold && viewportPos.y <= 0.5f + centerThreshold &&
            viewportPos.z > 0)
        {
            // Object is in the center of the screen
            DisplayTopping();
        }
        else
        {
            // Object is not in the center of the screen
            HideTopping();
        }
    }

    void DisplayTopping()
    {
        if (!explored) { 
            toppingDisplay.SetActive(true);
        }
    }

    void HideTopping()
    {
        toppingDisplay.SetActive(false);
    }
}

