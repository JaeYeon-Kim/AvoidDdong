using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


// PlayerController 스크립트 
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private float speed = 3f;

    private float moveDirection = 0f;

    private bool isLeftPressed = false;
    private bool isRightPressed = false;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.stopTrigger)
        {
            animator.SetTrigger("start");
            PlayerMove();
        }

        if (!GameManager.instance.stopTrigger)
        {
            animator.SetTrigger("dead");
        }
    }

    private void LateUpdate()
    {
        ScreenLimitArea();
    }

    private void PlayerMove()
    {
        animator.SetFloat("speed", Mathf.Abs(moveDirection));

        if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection > 0)
        {
            spriteRenderer.flipX = false;
        }
        rigid.velocity = new Vector2(moveDirection * speed, rigid.velocity.y);
    }

    // 캐릭터가 화면밖으로 나가지 못하도록 막음 
    private void ScreenLimitArea()
    {
        // 현재 오브젝트의 월드공간에서의 위치값을 뷰포트공간으로 바꿔서 가져옴 
        Vector3 worldPos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldPos.x < 0.05f) worldPos.x = 0.05f;
        if (worldPos.x > 0.95f) worldPos.x = 0.95f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldPos);
    }

    // 버튼을 통해 이동방향 설정

    // 좌측 
    public void MoveLeft()
    {
        isLeftPressed = true;
        moveDirection = -1f;
    }

    public void MoveRight()
    {
        isRightPressed = true;
        moveDirection = 1f;
    }



    // 버튼에서 손을 뗄 때 체크 후 이동 방향 유지
    public void StopMoveLeft()
    {
        isLeftPressed = false;
        if (!isRightPressed) // 오른쪽 버튼이 눌린 상태가 아니면 멈춤
        {
            moveDirection = 0f;
        }
    }

    public void StopMoveRight()
    {
        isRightPressed = false;
        if (!isLeftPressed) // 왼쪽 버튼이 눌린 상태가 아니면 멈춤
        {
            moveDirection = 0f;
        }
    }
}
