using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f; //���ł���܂ł̎���k��

    void Start()
    {
        //deleteTime�b��ɏ���
        Destroy(gameObject, deleteTime);
    }

}