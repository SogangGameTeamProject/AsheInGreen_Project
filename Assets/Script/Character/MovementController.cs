using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using System;
using Unity.Mathematics;
using Unity.Netcode.Components;
using System.Collections;
using AshGreen.State;
using Unity.VisualScripting;

namespace AshGreen.Character
{
    //이동 상태 타입
    public enum MovementStateType
    {
        Null = -1, Idle = 0, Move = 1, Unable = 2, Jump = 3,
    }

    public class MovementController : NetworkBehaviour
    {
        private CharacterController _character;
        private Rigidbody2D rBody = null;

        private Animator _characterAnimator;
        private NetworkAnimator _networkAnimator;

        public bool isGrounded = false;//땅위 체크
        public Collider2D groundChecker = null;
        public LayerMask groundLayer;//땅 레이어
        public bool isPlatformed = false;//플렛폼 위 체크
        public LayerMask platformLayer;//플렛폼 레이어

        public bool isUnableMove = false;//이동 불가 상태
        //넉백 관련
        private Coroutine nockbackCorutine = null;

        //충돌 관련
        public float downJumpTime = 0.25f;

        //------이동 상태-------
        public MovementStateType runningMovementStateType = MovementStateType.Null;
        private StateContext<CharacterController> movementStateContext = null;

        //상태 정보 관리를 위한 클래스
        [System.Serializable]
        public class MovementStateData
        {
            public MovementStateType type;//트리거할 타입
            public CharacterStateBase state;//실행할 상태
        }
        public List<MovementStateData> movementStateList//상태 관리를 위한 리스트
            = new List<MovementStateData>();

        //각 이동관련 행동을 실행하는 액션 선언
        public event Action<Vector2, float> MoveAction;
        public event Action<float> JumpAction;
        public event Action<float> DownJumpAction;
        public event Action<Vector2, float, float> NockBackAction;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _character = GetComponent<CharacterController>();
            rBody = GetComponent<Rigidbody2D>();
            _characterAnimator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();

            movementStateContext = new StateContext<CharacterController>(_character);//콘텍스트 생성

            MovementStateTransitionRpc(MovementStateType.Idle);

            //액션 초기화
            MoveAction += OnMove;
            JumpAction += OnJump;
            DownJumpAction += OnDownJump;
            NockBackAction += OnNockBack;
        }


        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            //액션 제거
            MoveAction -= OnMove;
            JumpAction -= OnJump;
            DownJumpAction -= OnDownJump;
            NockBackAction -= OnNockBack;
        }

        void Update()
        {
            if (IsOwner)
                MoveAniUpdate();
            CheckIfGrounded();
            movementStateContext.StateUpdate();
        }

        // 이동 시 애니메이션 파라미터 업데이트
        
        private void MoveAniUpdate()
        {
            _characterAnimator.SetFloat("VelocityAbcX", Mathf.Abs(rBody.linearVelocityX));
            _characterAnimator.SetFloat("VelocityY", rBody.linearVelocityY);
            _characterAnimator.SetBool("IsGrounded", isGrounded);
            _characterAnimator.SetFloat("NowHp", _character.NowHP);
        }

        // 땅에 있는지 확인하는 함수
        void CheckIfGrounded()
        {
            // groundChecker가 땅(LayerMask)에 닿았는지 여부를 확인
            isGrounded = groundChecker.IsTouchingLayers(groundLayer);
            if (isGrounded)
                _character.jumCnt = 0;
            isPlatformed = groundChecker.IsTouchingLayers(platformLayer);
        }



        //-----이동 상태 관련 함수-----
        //이동 상태 변환 함수
        [Rpc(SendTo.ClientsAndHost)]
        public void MovementStateTransitionRpc(MovementStateType type)
        {
            IState<CharacterController> state = null;
            MovementStateData findState = movementStateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<IState<CharacterController>>();
                runningMovementStateType = findState.type;
                movementStateContext.TransitionTo(state);
            }
        }

        /// <summary>
        /// 이동 관련 기능 수행을 호출받아 이벤트를 실행시키는 메서드들
        /// </summary>
        public void ExecuteMove(Vector2 moveVec, float moveSpeed)
        {
            if(!isUnableMove)
                MoveAction?.Invoke(moveVec, moveSpeed);
        }
        
        public void ExecutJump(float power)
        {
            JumpAction?.Invoke(power);
        }

        public void ExecutDownJump(float power)
        {
            DownJumpAction?.Invoke(power);
        }

        public void ExcutNockBack(Vector2 nockbackArrow, float power, float time)
        {
            if(IsOwner)
                NockBackAction?.Invoke(nockbackArrow, power, time);
        }

        /// <summary>
        /// 캐릭터 이동관련 기능구현 메서드 초기화
        /// 각 메서드들은 액션에 등록되어 작동
        /// </summary>
        //이동 구현
        private void OnMove(Vector2 moveVec, float moveSpeed)
        {
            if (rBody)
            {
                rBody.linearVelocityX = moveVec.x * moveSpeed;
            }
        }

        //점프 구현 함수
        private void OnJump(float power)
        {
            if(!isGrounded)
                _character.jumCnt++;

            if (rBody)
            {
                rBody.linearVelocityY = 0;
                rBody.AddForce(Vector2.up * power, ForceMode2D.Impulse);
            }
        }


        //다운 점프 구현 함수
        private void OnDownJump(float power)
        {
            if (rBody)
            {
                rBody.linearVelocity = Vector2.zero;
                rBody.AddForce(Vector2.down * power, ForceMode2D.Impulse);
            }

            StartCoroutine(ConflictAdjustment(downJumpTime));
        }

        //다운 점프 구현 코루틴
        IEnumerator ConflictAdjustment(float enableTIme)
        {
            SetCollisionWithLayer(platformLayer, true);
            yield return new WaitForSeconds(enableTIme);
            SetCollisionWithLayer(platformLayer, false);
        }

        //넉백 구현 함수
        private void OnNockBack(Vector2 vector2, float power, float time)
        {
            if (rBody && !_character.isDamageImmunity.Value)
            {
                if(nockbackCorutine != null)
                    StopCoroutine(nockbackCorutine);
                StartCoroutine(NockBackTreatment(vector2, power, time));

            }
        }
        
        //넉백 종료 처리 코루틴
        private IEnumerator NockBackTreatment(Vector2 vector2, float power, float time)
        {
            MovementStateTransitionRpc(MovementStateType.Unable);
            isUnableMove = true;
            rBody.linearVelocity = Vector2.zero;
            rBody.AddForce(vector2 * power, ForceMode2D.Impulse);

            yield return new WaitForSeconds(time);

            MovementStateTransitionRpc(MovementStateType.Idle);
            isUnableMove = false;
        }

        // 특정 레이어와의 충돌을 켜고 끌 수 있는 메서드
        public void SetCollisionWithLayer(LayerMask targetLayer, bool enable)
        {
            if(enable)
                rBody.excludeLayers |= targetLayer;
            else 
                rBody.excludeLayers &= ~targetLayer;
        }
    }
}