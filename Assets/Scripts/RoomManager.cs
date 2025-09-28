using System.Xml;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = { 0, 0, 0 }; //各入口の配置番号
    public static int key1PositionNumber; //鍵1の配置番号
    public static int[] itemsPositionNumber = { 0, 0, 0, 0, 0 }; //アイテムの配置番号

    public GameObject[] items = new GameObject[5]; //5つのアイテムプレハブの内訳

    public GameObject room; //ドアのプレハブ
    public MessageData[] messages; //配置したドアに割り振るScriptableObject

    public GameObject dummyDoor; //ダミーのドアプレハブ
    public GameObject key; //キーのプレハブ

    public static bool positioned; //初回配置が済かどうか
    public static string toRoomNumber = "fromRoom1"; //Playerが配置されるべき位置

    GameObject player; //プレイヤーの情報

    void Awake()
    {
        //プレイヤー情報の取得
        player = GameObject.FindGameObjectWithTag("Player");

        if (!positioned) //初期配置がまだ
        {
            StartKeysPosition(); //キーの初回配置
            StartItemsPosition(); //アイテムの初回配置
            StartDoorsPosition(); //ドアの初回配置
            positioned = true; //初回配置は済み
        }
        else //初期配置済みだった場合は配置を再現
        {
            LoadKeysPosition(); //キーの配置の再現
            LoadItemsPosition(); //アイテムの配置の再現
            LoadDoorsPosition(); //ドアの配置の再現

            PlayerPosition(); //プレイヤーの配置
        }
    }

    void StartKeysPosition()
    {
        //全Key1のスポットの取得
        GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

        //ランダムに番号を取得 (第一引数以上 第二引数未満）
        int rand = Random.Range(1, (keySpots.Length + 1));

        //全スポットをチェックしにいく
        foreach (GameObject spots in keySpots)
        {
            //ひとつひとつspotNumの中身を確認してrandと同じかチェック
            if (spots.GetComponent<KeySpot>().spotNum == rand)
            {
                //キー1を生成
                Instantiate(key, spots.transform.position, Quaternion.identity);
                //どのスポット番号にキーを配置したか記録
                key1PositionNumber = rand;
            }
        }

        //Key2およびKey3の対象スポット
        GameObject keySpot;
        GameObject obj; //生成したKey2、およびKey3が入る予定

        //Key2スポットの取得
        keySpot = GameObject.FindGameObjectWithTag("KeySpot2");
        //Keyの生成とonjへの格納
        obj = Instantiate(key, keySpot.transform.position, Quaternion.identity);
        //生成したKeyのタイプをkey2に変更
        obj.GetComponent<KeyData>().keyType = KeyType.key2;

        //Key3スポットの取得
        keySpot = GameObject.FindGameObjectWithTag("KeySpot3");
        //Keyの生成とonjへの格納
        obj = Instantiate(key, keySpot.transform.position, Quaternion.identity);
        //生成したKeyのタイプをkey3に変更
        obj.GetComponent<KeyData>().keyType = KeyType.key3;
    }

    void StartItemsPosition()
    {
        //全部のアイテムスポットを取得
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            //ランダムな数字の取得
            //※ただしアイテム割り振り済みの番号を引いたら、ランダム引き直し
            int rand; //ランダムな数の受け皿
            bool unique; //重複していないかのフラグ

            do
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (itemSpots.Length + 1)); //1番からスポット数の番号をランダムで取得

                //すでにランダムに取得した番号がどこかのスポットとして割り当てられていないか、doorsPositionNumber配列の状況を全点検
                foreach (int numbers in itemsPositionNumber)
                {
                    //取り出した情報とランダム番号が一致していたら重複したいたということになる
                    if (numbers == rand)
                    {
                        unique = false; //唯一のユニークなものではない
                        break;
                    }
                }
            } while (!unique);

            //スポットの全チェック（ランダム値とスポット番号の一致）
            //一致していれば、そこにアイテムを生成
            foreach (GameObject spots in itemSpots)
            {
                if (spots.GetComponent<ItemSpot>().spotNum == rand)
                {
                    GameObject obj = Instantiate(
                        items[i],
                        spots.transform.position,
                        Quaternion.identity
                        );

                    //どのスポット番号がどのアイテムに割り当てられているのかを記録
                    itemsPositionNumber[i] = rand;

                    //生成したアイテムに識別番号を割り振っていく
                    if (obj.CompareTag("Bill"))
                    {
                        obj.GetComponent<BillData>().itemNum = i;
                    }
                    else
                    {
                        obj.GetComponent<DrinkData>().itemNum = i;
                    }
                }
            }

        }
    }

    void StartDoorsPosition()
    {
        //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        //出入り口(鍵1～鍵3の3つの出入り口）の分だけ繰り返し
        for (int i = 0; i < doorsPositionNumber.Length; i++)
        {
            int rand; //ランダムな数の受け皿
            bool unique; //重複していないかのフラグ

            do
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (roomSpots.Length + 1)); //1番からスポット数の番号をランダムで取得

                //すでにランダムに取得した番号がどこかのスポットとして割り当てられていないか、doorsPositionNumber配列の状況を全点検
                foreach (int numbers in doorsPositionNumber)
                {
                    //取り出した情報とランダム番号が一致していたら重複したいたということになる
                    if (numbers == rand)
                    {
                        unique = false; //唯一のユニークなものではない
                        break;
                    }
                }
            } while (!unique);

            //全スポットを見回りしてrandと同じのスポットを探す
            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == rand)
                {
                    //ルームを生成
                    GameObject obj = Instantiate(
                        room,
                        spots.transform.position,
                        Quaternion.identity
                        );

                    //何番スポットが選ばれたのかstatic変数に記憶していく
                    doorsPositionNumber[i] = rand;

                    //生成したドアのセッティング
                    DoorSetting(
                        obj, //対象オブジェクト
                        "fromRoom" + (i + 1), //生成したドアの識別名
                        "Room" + (i + 1), //そこの出入り口に触れたときどこに行くのか
                        "Main", //行き先となるシーン名
                        false, //ドアの開錠の状況
                        DoorDirection.down, //この出入り口に戻った時のプレイヤーの配置
                        messages[i]
                        );
                }
            }
        }

        //ダミー扉の生成
        foreach (GameObject spots in roomSpots)
        {
            //すでに配置済みかどうか
            bool match = false;

            foreach (int doorsNum in doorsPositionNumber)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorsNum)
                {
                    match = true; //そのスポット番号にはすでに配置済み
                    break;
                }
            }

            //数字がマッチしていなければこれまで何も配置されていないということなのでダミードアを配置
            if (!match) Instantiate(dummyDoor, spots.transform.position, Quaternion.identity);
        }

    }

    //生成したドアのセッティング
    void DoorSetting(GameObject obj, string roomName, string nextRoomName, string sceneName, bool openedDoor, DoorDirection direction, MessageData message)
    {
        RoomData roomData = obj.GetComponent<RoomData>();
        //第一引数に指定したオブジェクトのRoomDataスクリプトの各変数に
        //第二引数以降で指定した値を代入
        roomData.roomName = roomName;
        roomData.nextRoomName = nextRoomName;
        roomData.nextScene = sceneName;
        roomData.openedDoor = openedDoor;
        roomData.direction = direction;
        roomData.message = message;

        roomData.DoorOpenCheck(); //ドアの開閉状況フラグをみてドアを表示/非表示メソッド
    }

    void LoadKeysPosition()
    {
        //Key1が未取得だったら
        if (!GameManager.keysPickedState[0])
        {
            //全Key1スポットの取得
            GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

            //全スポットを順番に点検
            foreach (GameObject spots in keySpots)
            {
                //記録しているスポットNOと一緒かどうか
                if (spots.GetComponent<KeySpot>().spotNum == key1PositionNumber)
                {
                    //Key1の生成
                    Instantiate(
                        key,
                        spots.transform.position,
                        Quaternion.identity
                        );
                }
            }
        }

        //Key2が未取得だったら
        if (!GameManager.keysPickedState[1])
        {
            //Key2スポットの取得
            GameObject keySpot2 = GameObject.FindGameObjectWithTag("KeySpot2");
            //Keyの生成
            GameObject obj = Instantiate(
                key,
                keySpot2.transform.position,
                Quaternion.identity
                );
            //生成したKeyのタイプを変えておく
            obj.GetComponent<KeyData>().keyType = KeyType.key2;
        }

        //Key3が未取得だったら
        if (!GameManager.keysPickedState[2])
        {
            //Key3スポットの取得
            GameObject keySpot3 = GameObject.FindGameObjectWithTag("KeySpot3");
            //Keyの生成
            GameObject obj = Instantiate(
                key,
                keySpot3.transform.position,
                Quaternion.identity
                );
            //生成したKeyのタイプを変えておく
            obj.GetComponent<KeyData>().keyType = KeyType.key3;
        }

    }

    void LoadItemsPosition()
    {
        //全部のアイテムスポットを取得
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            if (!GameManager.itemsPickedState[i])
            {
                //スポットの全チェック（ランダム値とスポット番号の一致）
                //一致していれば、そこにアイテムを生成
                foreach (GameObject spots in itemSpots)
                {
                    if (spots.GetComponent<ItemSpot>().spotNum == itemsPositionNumber[i])
                    {
                        GameObject obj = Instantiate(
                            items[i],
                            spots.transform.position,
                            Quaternion.identity
                            );

                        //生成したアイテムに識別番号を割り振っていく
                        if (obj.CompareTag("Bill"))
                        {
                            obj.GetComponent<BillData>().itemNum = i;
                        }
                        else
                        {
                            obj.GetComponent<DrinkData>().itemNum = i;
                        }
                    }
                }
            }

        }
    }

    void LoadDoorsPosition()
    {
        //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        //出入り口(鍵1～鍵3の3つの出入り口）の分だけ繰り返し
        for (int i = 0; i < doorsPositionNumber.Length; i++)
        {
            //全スポットを見回りして記録された番号と同じスポットを探す
            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorsPositionNumber[i])
                {
                    //ルームを生成
                    GameObject obj = Instantiate(
                        room,
                        spots.transform.position,
                        Quaternion.identity
                        );

                    //生成したドアのセッティング
                    DoorSetting(
                        obj, //対象オブジェクト
                        "fromRoom" + (i + 1), //生成したドアの識別名
                        "Room" + (i + 1), //そこの出入り口に触れたときどこに行くのか
                        "Main", //行き先となるシーン名
                        GameManager.doorsOpenedState[i], //ドアの開錠の状況をstaticから読み取る
                        DoorDirection.down, //この出入り口に戻った時のプレイヤーの配置
                        messages[i]
                        );
                }
            }
        }

        //ダミー扉の生成
        foreach (GameObject spots in roomSpots)
        {
            //すでに配置済みかどうか
            bool match = false;

            foreach (int doorsNum in doorsPositionNumber)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorsNum)
                {
                    match = true; //そのスポット番号にはすでに配置済み
                    break;
                }
            }

            //数字がマッチしていなければこれまで何も配置されていないということなのでダミードアを配置
            if (!match) Instantiate(dummyDoor, spots.transform.position, Quaternion.identity);
        }
    }

    //Playerの配置
    void PlayerPosition()
    {
        GameObject[] roomDatas = GameObject.FindGameObjectsWithTag("Room");

        foreach(GameObject room in roomDatas)
        {
            //それぞれのRoomのRoomDataスクリプトの情報を変数rに代入
            RoomData r = room.GetComponent<RoomData>();

            //取得してきたRoomの識別名が「今目標している行先」の識別名(static変数)と同じなら
            if(r.roomName == toRoomNumber)
            {
                float posY = 1.5f; //最初は対象となるRoomの上座標
                if(r.direction == DoorDirection.down)
                {
                    posY = -1.5f; //もしdirectionがdown設定のRoomならプレイヤーの位置は下側になる
                }

                //プレイヤーの位置を決める
                player.transform.position = new Vector2(room.transform.position.x, room.transform.position.y + posY);
                break; //目的のRoomが見つかって、チェックの必要性がなくなったので直近の繰り返し構文foreachをやめる
            }
        }
    }

}