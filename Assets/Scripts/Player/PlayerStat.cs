using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Player;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private GameplayService _service;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _killBonus;
    private float _remainingHealth;
    private PlayerID _lastDamageDealer;
    
    public PlayerID PlayerID;

    public event UnityAction OnDeath = delegate { }; 

    private void Awake()
    {
        _remainingHealth = _maxHealth;
    }

    private void Update()
    {
        CheckDeath();
    }

    public void DeductHealth(float damage, PlayerID lastDealer)
    {
        _remainingHealth -= damage;
        _lastDamageDealer = lastDealer;
    }

    private void CheckDeath()
    {
        if (_remainingHealth <= 0)
        {
            _remainingHealth = _maxHealth;
            _service.PlayerManager.IncreaseScore(_lastDamageDealer, _killBonus);
            _service.PlayerManager.ReduceRemainingLife(PlayerID);
            OnDeath.Invoke();
        }
    }
}
