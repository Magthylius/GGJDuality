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
        public Transform Yang;
        public CinemachineVirtualCamera VirtualCamera;

        [Header("Settings")] 
        public float pairDistance;

        public float mainMass = 1000f;
        public float subMass = 1f;
        
        public float movementLerp = 5f;
        public float mouseClickLerp = 5f;

        public float movementPower = 5f;
        public float endLinearDrag = 10f;
            
        public float spinPower = 5f;
        public float endAngularDrag = 10f;
        
        private Transform _main;
        private Transform _sub;
        private Rigidbody2D _mainRB;
        private Rigidbody2D _subRB;
        
        private PlayerMode _mode;

        private bool _isMoving = false;
        private Vector2 _movementStep = Vector2.zero;
        private Vector2 _movementInput = Vector2.zero;

        private bool _isSpinning = false;
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
            _mouseClickStep = MathEx.LerpSnap(_mouseClickStep, _mouseClickInput, mouseClickLerp * Time.deltaTime, 0.99f);
            
            _mainRB.AddForce(_movementStep * movementPower, ForceMode2D.Force);
            _subRB.AddForce(_sub.up * _mouseClickStep * spinPower, ForceMode2D.Force);
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
                    break;
                
                case PlayerMode.Yang:
                    _main = Yang;
                    _sub = Yin;
                    break;
            }

            _mainRB = _main.GetComponent<Rigidbody2D>();
            _subRB = _sub.GetComponent<Rigidbody2D>();

            _mainRB.mass = mainMass;
            _mainRB.angularDrag = 0f;
            
            _subRB.mass = subMass;
            _subRB.angularDrag = endAngularDrag;
            
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
            
        }

        public void OnMouseClick(InputAction.CallbackContext callback)
        {
            _mouseClickInput = callback.ReadValue<float>();
            
            if (callback.performed)
                _subRB.angularDrag = 0f;
            else if (callback.canceled)
                _subRB.angularDrag = endAngularDrag;
        }

        public void OnSwap(InputAction.CallbackContext callback)
        {
            if (callback.performed) ToggleMode();
        }
        
        public PlayerMode CurrentMode => _mode;
    }
}
