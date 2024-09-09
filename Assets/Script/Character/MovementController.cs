using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using System;
using Unity.Mathematics;
using Unity.Netcode.Components;
using System.Collections;

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

        //충돌 관련
        public Collider2D[] playerCol;
        public float downJumpTime = 0.25f;

        //------이동 상태-------
        public MovementStateType runningMovementStateType;
        private CharacterStateContext movementStateContext = null;

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
        public Action<Vector2, float> OnMoveAction;
        public Action<float> OnJumpAction;
        public Action<float> OnDownJumpAction;

        private void Start()
        {
            _character = GetComponent<CharacterController>();
            rBody = GetComponent<Rigidbody2D>();
            _characterAnimator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();

            movementStateContext = new CharacterStateContext(_character);//콘텍스트 생성
            MovementStateInit(MovementStateType.Idle);

            //액션 초기화
            OnMoveAction += OnMove;
            OnJumpAction += OnJump;
            OnDownJumpAction += OnDownJump;
        }

        void Update()
        {
            if (IsOwner)
                MoveAniUpdate();
            CheckIfGrounded();
        }

        private void FixedUpdate()
        {
            movementStateContext.StateUpdate();
        }

        // 이동 시 애니메이션 파라미터 업데이트
        private void MoveAniUpdate()
        {
            _characterAnimator.SetFloat("VelocityAbcX", Mathf.Abs(rBody.linearVelocityX));
            _characterAnimator.SetFloat("VelocityY", rBody.linearVelocityY);
            _characterAnimator.SetBool("IsGrounded", isGrounded);

            // NetworkAnimator로 파라미터 변화를 동기화
            _networkAnimator.SetTrigger("ParameterUpdated");
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
        //이동 상태 초기화 함수
        public void MovementStateInit(MovementStateType type)
        {
            CharacterState state = null;
            MovementStateData findState = movementStateList.Find(state => state.type.Equals(type));
            Debug.Log(findState);
            if (findState != null)
            {
                state = findState.state.GetComponent<CharacterState>();
                runningMovementStateType = findState.type;
                movementStateContext.Initialize(state);
            }
            
        }
        //이동 상태 변환 함수
        public void MovementStateTransition(MovementStateType type)
        {
            CharacterState state = null;
            MovementStateData findState = movementStateList.Find(state => state.type.Equals(type));
            if (findState != null)
            {
                state = findState.state.GetComponent<CharacterState>();
                runningMovementStateType = findState.type;
                movementStateContext.TransitionTo(state);
            }
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
                rBody.linearVelocity = Vector2.zero;
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
        IEnumerator ConflictAdjustment(float enableTIme)
        {
            SetCollisionWithLayer(platformLayer, false);
            yield return new WaitForSeconds(enableTIme);
            SetCollisionWithLayer(platformLayer, true);

        }

        // 특정 레이어와의 충돌을 켜고 끌 수 있는 메서드
        public void SetCollisionWithLayer(LayerMask targetLayer, bool enable)
        {
            // playerCol 배열에 있는 모든 콜라이더에 대해
            foreach (Collider2D col in playerCol)
            {
                if (col != null)
                {
                    // targetLayer와의 충돌을 끄거나 켜는 동작 수행
                    int layer1 = col.gameObject.layer;
                    int layer2 = (int)Mathf.Log(targetLayer.value, 2); // 타겟 레이어의 숫자를 계산
                    Physics2D.IgnoreLayerCollision(layer1, layer2, !enable);
                }
            }
        }
    }
}