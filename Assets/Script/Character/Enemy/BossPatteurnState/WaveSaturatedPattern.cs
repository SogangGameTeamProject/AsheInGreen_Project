using System.Collections;
using UnityEngine;

namespace AshGreen.Character{
    public class WaveSaturatedPattern : EnemyPatteurnStateInit
    {
        public override void Enter(EnemyController controller)
        {
            base.Enter(controller);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
        }

        public override void Exit()
        {

        }

        protected override IEnumerator ExePatteurn()
        {
            //가운데로 이동

            //넉백이 강한 파도 소환

            //기존 바닥 플렛폼 소환

            //갈메기 플렛폼 소환

            //갈메기 공격

            //원위치 이동

            yield return base.ExePatteurn();
        }
    }
}
