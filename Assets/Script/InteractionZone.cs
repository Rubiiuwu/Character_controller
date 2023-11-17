using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactionzone : MonoBehaviour
{
    TPS_CONTROLLER _controller;

    void Awake()
    {
        _controller = GetComponentInParent<TPS_CONTROLLER>();
    }

    void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Recogible")
        {
            _controller.objectToGrab = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Recogible")
        {
            _controller.objectToGrab = null;
        }
    }
    
}
