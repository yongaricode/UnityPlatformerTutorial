using UnityEngine;

public class TileItem : TileBase
{
    [Header("Tile Item")]
    [SerializeField]
    private ItemType itemType=ItemType.Random;//아이템 속성
    [SerializeField]
    private GameObject[] itemPrefabs;//아이템 타일과 상호작용 했을 때 생성되는 아이템 프리팹들
    [SerializeField]
    private int coinCount;//아이템의 속성이 코인일 때 코인 개수
    [SerializeField]
    private Sprite nonBrokeImage;//아이템 타일의 모든 아이템이 소진되었을 때 출력되는 이미지

    private bool isEmpty=false;//아이템 타일 비어있는지 여부

    public override void UpdateCollision()
    {
        //아이템이 비어있으면 NonBroke와 동일하게 상호작용 없음
        if(isEmpty==true) return;

        //부모클래스인 TileBase에 있는 UpdateCollision() 메소드 호출
        base.UpdateCollision();

        //아이템 생성
        SpawnItem();
    }

    private void SpawnItem()
    {
        if(itemType==ItemType.Random)//임의의 아이템으로 속성 변경
        {
            itemType = (ItemType)Random.Range(0, itemPrefabs.Length);
        }

        //아이템 생성시 ItemBase.Setup() 호출
        GameObject item=Instantiate(itemPrefabs[(int)itemType],transform.position,Quaternion.identity);
        item.GetComponent<ItemBase>().Setup();

        if (itemType == ItemType.Coin)
        {
            coinCount--;
        }

        //아이템의 속성이 코인이 아니거나 코인일 때 코인 개수가 0이면
        if(itemType!=ItemType.Coin||(itemType==ItemType.Coin && coinCount==0)) 
        {
            GetComponent<SpriteRenderer>().sprite = nonBrokeImage;//item tile이미지를 빈 타일 이미지로 변경
            isEmpty = true;//아이템 타일이 비어있음으로 설정
        }
    }
}
