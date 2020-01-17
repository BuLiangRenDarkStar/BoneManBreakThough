using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed=5.0f;
    public Animator animator;
    public Material material;

    private enum TransitionParameter
    {
        Move,
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(VirtualInputManager.Instance.MoveRight && VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
            return;
        }

        if( !VirtualInputManager.Instance.MoveRight && !VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
        }

        if( VirtualInputManager.Instance.MoveLeft)
        {            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
    }

    public void ChangeMaterial()
    {
        if( null==material)
        {
            Debug.LogError("材质未添加");
        }

        Renderer[] renderArrary = GetComponentsInChildren<Renderer>();

        for( int i=0; i<renderArrary.Length; ++i)
        {
            if( renderArrary[i].gameObject != this.gameObject)
            {
                renderArrary[i].material = material;
            }            
        }
    }
}
