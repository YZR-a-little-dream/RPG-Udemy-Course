using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("mousePosition:" + Input.mousePosition);
            Debug.Log("worldPosition:" +Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        
    }
}
