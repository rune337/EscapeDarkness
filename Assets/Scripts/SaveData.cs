using System;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // ゲームデータをPlayerPrefsに保存するメソッド
    public static void SaveGameData()
    {
        // 現在のstatic変数の状態をGameDataインスタンスにコピー
        GameData dataToSave = new GameData();

        // GameDataインスタンスをJSON文字列に変換
        string jsonData = JsonUtility.ToJson(dataToSave);

        // JSON文字列をPlayerPrefsに保存
        PlayerPrefs.SetString("GameData", jsonData);
        PlayerPrefs.Save(); // 変更をディスクに書き込む

        Debug.Log("セーブしました (JSON): " + jsonData);
    }


    // PlayerPrefsからJSONをロードし、ゲームデータに適用するメソッド
    public static void LoadGameData()
    {
        // PlayerPrefsからJSON文字列をロード
        string jsonData = PlayerPrefs.GetString("GameData");

        // JSON文字列をGameDataインスタンスに変換
        GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

        // ロードしたデータをstatic変数に適用
        loadedData.ApplyToStatic();
    }
}


[Serializable] // JsonUtility でシリアライズ可能（JSON化の準備）にするために必要
public class GameData
{
    public GameState gameState;

    public bool[] doorsOenedState;
    public int key1;
    public int key2;
    public int key3;
    public bool[] keysPickedState;

    public int bill;
    public bool[] itemsPickedState;

    public bool hasSpotLight;
    public int playerHP;

    // RoomManager のデータもここに含める
    public int[] doorsPositionNumber;
    public int key1PositionNumber;
    public int[] itemsPositionNumber; //アイテムの配置番号

    //初期配置が必要かどうか
    public bool positioned; //初回配置が済かどうか

    // コンストラクタで現在のstatic変数の値をコピー
    public GameData()
    {
        gameState = GameManager.gameState;
        doorsOenedState = GameManager.doorsOpenedState;
        key1 = GameManager.key1;
        key2 = GameManager.key2;
        key3 = GameManager.key3;
        keysPickedState = GameManager.keysPickedState;
        bill = GameManager.bill;
        itemsPickedState = GameManager.itemsPickedState;
        hasSpotLight = GameManager.hasSpotLight;
        playerHP = GameManager.playerHP;

        // RoomManager の static 変数もコピー
        doorsPositionNumber = RoomManager.doorsPositionNumber;
        key1PositionNumber = RoomManager.key1PositionNumber;
        itemsPositionNumber = RoomManager.itemsPositionNumber;
        positioned = RoomManager.positioned;
    }

    // static 変数にデータを適用するメソッド
    public void ApplyToStatic()
    {
        GameManager.gameState = gameState;
        GameManager.doorsOpenedState = doorsOenedState;
        GameManager.key1 = key1;
        GameManager.key2 = key2;
        GameManager.key3 = key3;
        GameManager.keysPickedState = keysPickedState;
        GameManager.bill = bill;
        GameManager.itemsPickedState = itemsPickedState;
        GameManager.hasSpotLight = hasSpotLight;
        GameManager.playerHP = playerHP;

        // RoomManager の static 変数に適用
        RoomManager.doorsPositionNumber = doorsPositionNumber;
        RoomManager.key1PositionNumber = key1PositionNumber;
        RoomManager.itemsPositionNumber = itemsPositionNumber;
        RoomManager.positioned = positioned;
    }
}