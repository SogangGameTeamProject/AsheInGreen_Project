using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Sound
{
    public class BGMPlayerController : MonoBehaviour
    {
        public AudioClip bgmClip;// 재생할 BGM
        void Start()
        {
            if (bgmClip != null)
                SoundManger.Instance.PlayBGM(bgmClip);
        }
    }

}
