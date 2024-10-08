using System.Collections;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    [SerializeField]
    private Transform target;//실제 이동하는 발판의 Transform
    [SerializeField]
    private Transform[] wayPoints;//이동 가능 지점
    [SerializeField]
    private float waitTime;//wayPoint 도착 후 대기시간
    [SerializeField]
    private float timeOffset;//이동시간=거리*timeOffset

    private int wayPointCount;//이동 가능한 wayPoint 개수
    private int currentIndex = 0;//현재 wayPoint 인덱스

    private void Awake()
    {
        target.position = wayPoints[currentIndex].position;
        wayPointCount = wayPoints.Length;

        currentIndex++;

        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        while (true)
        {
            //wayPoints[currentIndex].position 위치까지 이동
            yield return StartCoroutine(MoveAToB(target.position, wayPoints[currentIndex].position));

            //waitTime 시간만큼 대기
            yield return new WaitForSeconds(waitTime);

            //다음 이동 지점(wayPoint) 설정
            if (currentIndex < wayPointCount - 1) currentIndex++;
            else currentIndex = 0;
        }
    }

    private IEnumerator MoveAToB(Vector3 start,Vector3 end)
    {
        float percent = 0;
        float moveTime = Vector3.Distance(start, end) * timeOffset;//Distance: start와 end 사이 거리

        while(percent < 1) 
        {
            percent += Time.deltaTime / moveTime;
            target.position=Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(target.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
