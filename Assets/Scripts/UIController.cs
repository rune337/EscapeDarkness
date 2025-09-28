using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI key1Text; //対象コンポーネント
    public TextMeshProUGUI key2Text; //対象コンポーネント
    public TextMeshProUGUI key3Text; //対象コンポーネント
    public TextMeshProUGUI billText; //対象コンポーネント
    public TextMeshProUGUI swordText;//対象コンポーネント

    public Slider playerHP;  //対象スライダー

    int currentKey1Count; //差分
    int currentKey2Count; //差分
    int currentKey3Count; //差分
    int currentBillCount; //差分
    int currentSwordCount; //差分
    int currentHPCount; //差分

    void Start()
    {
        currentKey1Count = GameManager.key1; //初期設定
        currentKey2Count = GameManager.key2; //初期設定
        currentKey3Count = GameManager.key3; //初期設定
        currentBillCount = GameManager.bill; //初期設定
        currentSwordCount = GameManager.sword; //初期設定
        currentHPCount = GameManager.playerHP; //初期設定

        key1Text.text = currentKey1Count.ToString(); //UIに反映
        key2Text.text = currentKey2Count.ToString(); //UIに反映
        key3Text.text = currentKey3Count.ToString(); //UIに反映
        billText.text = currentBillCount.ToString(); //UIに反映
        swordText.text = currentSwordCount.ToString(); //UIに反映
        playerHP.value = currentHPCount; //UIに反映
    }

    void Update()
    {
        //key1の数に変化があれば
        if (currentKey1Count != GameManager.key1)
        {
            currentKey1Count = GameManager.key1;
            key1Text.text = currentKey1Count.ToString();
        }

        //key2の数に変化があれば
        if (currentKey2Count != GameManager.key2)
        {
            currentKey2Count = GameManager.key2;
            key2Text.text = currentKey2Count.ToString();
        }

        //key3の数に変化があれば
        if (currentKey3Count != GameManager.key3)
        {
            currentKey3Count = GameManager.key3;
            key3Text.text = currentKey3Count.ToString();
        }

        //お札の数に変化があれば
        if (currentBillCount != GameManager.bill)
        {
            currentBillCount = GameManager.bill;
            billText.text = currentBillCount.ToString();
        }

        //剣の数に変化があれば
        if (currentSwordCount != GameManager.sword)
        {
            currentSwordCount = GameManager.sword;
            swordText.text = currentSwordCount.ToString();
        }

        //PlayerのHPに変化があれば
        if (currentHPCount != GameManager.playerHP)
        {
            currentHPCount = GameManager.playerHP;
            playerHP.value = currentHPCount;
        }

    }
}
