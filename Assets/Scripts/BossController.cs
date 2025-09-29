using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // ヒットポイント
    public int hp = 30;

    public GameObject bulletPrefab;     //弾
    public float shootSpeed = 5.0f;     //弾の速度

    public bool onBarrier; //バリアにあたっているか
    // public bool onAttacker; // onAttackerは不要なため削除
    bool inDamage; // ★★★ ダメージ中の無敵状態を管理する専用フラグを追加

    GameObject player; //プレイヤー情報
    public float speed = 0.5f; // スピード
    float axisH;                //横軸値
    float axisV;                //縦軸値

    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        animator.SetBool("Active", true);
    }

    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        // ★★★ 停止条件を inDamage と onBarrier に変更
        if (inDamage || onBarrier) return;

        if (player == null) return;

        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);

        // 移動するベクトルを作る
        axisH = Mathf.Cos(rad); // speedを掛けるのはFixedUpdateで行う
        axisV = Mathf.Sin(rad);
    }

    void FixedUpdate()
    {
        if (GameManager.gameState != GameState.playing) return;
        if (player == null) return;

        // ★★★ 停止条件を inDamage と onBarrier に変更
        if (inDamage || onBarrier)
        {
            rbody.linearVelocity = Vector2.zero;

            // 点滅処理はダメージ中のみ行う
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);
                GetComponent<SpriteRenderer>().enabled = val > 0;
            }
            return;
        }

        // 移動
        rbody.linearVelocity = new Vector2(axisH, axisV).normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ★★★ ダメージ処理のロジックを全面的に修正 ★★★

        // バリアに触れたときの処理
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = true;
            // 必要であればバリア接触時のリアクションをここに書く
        }

        // 攻撃に触れたときの処理
        if (collision.gameObject.CompareTag("Attack"))
        {
            // 既にダメージ中でなければ（無敵時間中でなければ）
            if (!inDamage)
            {
                TakeDamage(); // ダメージを受ける処理を呼び出す
            }
        }
    }

    // ★★★ OnTriggerExit2Dを新設してバリアから離れたことを検知 ★★★
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = false;
        }
    }

    // ★★★ ダメージを受ける処理を独立したメソッドに分離 ★★★
    void TakeDamage()
    {
        hp--;
        Debug.Log("ボスがダメージを受けた！ 残りHP: " + hp);

        if (hp > 0)
        {
            // ダメージリアクション（無敵時間の開始）
            StartCoroutine(DamageRoutine());
        }
        else
        {
            // 死亡処理
            if (GameManager.gameState == GameState.playing)
            {
                StartCoroutine(StartEnding());
            }
        }
    }

    // ★★★ Damagedコルーチンをダメージ専用に修正 ★★★
    IEnumerator DamageRoutine()
    {
        inDamage = true; // 無敵開始
        yield return new WaitForSeconds(1.0f); // 無敵時間（5秒は長すぎるので1秒に調整）
        inDamage = false; // 無敵終了
        GetComponent<SpriteRenderer>().enabled = true; // 必ず表示状態に戻す
    }

    IEnumerator StartEnding()
    {
        GameManager.gameState = GameState.ending;
        animator.SetTrigger("Dead");
        rbody.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false; // 当たり判定を消す
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Ending");
    }

    void Attack()
    {
        if (onBarrier || inDamage)
        {
            return; // 弾を撃たずにメソッドを終了
        }

        // ... (攻撃処理は変更なし) ...
        Transform tr = transform.Find("gate");
        if (tr == null || player == null) return;

        GameObject gate = tr.gameObject;
        float dx = player.transform.position.x - gate.transform.position.x;
        float dy = player.transform.position.y - gate.transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;
        Quaternion r = Quaternion.Euler(0, 0, angle);
        GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);

        float x = Mathf.Cos(rad);
        float y = Mathf.Sin(rad);
        Vector3 v = new Vector3(x, y) * shootSpeed;

        Rigidbody2D bulletRbody = bullet.GetComponent<Rigidbody2D>();
        if (bulletRbody != null)
        {
            bulletRbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}