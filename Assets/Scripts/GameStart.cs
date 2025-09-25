using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void NewGame()
    {
        //ゲームの初期化
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Opening");
    }

    public void ContinueGame()
    {
        RoomManager.toRoomNumber = "SavePoint";
        //ゲームのローディング
        SaveData.LoadGameData();
        SceneManager.LoadScene("Main");
    }

}
