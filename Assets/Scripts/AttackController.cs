using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float deleteTime = 1.0f; //攻撃消滅するまでの時間
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //deleteTime秒後に消滅
        Destroy(gameObject, deleteTime);
    }
}
