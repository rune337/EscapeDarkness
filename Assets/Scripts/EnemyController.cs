using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 移動スピード
    public float speed = 0.5f; // 反応距離
    public float reactionDistance = 4.0f;
    float axisH;                //横軸値(-1.0 ~ 0.0 ~ 1.0)
    float axisV;                //縦軸値(-1.0 ~ 0.0 ~ 1.0)

    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator

    bool isActive = false;      //アクティブフラグ
    public int arrangeId = 0;   //配置の識別に使う


    public bool onBarrier; //バリアにあたっているか
    public bool onAttacker;
    GameObject player; //プレイヤー情報
    bool inDamage; //ダメージ中かどうかのフラグ管理
    public int enemyHP = 3;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();    // Rigidbody2Dを得る
        animator = GetComponent<Animator>();    //Animatorを得る
        player = GameObject.FindGameObjectWithTag("Player"); //プレイヤー情報を得る

    }

    void Update()
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier || onAttacker) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        //移動値初期化
        axisH = 0;
        axisV = 0;

        // プレイヤーとの距離チェック
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < reactionDistance)
        {
            isActive = true;    // アクティブにする
        }
        else
        {
            isActive = false;    // 非アクティブにする
        }

        // アニメーションを切り替える
        animator.SetBool("IsActive", isActive);

        if (isActive)
        {
            animator.SetBool("IsActive", isActive);
            // プレイヤーへの角度を求める

            float dx = player.transform.position.x - transform.position.x;
            float dy = player.transform.position.y - transform.position.y;

            float rad = Mathf.Atan2(dy, dx);
            float angle = rad * Mathf.Rad2Deg;

            // 移動角度でアニメーションを変更する
            int direction;
            if (angle > -45.0f && angle <= 45.0f)
            {
                direction = 3;    //右向き
            }
            else if (angle > 45.0f && angle <= 135.0f)
            {
                direction = 2;    //上向き
            }
            else if (angle >= -135.0f && angle <= -45.0f)
            {
                direction = 0;    //下向き
            }
            else
            {
                direction = 1;    //左向き
            }

            animator.SetInteger("Direction", direction);
            // 移動するベクトルを作る
            axisH = Mathf.Cos(rad);
            axisV = Mathf.Sin(rad);
        }

    }

    void FixedUpdate()
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier || onAttacker)
        {
            rbody.linearVelocity = Vector2.zero;
            Debug.Log("バリアまたは攻撃を受けている");
            return;
        }

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        if (isActive)
        {
            // 移動
            rbody.linearVelocity = new Vector2(axisH, axisV).normalized * speed;
        }
        else
        {
            rbody.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = true;
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            onAttacker = true;
            // 攻撃してきたオブジェクト(collision.gameObject)を引数に渡す
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject attacker)
    {
        //ステータスがplayingでなければ何もせず終わり
        if (GameManager.gameState != GameState.playing) return;

        SoundManager.instance.SEPlay(SEType.Damage); //ダメージを受ける音

        enemyHP--; //敵HPを1減らす

        if (enemyHP > 0)
        {
            //そこまでの敵の動きをいったんストップ
            rbody.linearVelocity = Vector2.zero; //new Vector2(0,0)
            //攻撃者と敵との差を取得し、方向を決める
            Vector3 v = (transform.position - player.transform.position).normalized;
            //決まった方向に押される
            rbody.AddForce(v * 4, ForceMode2D.Impulse);
            

            //点滅するためのフラグ
            inDamage = true;

            //時間差で0.25秒後に点滅フラグ解除
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            //残HPが残っていなければ消える処理
            EnemyDie();
        }
    }

    void DamageEnd()
    {
        inDamage = false; //点滅ダメージフラグを解除
        gameObject.GetComponent<SpriteRenderer>().enabled = true; //プレイヤーを確実に表示
    }

    void EnemyDie()
    {
        Debug.Log("敵を倒した！");
        GetComponent<CircleCollider2D>().enabled = false; //当たり判定の無効化
        rbody.linearVelocity = Vector2.zero; //動きを止める
        rbody.gravityScale = 1.0f; //重力の復活
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); //上に跳ね上げる
        Destroy(gameObject, 1.0f); //1秒後に存在を消去
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = false;
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            onAttacker = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }



}
