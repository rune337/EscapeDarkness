using System.Collections;
using UnityEngine;

public class BarrierGenerator : MonoBehaviour
{
    bool isBarrier;
    
    public GameObject barrierPrefab;
    public GameObject light;
    public float interval;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!isBarrier) StartCoroutine(BarrierStart());
        }
    }

    IEnumerator BarrierStart()
    {
        light.SetActive(false);
        isBarrier = true;
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(interval);
        isBarrier = false;
        light.SetActive(true);
    }



}
