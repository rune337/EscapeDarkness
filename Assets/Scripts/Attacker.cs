using UnityEngine;

public class Attacker : MonoBehaviour
{
    PlayerController playerCnt;

    bool inAttack; //攻撃中ならtrue
    public float atackDelay;

    // ★★★ 攻撃エフェクトのプレハブをインスペクターから設定できるようにする ★★★
    public GameObject attackPrefab;
    // ★★★ プレイヤーからどれだけ前に出すかの距離 ★★★
    public float attackOffset = 1.0f;


    // public float deleteTime = 0f; // この機能は一旦コメントアウトします
    // public GameObject attackerPrefab; // attackPrefabと統合します

    void Start()
    {
        playerCnt = GetComponent<PlayerController>(); //コンポーネント取得 
        //deleteTime秒後に「攻撃エフェクト展開して消滅」
        // Invoke("FieldExpansion", deleteTime); // この機能はAttackとは別なので一旦コメントアウト
    }

    // Update is called once per frame
    void Update()
    {
        //Enterをおしたら前方に攻撃エリアを展開
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (inAttack || (GameManager.sword <= 0)) return;

        SoundManager.instance.SEPlay(SEType.Shoot); //お札を投げる音
        inAttack = true; //攻撃中

        // 1. プレイヤーの角度を入手
        float angleZ = playerCnt.angleZ;

        // 2. プレイヤーの向いている方向のベクトルを計算
        //    角度に対するX方向(cos)とY方向(sin)
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
        Vector2 forwardDir = new Vector2(x, y);

        // ★★★ 3. 攻撃エフェクトを生成する「位置」を計算 ★★★
        //    (Vector2)transform.position で現在の位置をVector2に変換
        //    それに「向いている方向 * 距離」を足す
        Vector2 spawnPos = (Vector2)transform.position + forwardDir * attackOffset;

        // ★★★ 4. 攻撃エフェクトの「回転」を計算 ★★★
        Quaternion q = Quaternion.Euler(0, 0, angleZ);

        // ★★★ 5. 計算した「位置」と「回転」でプレハブを生成 ★★★
        Instantiate(attackPrefab, spawnPos, q);

        //時間差で攻撃中フラグを解除
        Invoke("StopAttack", atackDelay);
    }

    void StopAttack()
    {
        inAttack = false; //攻撃中フラグをOFFにする
    }

    /*
    //攻撃エフェクト展開と自己消滅を行うメソッド
    //これはAttack()とは別の、自動発動するスキルのようなので、一旦分けておきます
    void FieldExpansion()
    {
        Instantiate(attackerPrefab, transform.position, Quaternion.identity); 
        Destroy(gameObject); 
    }
    */
}