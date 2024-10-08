using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenArea : MonoBehaviour
{
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeEffect.Fade(tilemap,tilemap.color.a,0,tilemap.color.a));
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //충돌 종료-> 알파 0 될 때까지 증가하는 페이드 효과
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeEffect.Fade(tilemap, tilemap.color.a, 1, 1-tilemap.color.a));
        }
    }
}
