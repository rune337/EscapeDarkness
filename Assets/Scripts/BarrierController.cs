using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f; //Á–Å‚·‚é‚Ü‚Å‚Ì‚ kƒ“

    void Start()
    {
        //deleteTime•bŒã‚ÉÁ–Å
        Destroy(gameObject, deleteTime);
    }

}