using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_tube : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.eulerAngles = new Vector3(0, Time.deltaTime, 0);
        gameObject.transform.Rotate(0, Time.deltaTime * -2, 0);
        
    }
}
