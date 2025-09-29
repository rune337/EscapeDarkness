using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static bool[] doorsOpenedState = { false, false, false }; //ドアの開閉状況

    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keysPickedState = { false, false, false }; //鍵の取得状況

    public static int bill = 0; //お札の残数
    public static int sword = 0; //剣の所持数
    public static bool[] itemsPickedState = { false, false, false, false, false,false}; //アイテムの取得状況

    public static bool hasSpotLight; //スポットライトを持っているかどうか

    public static int playerHP = 30; //プレイヤーのHP

    void Start()
    {
        //まずはゲームは開始状態にする
        gameState = GameState.playing;

        //シーン名の取得
        Scene currentScene = SceneManager.GetActiveScene();
        // シーンの名前を取得
        string sceneName = currentScene.name;

        switch (sceneName)
        {
            case "Title":
                SoundManager.instance.PlayBgm(BGMType.Title);
                break;
            case "Boss":
                SoundManager.instance.PlayBgm(BGMType.InBoss);
                break;
            case "Opening":
            case "Ending":
                SoundManager.instance.StopBgm();
                break;
            default:
                SoundManager.instance.PlayBgm(BGMType.InGame);
                break;
        }
    }

    private void Update()
    {
        //ゲームオーバーになったらタイトルに戻る
        if (gameState == GameState.gameover)
        {
            //時間差でシーン切り替え
            StartCoroutine(TitleBack());

            //Invokeメソッドでも可
        }
    }

    //ゲームオーバーの際に発動するコルーチン
    IEnumerator TitleBack()
    {
        yield return new WaitForSeconds(5); //5秒待つ
        SceneManager.LoadScene("Title"); //タイトルに戻る
    }

}