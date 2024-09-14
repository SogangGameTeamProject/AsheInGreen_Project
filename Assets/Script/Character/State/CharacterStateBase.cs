using AshGreen.Sound;
using AshGreen.State;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public abstract class CharacterStateBase : NetworkBehaviour, IState<CharacterController>
    {
        protected CharacterController _character = null;
        protected SoundManager _soundManager = null;
        public AudioClip stateSoundClip = null;
        public Animator _animator = null;

        public virtual void Enter(CharacterController character)
        {
            //플레이어 초기화
            if (_character == null)
                _character = character;
            //사운드 매니저 초기화
            if (_soundManager == null)
                _soundManager = SoundManager.Instance;
            //사운드 재생
            if(_soundManager && stateSoundClip)
                _soundManager.PlaySFX(stateSoundClip);
            //애니메이터 초기화
            if(_animator == null)
                _animator = _character.GetComponent<Animator>();
        }

        public abstract void StateUpdate();

        public abstract void Exit();
    }
}

