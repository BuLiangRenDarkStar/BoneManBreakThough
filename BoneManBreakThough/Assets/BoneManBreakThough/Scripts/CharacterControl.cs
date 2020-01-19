using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransitionParameter
{
    Move,
}


public class CharacterControl : MonoBehaviour
{
    public float speed = 5.0f;
    public Animator animator;
    public Material material;
    public bool MoveRight;
    public bool MoveLeft;
    public bool Jump;
        
    void Start()
    {
       
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
