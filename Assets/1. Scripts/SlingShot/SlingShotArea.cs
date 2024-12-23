using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotAreaMask;

    public bool IsWithinSlingShotArea()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        
        if (Physics2D.OverlapPoint(worldPos, slingShotAreaMask)) //Return true if there's a collider object that has specific layer, Overlap = °ãÄ¡´Ù
        { 
            return true;
        }
        else
        {
            return false;
        }
    }
  
}
