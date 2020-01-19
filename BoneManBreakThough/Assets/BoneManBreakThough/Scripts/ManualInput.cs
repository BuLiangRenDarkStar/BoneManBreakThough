using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualInput : MonoBehaviour
{
    private CharacterControl characterControl;

    private void Awake()
    {
        characterControl = this.gameObject.GetComponent<CharacterControl>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            characterControl.MoveRight = true;
        }
        else
        {
            characterControl.MoveRight = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            characterControl.MoveLeft = true;
        }
        else
        {
            characterControl.MoveLeft = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            characterControl.Jump = true;
        }
        else
        {
            characterControl.Jump = false;
        }
    }

}
