using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : ISingleton<Player> {

    [SerializeField] public float maxLifes { get; private set; } = 3;
    [SerializeField] public float lifes { get; private set; } = 3;
    [SerializeField] public float invecibilityDelay { get; private set; } = 1;

    public event EventHandler<OnPlayerLifeChangedArgs> OnPlayerLifeChanged;
    public class OnPlayerLifeChangedArgs : EventArgs {
        public float currentLife;
    }

    private bool isInvecible = false;

    protected override void Awake() {
        base.Awake();
        lifes = maxLifes;
    }

    public void Hurt(float amount) {
        if (!isInvecible) {
            lifes -= amount;
            lifes = Mathf.Max(0, lifes);

            StartCoroutine(InvecibilityDelay());
            isInvecible = true;
        }

        Debug.Log(lifes);

        OnPlayerLifeChanged?.Invoke(this, new OnPlayerLifeChangedArgs {
            currentLife = this.lifes
        });
    }

    public void Heal(float amount) {
        lifes += amount;
        lifes = Mathf.Min(maxLifes, lifes);

        OnPlayerLifeChanged?.Invoke(this, new OnPlayerLifeChangedArgs {
            currentLife = this.lifes
        });
    }

    private IEnumerator InvecibilityDelay() {
        yield return new WaitForSeconds(invecibilityDelay);
        isInvecible = false;
    }
}
