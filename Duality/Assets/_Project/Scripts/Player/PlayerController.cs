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
        
        private PlayerMode _mode;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ToggleMode();
        }

        public void ToggleMode()
        {
            _mode = _mode == PlayerMode.Yin ? PlayerMode.Yang : PlayerMode.Yin;
            VirtualCamera.Follow = _mode == PlayerMode.Yin ? Yin : Yang;
        }
        
        public PlayerMode CurrentMode => _mode;
    }
}
