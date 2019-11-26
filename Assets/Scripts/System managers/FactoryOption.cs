﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Option for factory production
public class FactoryOption : MonoBehaviour
{
    Resource resource;                              // Resource
    [SerializeField] Slider slider;                 // Slider contianing number of resource you want to produce
    [SerializeField] Text sliderText;               // Text of slider

    // Initializes factory options, cnanot be awake since resources must be sent
    public void InitializeOption(Resource r)
    {
        resource = r;
        transform.SetParent(transform.parent, false);
    }


    void FixedUpdate()
    {
        // Checks if resources is empty or not
        if (resource != null)
        {
            // Updates value indicating number of element to be produced
            sliderText.text = slider.value.ToString();

            // gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            // Goes through all resources
            foreach (Resource.ResourceAmount r in resource.ReturnParents())
            {

            }
        }
    }

    // Updates isInGUI in game manager
    public void UpdateFactoryBusy(bool currentState)
    {
        Debug.Log(currentState); // DEBUG
        GameManager.isInGUI = currentState;
    }
}
