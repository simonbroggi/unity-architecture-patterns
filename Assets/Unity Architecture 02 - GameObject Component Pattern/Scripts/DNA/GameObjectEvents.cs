﻿using System;
using UnityEngine;

namespace UnityArchitecture.GameObjectComponentPattern
{
    public class GameObjectEvents : MonoBehaviour
    {
        public event Action OnEnabled; 
        public event Action OnDisabled;
        public event Action OnDestroyed;

        private void OnEnable()
        {
            OnEnabled?.Invoke();
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            OnEnabled = null;
            OnDisabled = null;
            OnDestroyed = null;
        }
    }
}