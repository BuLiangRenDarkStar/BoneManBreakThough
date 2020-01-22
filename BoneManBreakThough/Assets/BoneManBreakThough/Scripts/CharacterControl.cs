using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransitionParameter
{
    Move,
    Jump,
    ForceTransition,
    Grounded,
}


public class CharacterControl : MonoBehaviour
{
    public float speed = 5.0f;
    public Animator animator;
    public Material material;
    public bool MoveRight;
    public bool MoveLeft;
    public bool Jump;
    public GameObject ColliderEdgePrefab;
    public List<GameObject> BottomSpheres = new List<GameObject>();//Box碰撞框底部四个点
    public List<GameObject> FrontSpheres = new List<GameObject>();//Box前面的点
    List<Collider> RagdollParts = new List<Collider>();

    public float GravityMultiplier;
    public float PullMultiplier;

    private Rigidbody rigid;
   
    public Rigidbody RIGID_BODY
    {
        get
        {
            if( rigid==null )
            {
                rigid = GetComponent<Rigidbody>();
            }
            return rigid;
        }
    }

    private void Awake()
    {
        SetColliderSpheres(  );
        SetRagdollParts(  );
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        for( int i=0; i<colliders.Length; ++i)
        {
            if( colliders[i].gameObject != this.gameObject)
            {
                colliders[i].isTrigger = true;
                RagdollParts.Add(colliders[i]);
            }
        }
    }
    private void SetColliderSpheres()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        float bottom = box.bounds.center.y - box.bounds.extents.y+0.005f;
        float top = box.bounds.center.y + box.bounds.extents.y;
        float front = box.bounds.center.z + box.bounds.extents.z;
        float back = box.bounds.center.z - box.bounds.extents.z;

        GameObject topFront = CreateEdgeSphere(new Vector3(0f, top, front));
        GameObject bottomFront = CreateEdgeSphere(new Vector3(0f, bottom, front));
        GameObject bottomBack = CreateEdgeSphere(new Vector3(0f, bottom, back));

        topFront.transform.parent = transform;
        bottomFront.transform.parent = transform;
        bottomBack.transform.parent = transform;

        BottomSpheres.Add(bottomFront);
        BottomSpheres.Add(bottomBack);

        FrontSpheres.Add(bottomFront);
        FrontSpheres.Add(topFront);

        float horSec = (bottomFront.transform.position - bottomBack.transform.position).magnitude / 5f;
        CreateMiddleSpheres(bottomBack, this.transform.forward, horSec, 4, BottomSpheres);

        float verSec = (bottomFront.transform.position - topFront.transform.position).magnitude / 10f;
        CreateMiddleSpheres(bottomFront, this.transform.up, verSec, 9, FrontSpheres);
    }

    void CreateMiddleSpheres( GameObject startObj, Vector3 dir, float sec, int interations, List<GameObject> spheresList )
    {
        for( int i=0; i<interations; ++i)
        {
            Vector3 pos = startObj.transform.position + (dir * sec * (i + 1));

            GameObject obj = CreateEdgeSphere(pos);
            obj.transform.parent = this.transform;
            spheresList.Add(obj);
        }
    }

    GameObject CreateEdgeSphere( Vector3 pos)
    {
        GameObject obj = Instantiate(ColliderEdgePrefab, pos, Quaternion.identity);
        return obj;
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
   
    private void FixedUpdate()
    {
        if( RIGID_BODY.velocity.y<0f)
        {
            RIGID_BODY.velocity += (-Vector3.up * GravityMultiplier);
        }

        if( RIGID_BODY.velocity.y>0 && !Jump)
        {
            RIGID_BODY.velocity += (-Vector3.up * PullMultiplier);
        }
    }
}
