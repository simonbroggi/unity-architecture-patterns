﻿using UnityEngine;
using UnityEngine.Events;

namespace UnityArchitecture.ScriptableObjectPattern
{
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(CombatTarget))]
    public class RangedAttack : MonoBehaviour
    {
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private BaseWeapon weapon;
        private CombatTarget _target;
        private Stat _damage;
        private Stat _knockBack;
        private Stat _range;
        private Stat _fireRate;
        private Stat _pierce;
        private Stat _critChance;

        private float _timeSinceLastAttack = 0f;

        private void Start()
        {
            _target = GetComponent<CombatTarget>();
            var stats = GetComponent<Stats>();
            _damage = stats.GetStat(StatType.Damage);
            _knockBack = stats.GetStat(StatType.KnockBack);
            _range = stats.GetStat(StatType.Range);
            _fireRate = stats.GetStat(StatType.FireRate);
            _pierce = stats.GetStat(StatType.Pierce);
            _critChance = stats.GetStat(StatType.CritChance);
        }

        private void Update()
        {
            var inverseAttackSpeed = 10f / _fireRate.value;
            if (_timeSinceLastAttack < inverseAttackSpeed)
            {
                _timeSinceLastAttack += Time.deltaTime;
                return;
            }

            if (!_target.HasTarget)
            {
                return;
            }

            if (_target.TargetDistance > _range.value * 0.5f)
            {
                return;
            }
            
            var info = new WeaponStatsInfo
            {
                Damage = _damage.value,
                KnockBack = _knockBack.value,
                Range = _range.value * 0.5f,
                AttackSpeed = _fireRate.value,
                Pierce = _pierce.value,
                IsCrit = Random.Range(0f, 100f) < _critChance.value
            };

            weapon.Attack(info, _target, projectileSpawnPoint);

            _timeSinceLastAttack = 0f;
        }
    }
}