using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Magthylius;

namespace Duality.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] 
        public PairElement YinElement;
        public PairElement YangElement;
        public CinemachineVirtualCamera VirtualCamera;

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

        private void Start()
        {
            Vector3 halfDistance = new Vector3(pairDistance * 0.5f, 0f, 0f);
            YinTR.position = halfDistance;
            YangTR.position = -halfDistance;
            _main = YinElement;
            _sub = YangElement;

            DeathEvent += YinElement.OnDeath;
            DeathEvent += YangElement.OnDeath;

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
        }

        public void OnMovement(InputAction.CallbackContext callback)
        {
            _movementInput = callback.ReadValue<Vector2>();

            if (callback.performed)
                _mainRB.drag = 0f;
            else if (callback.canceled)
                _mainRB.drag = endLinearDrag;

            _isMoving = callback.phase == InputActionPhase.Performed;
        }

        public void OnMouseClick(InputAction.CallbackContext callback)
        {
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
            if (callback.performed) ToggleMode();
        }
        
        public PlayerMode CurrentMode => _mode;

        public Transform MainTR => _main.transform;
    
        private Transform YinTR => YinElement.transform;
        private Transform YangTR => YangElement.transform;
    }
}
