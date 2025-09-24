using Unity.VisualScripting;
using UnityEngine;

public class BillData : MonoBehaviour
{
    Rigidbody2D rbody;
    public int itemNum; //�A�C�e���̎��ʔԍ�
    CircleCollider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Rigidbody2D�R���|�[�l���g�̎擾
        rbody.bodyType = RigidbodyType2D.Static; //Rigidbody�̋�����Î~
        
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.bill++; //1���₷
            //�Y������擾�t���O��ON
            GameManager.itemsPickedState[itemNum] = true;

            //�A�C�e���擾���o
            //�@�R���C�_�[�𖳌���
            collider.enabled = false;




            //�ARigidbody2D�̕���(Dynamic�ɂ���)
            rbody.bodyType = RigidbodyType2D.Dynamic;


            //�B�ɑł��グ(�����5�ォ��)(�����5�̗�)
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);


            //�C�������g�𖕏�(0.5�b��)
            Destroy(gameObject, 0.5f);

        }
    
    }
}
