using System.Collections;
using UnityEngine;

namespace AshGreen.Character{
    public class ShootDownAttackPattern : EnemyPatteurnStateInit
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
            //흡수 공격

            //갈매기 소환 코루틴 호출

            //플랫폼 이동 상태로 전환

            //갈매기 플랫폼 공격


            yield return base.ExePatteurn();
        }
    }
}
