using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public PlayerController playerCnt; //PlayerControllerコンポーネント
    public float rotationSpeed = 20.0f; //スポットライトの回転速度
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //寸前までのスポット来の回転値(Z軸のみ取得)
        //float currentAngle = transform.eulerAngles.z;

        //プレイヤーの角度
        float targetAngle = playerCnt.angleZ;

        //ターゲットとなる角度を調整
        Quaternion targetRotation = Quaternion.Euler(0, 0, (targetAngle - 90));

        //現在の回転を(寸前回転)→(ターゲットの回転)になるようになめらかに補完する Quaternion.Slerpメソッド
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}
