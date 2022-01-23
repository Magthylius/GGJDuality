using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

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

        public float spinPower = 5f;
        public float initialAngularDrag = 0f;
        public float endAngularDrag = 10f;

        private Transform _main;
        private Transform _sub;
        private Rigidbody2D _mainRB;
        private Rigidbody2D _subRB;
        
        private PlayerMode _mode;

        private void Start()
        {
            Vector3 halfDistance = new Vector3(pairDistance * 0.5f, 0f, 0f);
            Yin.position = halfDistance;
            Yang.position = -halfDistance;

            ResolveMode();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ToggleMode();
            _mainRB.AddForce(MovementInput, ForceMode2D.Force);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                _subRB.angularDrag = initialAngularDrag;
            }
            
            if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
            {
                _subRB.angularDrag = endAngularDrag;
            }
            
            _subRB.AddForce(_sub.up * MouseInput * spinPower, ForceMode2D.Force);
        }

        public void ToggleMode()
        {
            _mode = _mode == PlayerMode.Yin ? PlayerMode.Yang : PlayerMode.Yin;
            ResolveMode();
        }

        private void ResolveMode()
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
            _subRB.angularDrag = endAngularDrag;
            VirtualCamera.Follow = _main;
        }

        private Vector2 MovementInput => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        private float MouseInput => Input.GetAxis("MouseRot");
        public PlayerMode CurrentMode => _mode;
    }
}
