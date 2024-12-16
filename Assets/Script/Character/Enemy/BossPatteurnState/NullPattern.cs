using AshGreen.Platform;
using AshGreen.Sound;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace AshGreen.Character{
    public class NullPattern : EnemyPatteurnStateInit
    {
        
        [SerializeField]
        private float firstDealay = 0.5f;//공격 선딜

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
            base.Exit();
        }

        protected override IEnumerator ExePatteurn()
        {
            yield return new WaitForSeconds(firstDealay);

            yield return base.ExePatteurn();
        }
    }
}
