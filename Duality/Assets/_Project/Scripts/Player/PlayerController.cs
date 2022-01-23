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
        public Transform Yin;
        public TrailRenderer YinMoveTrail;
        public TrailRenderer YinSpinTrail;
        public Transform Yang;
        public TrailRenderer YangMoveTrail;
        public TrailRenderer YangSpinTrail;
        public CinemachineVirtualCamera VirtualCamera;

        [Header("General Settings")] 
        public float pairDistance;
        public float mainMass = 1000f;
        public float subMass = 1f;

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
        
        private Transform _main;
        private Transform _sub;
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

        private void Start()
        {
            Vector3 halfDistance = new Vector3(pairDistance * 0.5f, 0f, 0f);
            Yin.position = halfDistance;
            Yang.position = -halfDistance;

            ResolvePairSettings();
        }

        private void Update()
        {
            _movementStep = MathEx.LerpSnap(_movementStep, _movementInput, movementLerp * Time.deltaTime, 0.99f);
            _mouseClickStep = MathEx.LerpSnap(_mouseClickStep, _mouseClickInput, spinLerp * Time.deltaTime, 0.99f);
            
            _mainRB.AddForce(_movementStep * movementPower, ForceMode2D.Force);
            _mainRB.velocity = MathEx.MagnitudeCap(_mainRB.velocity, maxMovementSpeed);
            _subRB.AddForce(_sub.up * _mouseClickStep * spinPower * _spinDir, ForceMode2D.Force);
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
                    _main = Yin;
                    _sub = Yang;
                    _spinDir = 1;
                    YinMoveTrail.emitting = true;
                    YinSpinTrail.emitting = false;
                    YangMoveTrail.emitting = false;
                    YangSpinTrail.emitting = true;
                    break;
                
                case PlayerMode.Yang:
                    _main = Yang;
                    _sub = Yin;
                    _spinDir = -1;
                    YinMoveTrail.emitting = false;
                    YinSpinTrail.emitting = true;
                    YangMoveTrail.emitting = true;
                    YangSpinTrail.emitting = false;
                    break;
            }

            _mainRB = _main.GetComponent<Rigidbody2D>();
            _subRB = _sub.GetComponent<Rigidbody2D>();

            _subRB.mass = subMass;
            _subRB.angularDrag = _isSpinning ? 0f : endAngularDrag;
            _subRB.drag = 0f;
            
            _mainRB.mass = mainMass;
            _mainRB.angularDrag = 0f;
            _mainRB.drag = _isMoving ? 0f : endLinearDrag;

            VirtualCamera.Follow = _main;
            VirtualCamera.LookAt = _main;
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
                _subRB.angularDrag = 0f;
            else if (callback.canceled)
                _subRB.angularDrag = endAngularDrag;

            _isSpinning = callback.phase == InputActionPhase.Performed;
        }

        public void OnSwap(InputAction.CallbackContext callback)
        {
            if (callback.performed) ToggleMode();
        }
        
        public PlayerMode CurrentMode => _mode;
    }
}
