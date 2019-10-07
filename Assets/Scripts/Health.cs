using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int totalHealth = 10;
    public int TotalHealth {get { return totalHealth;}}
    public int CurrentHealth {get; private set; }
    public Action<int> OnDamageTaken =  delegate {};
    public Action OnHealthDepleated = delegate {};
    public Action<float> onHealthChanged = null;

    public float invincibilityTime = 0;

    private float invincibilityTimer = 0;

    private void Start() {
        CurrentHealth = TotalHealth;
    }

    public void Damage(int amount) {

        int damageTaken = amount;
        if (damageTaken > CurrentHealth)
            damageTaken = CurrentHealth;

        CurrentHealth -= damageTaken;
        OnDamageTaken(damageTaken);
        onHealthChanged?.Invoke((float) CurrentHealth / TotalHealth);

        if (CurrentHealth <= 0)
        {
            OnHealthDepleated();
        }

        if (Mathf.Approximately(invincibilityTimer, 0))
            invincibilityTimer = invincibilityTime;
    }

    private void Update() {
        if (!Mathf.Approximately(invincibilityTime, 0))
            if (invincibilityTimer > 0) {
                invincibilityTimer -= Time.deltaTime;
                return;
            }
    }

    public void Damage(float amount) {
        Damage((int)Math.Ceiling(amount));
    }

    public void Kill() {
        CurrentHealth = 0;
        OnDamageTaken(CurrentHealth);
        OnHealthDepleated();
    }
}
