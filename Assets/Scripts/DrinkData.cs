using UnityEngine;

public class DrinkData : MonoBehaviour
{
    Rigidbody2D rbody;
    public int itemNum;//�A�C�e���̎��ʔԍ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Player��HP���ő�Ȃ�Ȃɂ����Ȃ�
        if (collision.gameObject.CompareTag("Player"))
        {

             if(GameManager.playerHP < 3)
            {
                GameManager.playerHP++;
            }

            GameManager.itemsPickedState[itemNum] = true;

            //�A�C�e���擾�̉��o
            GetComponent<CircleCollider2D>().enabled = true;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
        }
        
    }
}
