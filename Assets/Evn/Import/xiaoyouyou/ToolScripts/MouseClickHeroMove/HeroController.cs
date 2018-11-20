using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    public Camera followCamera;
    public float m_rotatOffset = 0;
    public float cameraHeight = 10f;
    private Vector3 moveToPoint = Vector3.zero;
    private bool canMove = false;
    public float speed = 20f;
    private heroState currentHeroState = heroState.idle;

    public float cameraDistance = 12f;
    public float LookYOffset;
    
    
    private Vector3 dir;

    CollisionFlags flags = CollisionFlags.Below;

    private bool isMouseDown = false;
    private bool isGround = true;

    private CAnimator m_pAnimator;

    enum heroState
    {
        idle,
        walk,
        run,
        attck,
    }
    
    public int TERRAIN_LAYER = 10;


	void Start () {
        m_pAnimator = new CAnimator(GetComponent<Animator>());

        GameObject dimian = GameObject.Find("pengzhuang_zong");
        if (dimian != null)
        {
            Common.SetObjectAlllayer(dimian, TERRAIN_LAYER);
        }
	}
	
	void Update () {

        m_pAnimator.Update();

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            RaycastHit hit;
            Ray ray = followCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            if (Physics.Raycast(ray, out hit,100, (1<<TERRAIN_LAYER)))
            {
                moveToPoint = hit.point;
                canMove = true;

                dir = Vector3.Normalize(moveToPoint - transform.position);
                Quaternion rot = transform.rotation;
                Quaternion target = Quaternion.LookRotation(dir);
                Vector3 euler = target.eulerAngles;
                euler.z = 0;
                euler.x = 0;
                rot = Quaternion.Euler(euler);

                transform.rotation = rot;

                currentHeroState = heroState.run;
            }
        }

        if (!canMove && !isGround)
        {
            return;
        }
        if (Vector3.Distance(moveToPoint, transform.position) < 0.5f)
        {
            canMove = false;
            currentHeroState = heroState.idle;
        }
        else
        {
            Vector3 m = dir * speed * Time.deltaTime;
            m.y = 0;
            flags = GetComponent<CharacterController>().Move(m);
        }
	}

    void LateUpdate()
    {
        switch (currentHeroState)
        {
            case heroState.idle:
                m_pAnimator.PlayAction("idle");
                break;
            case heroState.run:
                m_pAnimator.PlayAction("run");
                break;
            case heroState.walk:
                break;
            case heroState.attck:
                break;
        }

        if ((GetComponent<CharacterController>().collisionFlags & CollisionFlags.Below) != 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        if (!isGround)
        {
            flags = GetComponent<CharacterController>().Move(new Vector3(0f, -10f, 0f) * Time.deltaTime);
        }

        float currentRotationAngle = m_rotatOffset;
        float wantedHeight = transform.position.y + cameraHeight;
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        followCamera.transform.position = transform.position;
        followCamera.transform.position -= currentRotation * Vector3.forward * cameraDistance;

        followCamera.transform.position = new Vector3(followCamera.transform.position.x, wantedHeight, followCamera.transform.position.z);
        followCamera.transform.LookAt(transform.position + new Vector3(0, LookYOffset,0));
    }
}
