using UnityEngine;
using UnityEngine.SceneManagement;

//プレイヤーが出てきた時の方向
public enum DoorDirection
{ 
    up,
    down
}


public class RoomData : MonoBehaviour
{

    public string roomName;
    public string nextRoomName;
    public string nextScene;
    public bool openDoor;
    public DoorDirection direction;
    public MessageData message;
    public GameObject door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isSavePoint; //セーブポイントに使われるスクリプトにするかどうか

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSavePoint)
        ChanegeScene();
    }

    public void ChanegeScene()
    {

        //このROOMに触れたらどこに行くのかを変数nextRoomNameで決めておく
        //シーンが切り替わって情報がリセットされる前にstatic変数であるtoRoomNumberに行先情報を記録
        RoomManager.toRoomNumber = nextRoomName;

        SceneManager.LoadScene(nextScene);
    }

    public void DoorOpenCheck()
    {
        //もしも開城されていたら子オブジェクトである変数doorは非表示
        if (openDoor) door.SetActive(false);
    }
}
