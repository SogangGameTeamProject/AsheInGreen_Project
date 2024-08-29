using AshGreen.Sound;
using UnityEngine;

namespace AshGreen.Player
{
    public abstract class PlayerStateBase : MonoBehaviour, PlayerState
    {
        protected PlayerController _player = null;
        protected SoundManager _soundManager = null;
        public AudioClip stateSoundClip = null;

        public virtual void Enter(PlayerController palyer)
        {
            //플레이어 초기화
            if (_player == null)
                _player = palyer;
            //사운드 매니저 초기화
            if (_soundManager == null)
                _soundManager = SoundManager.Instance;
            //사운드 재생
            if(_soundManager && stateSoundClip)
                _soundManager.PlaySFX(stateSoundClip);
        }

        public abstract void StateUpdate();

        public abstract void Exit();
    }
}

