﻿using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static InputManager inputManager;        // The input manager
    public static bool isPaused = false;            // Is paused

    // Array of resources
    // [SerializeField] 
    public static Resource.ResourceAmount[] resources;
    [SerializeField] Resource.ResourceAmount[] startResources;

    // Miltary mght, might be expanded upon later
    public float defensivePower = 0;
    public float offensivePower = 0;
    public float supplyPower = 0;

    // Amount of resources generated through various systems
    public float moneyAmount = 0;                   // Money
    public uint population = 0;                     // Population
    public float happiness = 0;                     // Happiness, might be changed to be a value between 0 and 100 in the future

    // Bools for states
    public static bool isInGUI;                     // Specifies that the user is in a GUI and it should not be shut down

    float previousTimeSpeed = 1;                    // Previous speed of time

    // Options
    [SerializeField] Canvas optionsMenu;            // The options menu
    [SerializeField] OptionsManager optionsManager; // The options manager

    FactoryBuilding currentlySelectedFactory;       // Currently selected factory



    // Start is called before the first frame update
    void Awake()
    {
        // Loads all resources automatically
        Resource[] resourceObjects1 = Resources.LoadAll("\\Prefabs/Resources/Raw resources", typeof(Resource)).Cast<Resource>().ToArray();
        Resource[] resourceObjects2 = Resources.LoadAll("\\Prefabs/Resources", typeof(Resource)).Cast<Resource>().ToArray();
        Resource[] resourceObjects = new Resource[resourceObjects1.Length + resourceObjects2.Length];

        // Loads resources into single array
        Array.Copy(resourceObjects1, resourceObjects, resourceObjects1.Length);
        Array.Copy(resourceObjects2, 0, resourceObjects, resourceObjects1.Length, resourceObjects2.Length);
        
        // Loads resources into static with 0 as the amount in all cases
        resources = new Resource.ResourceAmount[resourceObjects.Length];
        for (int i = 0; i < resourceObjects.Length; i++)
        {
            Resource.ResourceAmount currentResource;
            currentResource.amount = 0;
            currentResource.resource = resourceObjects[i];
        }
        
        // DEBUG
        Debug.Log(resourceObjects2.Length);

        // DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        optionsMenu.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        // If the game is not paused
        if (!isPaused)
        {
            // Checks if primary mouse button is down
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                
                // Factory
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Factory")
                {
                    if (currentlySelectedFactory != null)                                           // Checks if a building is already selected
                    {
                        currentlySelectedFactory.ActivateGUI(false);                                // Disables menu of previously selected game object
                    }
                    currentlySelectedFactory = hitInfo.transform.GetComponent<FactoryBuilding>();   // Sets newly selected game object

                    currentlySelectedFactory.ActivateGUI(true);                                     // Activates GUI of game object
                }
                else if (currentlySelectedFactory != null && !isInGUI)                              // Checks if tehre is a factory there and that you are outside of any relevant GUI element
                {
                    currentlySelectedFactory.ActivateGUI(false);                                    // Disables menu of previously selected game object
                }
            }
        }
    }
    
    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        optionsMenu.enabled = isPaused = active;

        // if paused then it sets speed to zero
        if (active)
        {
            previousTimeSpeed = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = previousTimeSpeed;
        }
        // Freezes rotation while paused, needed to fix a bug causing major rotation sometimes, TODO: Find and fix that bug, then remove the following line of code
        Camera.main.transform.GetComponent<Rigidbody>().freezeRotation = !active;

        // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
        Cursor.visible = active || inputManager.frozenAngle; // WIP
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
        optionsManager.currentButton = EventSystem.current.currentSelectedGameObject;
    }

    // Turns on/off rendering of resources
    public void ResourceRendering(bool render)
    {
        if (render)
        {
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Resources");
        }
        else
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Resources"));
        }
    }

    // Toggle rendering of resources
    public void ToggleResourceRendering()
    {
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("Resources");
    }
}
