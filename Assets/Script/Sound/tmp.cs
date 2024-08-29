using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AshGreen.Sound
{
    public class tmp : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Slider bgmSlider;
        public Slider sfxSlider;

        private void Start()
        {
            //사운드 바 값 초기화
            float bgmValue;
            audioMixer.GetFloat("BGM", out bgmValue);
            bgmSlider.value = bgmValue;
            float sfxValue;
            audioMixer.GetFloat("SFX", out sfxValue);
            sfxSlider.value = sfxValue;
        }

        //BGM 컨트롤
        public void BGMControl()
        {
            float sound = bgmSlider.value;

            if (sound == -40f) audioMixer.SetFloat("BGM", -80);
            else audioMixer.SetFloat("BGM", sound);
            Debug.Log(sound);
        }

        //SFX 컨트롤
        public void SFXControl()
        {
            float sound = sfxSlider.value;

            if (sound == -40f) audioMixer.SetFloat("SFX", -80);
            else audioMixer.SetFloat("SFX", sound);
        }
    }

}
