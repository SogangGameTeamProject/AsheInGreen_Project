using System.Collections;
using UnityEngine;

namespace AshGreen.Character
{
    public class HitState : CharacterStateBase
    {
        public float hitTIme = 0.6f;
        public float flashCycle = 0.05f;
        private SpriteRenderer spriteRenderer;
        private Color originalColor = Color.white; // 원래 색상 저장
        private Coroutine plashCoroutine = null;
        public override void Enter(CharacterController character)
        {
            base.Enter(character);
            if(spriteRenderer == null )
                spriteRenderer = _character.GetComponent<SpriteRenderer>();

            plashCoroutine = StartCoroutine(FlashCharacter());
        }

        public override void StateUpdate()
        {
            
        }

        public override void Exit()
        {
            //번쩍임 취소
            if (plashCoroutine != null)
            {

                StopCoroutine(plashCoroutine);
                plashCoroutine = null;
                spriteRenderer.color = originalColor;
            }
        }

        private IEnumerator FlashCharacter()
        {
            float elapsedTime = 0f;
            bool isFlash = true;

            while (elapsedTime < hitTIme)
            {
                // 캐릭터의 투명/가시 상태 전환
                if (isFlash)
                {
                    spriteRenderer.color = Color.red; // 투명하게
                }
                else
                {
                    spriteRenderer.color = originalColor; // 원래 색상으로
                }

                // 다음 번쩍임을 위해 상태 전환
                isFlash = !isFlash;

                // 주기 동안 대기
                yield return new WaitForSeconds(flashCycle);

                // 경과 시간 증가
                elapsedTime += flashCycle;
            }

            // 번쩍거림이 끝난 후, 원래 상태로 복구
            spriteRenderer.color = originalColor;
            if(_character.NowHP > 0)
                _character.CombatStateTransitionServerRpc(CombatStateType.Idle);
            else
                _character.CombatStateTransitionServerRpc(CombatStateType.Death);

        }
    }
}