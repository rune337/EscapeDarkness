using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // ヒットポイント
    public int hp = 5;

    public GameObject bulletPrefab;     //弾
    public float shootSpeed = 5.0f;     //弾の速度

    public bool onBarrier; //バリアにあたっているか
    GameObject player; //プレイヤー情報

    public float speed = 0.5f; // スピード
    float axisH;                //横軸値(-1.0 ~ 0.0 ~ 1.0)
    float axisV;                //縦軸値(-1.0 ~ 0.0 ~ 1.0)

    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();    // Rigidbody2Dを得る
        animator = GetComponent<Animator>();    //Animatorを得る
        player = GameObject.FindGameObjectWithTag("Player"); //プレイヤー情報を得る

        animator.SetBool("Active", true);
    }

    // Update is called once per frame
    void Update()
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;

        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;

        // 移動するベクトルを作る
        axisH = Mathf.Cos(rad) * speed;
        axisV = Mathf.Sin(rad) * speed;
    }

    void FixedUpdate()
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        //バリアに触れている時は何もしない
        if (onBarrier)
        {
            rbody.linearVelocity = Vector2.zero;

            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                //描画機能を有効
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //描画機能を無効
                GetComponent<SpriteRenderer>().enabled = false;
            }

            return;
        }

        // 移動
        rbody.linearVelocity = new Vector2(axisH, axisV).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            if (onBarrier) return;

            hp--;
            if(hp > 0)
            {
                onBarrier = true;
                StartCoroutine(Damaged());
            }
            else
            {
                if(GameManager.gameState == GameState.playing) StartCoroutine(StartEnding());
            }
        }
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(5);
        onBarrier = false;
        //描画機能を有効
        GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator StartEnding()
    {
        //ゲームエンド
        animator.SetTrigger("Dead");
        rbody.linearVelocity = Vector2.zero;
        GameManager.gameState = GameState.ending;
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Ending");
    }

    //攻撃　Attackアニメーションにつく
    void Attack()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;
        //弾を発射するベクトルを作る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg;
            //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //発射
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
