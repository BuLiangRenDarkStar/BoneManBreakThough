using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransitionParameter
{
    Move,
    Jump,
    ForceTransition,
    Grounded,
    Attack,
}


public class CharacterControl : MonoBehaviour
{
    public Animator SkinnedMeshAnimator;
    public Material material;
    public bool MoveRight;
    public bool MoveLeft;
    public bool Jump;
    public bool Attack;

    public GameObject ColliderEdgePrefab;
    public List<GameObject> BottomSpheres = new List<GameObject>();//Box碰撞框底部四个点
    public List<GameObject> FrontSpheres = new List<GameObject>();//Box前面的点
    public List<Collider> RagdollParts = new List<Collider>();
    public List<Collider> CollidingParts = new List<Collider>();

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
        bool SwitchBack = false;

        if (!IsFacingForward())
        {
            SwitchBack = true;
        }

        SetRagdollParts();//先获取布娃娃系统上的collider
        SetColliderSpheres(  );
        if (SwitchBack)
        {
            FaceForward(false);
        }
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

    //开启布娃娃系统
    public void TurnOnRagdoll()
    {
        RIGID_BODY.useGravity = false;//需要关闭根节点重力系统
        RIGID_BODY.velocity = Vector3.zero;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        SkinnedMeshAnimator.enabled = false;
        //animator.avatar = null;

        for ( int i=0; i<RagdollParts.Count; ++i)
        {
            RagdollParts[i].isTrigger = false;           
            RagdollParts[i].attachedRigidbody.velocity = Vector3.zero;
        }
    }

    //private IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(2f);
    //    //RIGID_BODY.AddForce(200f * Vector3.up);
    //    yield return new WaitForSeconds(0.5f);
    //    TurnOnRagdoll();
    //}

    //检测除自身子节点外的触发器有无其他触发器 与该节点碰撞器接触
    private void OnTriggerEnter(Collider col)
    {
        
        if (RagdollParts.Contains(col))
            return;

        CharacterControl characterControl = col.transform.root.GetComponent<CharacterControl>();

        if( null==characterControl )
        {
            return;
        }

        if( col.gameObject == characterControl.gameObject )
        {
            Debug.Log( "this GameObject" + gameObject.name + "col GameObject" + col.gameObject.name);
            return;
        }

        if( !CollidingParts.Contains(col) )
        {
            CollidingParts.Add(col);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if( CollidingParts.Contains(col))
        {
            CollidingParts.Remove(col);
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

    public void MoveForward( float Speed, float SpeedGraph )
    {
        transform.Translate(Vector3.forward * Speed * SpeedGraph * Time.deltaTime);
    }

    public void FaceForward( bool forward )
    {
        if( forward)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public bool IsFacingForward()
    {
        if (transform.forward.z > 0f)
        {
            return true;
        }
        else
        {
            return false;
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
