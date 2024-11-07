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
            //가운데로 위치 이동

            //파도 소환

            //갈메기 소환

            //갈메기 연속 공격

            //원래 위치로 이동

            yield return base.ExePatteurn();
        }
    }
}
