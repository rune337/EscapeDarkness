using UnityEngine;

//ゲーム状態を管理する列挙型
public enum GameState
{
    playing,
    talk,
    gameover,
    gameclear,
    ending
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState; //ゲームのステータス

    public static bool[] doorsOpenedState = {false,false,false}; //ドアの開閉状況

    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keysPickedState = { false,false,false}; //鍵の取得状況

    public static int bill = 10; //お札の残数
    public static bool[] itemsPickedState = { false,false,false,false,false}; //アイテムの取得状況
  

    public static bool hasSpotLight; //スポットライトを持っているかどうか

    public static int playerHP = 3; //プレイヤーのHP

    void Start()
    {
        //まずはゲームは開始状態にする
        gameState = GameState.playing;
    }

}