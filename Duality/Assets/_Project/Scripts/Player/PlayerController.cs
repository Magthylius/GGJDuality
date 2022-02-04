using System;
using Cinemachine;
using Duality.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Magthylius;
using MoreMountains.Feedbacks;

namespace Duality.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] 
        public PairElement YinElement;
        public PairElement YangElement;
        public FixedJoint2D joint;
        public CinemachineVirtualCamera VirtualCamera;
        public PlayerFollower mouseFollower;
        public PlayerFollower swapFollower;

        [Header("General Settings")] 
        public float pairDistance;
        public float mainMass = 1000f;
        public float subMass = 1f;
        public float mainScale;
        public float subScale;
        public float scaleLerp = 5f;

        [Header("Movement Settings")] 
        public float movementLerp = 5f;
        public float movementPower = 5f;
        public float endLinearDrag = 10f;
        public float maxMovementSpeed = 15f;
            
        [Header("Spin Settings")] 
        public float spinLerp = 5f;
        public float spinPower = 5f;
        public float endAngularDrag = 10f;
        public float maxSpinSpeed = 25f;

        [Header("Explosion power")] 
        public float explosionForce;
        public float explosionRadius;
        public ContactFilter2D explosiveFilter;
        public float spinSpeedThreshold;
        public Color chargedColor;

        private float _spinSpeedThresholdSqr;
        private bool _explosiveCharged = false;
        
        [Header("UI Settings")] 
        public MMFeedbacks helpStart;
        public MMFeedbacks helpEnd;

        [Header("Debug")] 
        public bool godMode;
        
        private PairElement _main;
        private PairElement _sub;
        private Rigidbody2D _mainRB;
        private Rigidbody2D _subRB;

        private PlayerMode _mode;
        private int _spinDir = 1;
        private bool _isSpinning = false;
        private bool _isMoving = false;
        
        private Vector2 _movementStep = Vector2.zero;
        private Vector2 _movementInput = Vector2.zero;
        
        private float _mouseClickStep = 0f;
        private float _mouseClickInput = 0f;

        public Action InitializedEvent;
        public Action DeathEvent;
        public Action RespawnEvent;

        private bool _lockInput = true;

        private void Start()
        {
            _main = YinElement;
            _sub = YangElement;

            DeathEvent += YinElement.OnDeath;
            DeathEvent += YangElement.OnDeath;
            RespawnEvent += YinElement.OnRespawn;
            RespawnEvent += YangElement.OnRespawn;
            CoreManager.GameStartedEvent += OnGameStart;
            CoreManager.GameEndedEvent += OnGameEnd;

            _spinSpeedThresholdSqr = MathEx.Square(spinSpeedThreshold);

            ResolvePairSettings();
            InitializedEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            _movementStep = MathEx.LerpSnap(_movementStep, _movementInput, movementLerp * Time.deltaTime, 0.99f);
            _mouseClickStep = MathEx.LerpSnap(_mouseClickStep, _mouseClickInput, spinLerp * Time.deltaTime, 0.99f);

            _mainRB.velocity = _movementStep * movementPower;
            _subRB.AddForce(_sub.transform.up * _mouseClickStep * spinPower * _spinDir, ForceMode2D.Force);
            _subRB.velocity = MathEx.MagnitudeCap(_subRB.velocity, maxSpinSpeed);

            _explosiveCharged = _subRB.SpeedSqr() > _spinSpeedThresholdSqr;
            if (_explosiveCharged)
                _sub.SetHollowColor(ColorEx.LerpSnap(_sub.hollowSprite.color, chargedColor, 5f * Time.deltaTime, 0.99f));
            else
                _sub.SetHollowColor(ColorEx.LerpSnap(_sub.hollowSprite.color, _sub.OriginalSpinColor, 5f * Time.deltaTime, 0.99f));
        }

        public void MovePos(Vector3 newPos)
        {
            //_main.transform.position = newPos;
            _main.rigidbody.MovePosition(newPos);
            _sub.rigidbody.MovePosition(_main.rigidbody.position + new Vector2(-3f, 0f));
        }
        
        public void ToggleMode()
        {
            _mode = _mode == PlayerMode.Yin ? PlayerMode.Yang : PlayerMode.Yin;
            ResolvePairSettings();
        }

        private void ResolvePairSettings()
        {
            switch (_mode)
            {
                case PlayerMode.Yin:
                    _main = YinElement;
                    _sub = YangElement;
                    _spinDir = 1;
                    break;
                
                case PlayerMode.Yang:
                    _main = YangElement;
                    _sub = YinElement;
                    _spinDir = -1;
                    break;
            }

            mouseFollower.follow = _main.transform;
            swapFollower.follow = _sub.transform;
            
            _main.ChangeMode(PairElementMode.Move);
            _sub.ChangeMode(PairElementMode.Spin);

            _main.transform.localScale = Vector3Ex.New(mainScale);
            _sub.transform.localScale = Vector3Ex.New(subScale);
            
            _mainRB = _main.rigidbody;
            _subRB = _sub.rigidbody;

            _subRB.mass = subMass;
            _subRB.angularDrag = _isSpinning ? 0f : endAngularDrag;
            _subRB.drag = 0f;
            
            _mainRB.mass = mainMass;
            _mainRB.angularDrag = 0f;
            _mainRB.drag = _isMoving ? 0f : endLinearDrag;

            VirtualCamera.Follow = _main.transform;
            VirtualCamera.LookAt = _main.transform;
        }

        public void Kill()
        {
            DeathEvent?.Invoke();
            CoreManager.Instance.ReportPlayerDeath();
            _lockInput = true;
            _movementInput = Vector2.zero;
            _mouseClickInput = 0f;
            //joint.enabled = false;
        }

        public void Respawn()
        {
            RespawnEvent?.Invoke();
            CoreManager.Instance.ReportPlayerSpawn();
            //joint.enabled = true;
        }

        public void OnGameStart()
        {
            _lockInput = false;
        }

        public void OnGameEnd()
        {
            _lockInput = true;
        }

        public void OnMovement(InputAction.CallbackContext callback)
        {
            if (_lockInput) return;;
            _movementInput = callback.ReadValue<Vector2>();

            if (callback.performed)
                _mainRB.drag = 0f;
            else if (callback.canceled)
                _mainRB.drag = endLinearDrag;

            _isMoving = callback.phase == InputActionPhase.Performed;
        }

        public void OnMouseClick(InputAction.CallbackContext callback)
        {
            if (_lockInput) return;
            _mouseClickInput = callback.ReadValue<float>();

            if (callback.performed)
            {
                _subRB.angularDrag = 0f;
                _subRB.drag = 0f;
            }
            else if (callback.canceled)
            {
                _subRB.angularDrag = endAngularDrag;
                _subRB.drag = endLinearDrag;
            }

            _isSpinning = callback.phase == InputActionPhase.Performed;
        }

        public void OnSwap(InputAction.CallbackContext callback)
        {
            if (_lockInput) return;
            if (callback.performed) ToggleMode();
        }

        public void OnHelp(InputAction.CallbackContext callback)
        {
            if (_lockInput) return;
            if (callback.performed)
            {
                helpEnd.StopFeedbacks();
                helpStart.PlayFeedbacks();
            }
            else if (callback.canceled)
            {
                helpStart.StopFeedbacks();
                helpEnd.PlayFeedbacks();
            }
        }
        
        public PlayerMode CurrentMode => _mode;
        public bool ExplosiveCharged => _explosiveCharged;

        public Transform MainTR => _main.transform;
    
        private Transform YinTR => YinElement.transform;
        private Transform YangTR => YangElement.transform;
    }
}
