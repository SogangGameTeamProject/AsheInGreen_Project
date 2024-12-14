using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace AshGreen.Sound
{
    public class tmp : MonoBehaviour
    {
        public float maxVolume = 0;
        public float minVolume = -40;
        public AudioMixer audioMixer;
        public Slider bgmSlider;
        public TextMeshProUGUI bgmText;
        public Slider sfxSlider;
        public TextMeshProUGUI sfxText;

        private void Start()
        {
            // 사운드 바 값 초기화
            float bgmValue;
            if (audioMixer.GetFloat("BGM", out bgmValue))
            {
                bgmSlider.value = bgmValue;
                float percentage = ((bgmValue - minVolume) / (maxVolume - minVolume)) * 100f;
                bgmText.text = percentage.ToString("F0") + "%";
            }
            else
            {
                Debug.LogError("BGM 파라미터를 찾을 수 없습니다.");
            }

            float sfxValue;
            if (audioMixer.GetFloat("SFX", out sfxValue))
            {
                sfxSlider.value = sfxValue;
                float percentage = ((sfxValue - minVolume) / (maxVolume - minVolume)) * 100f;
                sfxText.text = percentage.ToString("F0") + "%";
            }
            else
            {
                Debug.LogError("SFX 파라미터를 찾을 수 없습니다.");
            }

            // 슬라이더 값 변경 시 이벤트 등록
            bgmSlider.onValueChanged.AddListener(delegate { BGMControl(); });
            sfxSlider.onValueChanged.AddListener(delegate { SFXControl(); });
        }

        // BGM 컨트롤
        public void BGMControl()
        {
            float sound = bgmSlider.value;

            if (sound == minVolume) sound= -80;
audioMixer.SetFloat("BGM", sound);

            float percentage = ((sound - minVolume) / (maxVolume - minVolume)) * 100f;
            bgmText.text = sound > minVolume ? percentage.ToString("F0") + "%" : "0%";
        }

        // SFX 컨트롤
        public void SFXControl()
        {
            float sound = sfxSlider.value;

            if (sound == minVolume) sound = -80;
            audioMixer.SetFloat("SFX", sound);

            float percentage = ((sound - minVolume) / (maxVolume - minVolume)) * 100f;
            sfxText.text = sound > minVolume ? percentage.ToString("F0") + "%" : "0%";
        }
    }

}
