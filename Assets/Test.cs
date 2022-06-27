using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.back;
    
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.back;
        this.transform.position = -Vector3.back;
        this.transform.position = -Vector3.right;
        this.transform.position = Vector3.right;

    }
}
