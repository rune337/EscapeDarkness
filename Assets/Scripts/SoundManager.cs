using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//BGMタイプ
public enum BGMType
{
    None,
    Title,
    InGame,
    InBoss
}

//SEタイプ
public enum SEType
{
    Shoot,
    Damage,
    Door,
    DoorOpen,
    Walk,
    Barrier
}

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmInTitle; //タイトルBGM
    public AudioClip bgmInGame; //ゲーム中
    public AudioClip bgmInBoss; //ボス

    public AudioClip seShoot;
    public AudioClip seDamage;
    public AudioClip seDoor;
    public AudioClip seDoorOpen;
    public AudioClip seWalk;
    public AudioClip seBarrier;

    public static SoundManager instance; // シングルトンインスタンス
    public static BGMType playingBGM = BGMType.None; //再生中のBGM

    AudioSource audio;

    void Awake()
    {
        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンが切り替わっても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audio = GetComponent<AudioSource>();

    }

    //BGM再生
    public void PlayBgm(BGMType type)
    {
        if(type != playingBGM)
        {
            playingBGM = type;


            switch (type)
            {
                case BGMType.Title:
                    audio.clip = bgmInTitle;
                    audio.Play();
                    break;
                case BGMType.InGame:
                    audio.clip = bgmInGame;
                    audio.Play();
                    break;
                case BGMType.InBoss:
                    audio.clip = bgmInBoss;
                    audio.Play();
                    break;
            }
        }
    }

    //SE再生
    public void SEPlay(SEType type)
    {
        switch (type)
        {
            case SEType.Shoot:
                audio.PlayOneShot(seShoot);
                break;
            case SEType.Damage:
                audio.PlayOneShot(seDamage);
                break;
            case SEType.Door:
                audio.PlayOneShot(seDoor);
                break;
            case SEType.DoorOpen:
                audio.PlayOneShot(seDoorOpen);
                break;
            case SEType.Walk:
                audio.PlayOneShot(seWalk);
                break;
            case SEType.Barrier:
                audio.PlayOneShot(seBarrier);
                break;
        }
    }

    //停止メソッド
    public void StopBgm()
    {
        audio.Stop();
        playingBGM = BGMType.None;
    }

}
