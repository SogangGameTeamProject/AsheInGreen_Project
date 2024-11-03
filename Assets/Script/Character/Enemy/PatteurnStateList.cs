using AshGreen.Sound;
using AshGreen.State;
using Unity.Netcode;
using UnityEngine;

namespace AshGreen.Character
{
    public abstract class EnemyPatteurnStateInit : NetworkBehaviour, IState<EnemyController>
    {
        protected EnemyController _enemy = null;
        protected SoundManager _soundManager = null;
        public AudioClip stateSoundClip = null;
        protected Animator _animator = null;

        private int nextPatteurnIndex = 0;

        public virtual void Enter(EnemyController controller)
        {
            //플레이어 초기화
            if (_enemy == null)
                _enemy = controller;
            //사운드 매니저 초기화
            if (_soundManager == null)
                _soundManager = SoundManager.Instance;
            //사운드 재생
            if(_soundManager && stateSoundClip)
                _soundManager.PlaySFX(stateSoundClip);
            //애니메이터 초기화
            if(_animator == null)
                _animator = _enemy.GetComponent<Animator>();
        }

        public abstract void StateUpdate();

        public abstract void Exit();
    }
}

