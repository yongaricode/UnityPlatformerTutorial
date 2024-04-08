using UnityEngine;

public class Movement2D : MonoBehaviour

{
    [SerializeField]
    private float speed = 5.0f;  //이동 속도
    [SerializeField]
    private float jumpForce = 8.0f; // 점프 힘 (클수록 높게 점프)
    private Rigidbody2D rigid2D;
    [HideInInspector]
    public bool isLongJump = false; //낮은 점프, 높은 점프 체크

    [SerializeField]
    private LayerMask groundLayer; // 바닥 체크를 위한 충돌 레이어
    private CapsuleCollider2D capsuleCollider2D; //오브젝트의 충돌 범위 컴포넌트
    private bool isGrounded; // 바닥 체크 (바닥에 닿아있을 때 true)
    private Vector3 footposition; // 발의 위치

    [SerializeField]
    private int maxJumpCount = 2; //땅을 밟기 전까지 할 수 있는 최대 점프 횟수
    private int currentJumpCount = 0; // 현재 가능한 점프 횟수 

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();  
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        //플레이어 오브젝트의 Collider2D min, center, max의 위치 정보
        Bounds bounds = capsuleCollider2D.bounds;
        //플레이어의 발 위치 설정
        footposition = new Vector2(bounds.center.x, bounds.center.y);
        //플레이어의 발 위치에 원을 생성하고, 원이 바닥과 닿아있으면 isGrounded = true
        //Physics2D.OverlapCircle()의 반환 값을 bool로 설정하면 충돌하는 오브젝트가 있을 때 true, 없을 때 false를 반환하게 된다
        isGrounded = Physics2D.OverlapCircle(footposition, 0.5f, groundLayer);

        //플레이어의 발이 땅에 닿아 있고, y축 속도가 0이하이면 점프 횟수 초기화
        //velocity.y <= 0을 추가하지 않으면 점프키를 누르는 순간에도 초기화 되어
        // 최대 점프 횟수를 2로 설정하면 3번까지 점프가 가능하게 된다
        if(isGrounded == true && rigid2D.velocity.y <= 0)
        {
            currentJumpCount = maxJumpCount;
        }

        //낮은 점프, 높은 점프 구현을 위한 중력 계수(gravityScale) 조절(Jump up일 때만 적용)
        //중력 계수가 낮은 if문은 높은 점프가 되고, 중력 계수가 높은 else 문은 낮은 점프가 된다. 
        if(isLongJump && rigid2D.velocity.y > 0) {
            rigid2D.gravityScale = 1.0f;
        }
        else
        {
            rigid2D.gravityScale = 2.5f;
        }
    }

    //화면에 보이지 않는 충돌 범위와 같은 것들을 내가 원하는 위치에 잘 배치했는지 확인하고 싶을 때 선, 사각형, 원 등을 그려서 확인하는 용도
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footposition, 0.1f);
    }

    public void Move(float x)
    {
        //x축 이동은 x*speed로, y축 이동은 기존의 속력 값(현재는 중력)
        rigid2D.velocity = new Vector2(x*speed, rigid2D.velocity.y);
    }

    public void Jump()
    {
        if (currentJumpCount > 0)
        {
            //jumpForce의 크기만큼 윗쪽 방향으로 속력 설정
            rigid2D.velocity = Vector2.up * jumpForce;
            //점프 횟수 1 감소
            currentJumpCount--;
        }
        
    }



}
