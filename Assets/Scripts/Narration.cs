using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Narration : MonoBehaviour
{
    public MessageData message;
    public TextMeshProUGUI messageText;

    public bool isEnding; //エンディングシーンで使われるかどうか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TalkStart());
    }

    IEnumerator TalkStart()
    {
        //対象としたScriptbleObject(変数message)が扱っている配列msgArrayの数だけ繰り返す
        for (int i = 0; i < message.msgArray.Length; i++)
        {
            messageText.text = message.msgArray[i].message;

            //yield return new WaitForSeconds(0.1f); //0.1秒待つ
            yield return new WaitForSecondsRealtime(0.1f); //0.1秒待つ

            while (!Input.GetKeyDown(KeyCode.E))
            { //Eキーがおされるまで
                yield return null; //何もしない
            }
        }

        yield return new WaitForSeconds(3.0f);

        if (isEnding)
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }


}
