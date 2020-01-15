using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    public float speed=5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(VirtualInputManager.Instance.MoveRight && VirtualInputManager.Instance.MoveLeft)
        {
            return;
        }

        if( VirtualInputManager.Instance.MoveLeft)
        {            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
