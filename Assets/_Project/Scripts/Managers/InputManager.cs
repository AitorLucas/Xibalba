using System;
using UnityEngine;

public class InputManager : ISingleton<InputManager> {

    private GameInputActions gameInputActions;

    public event EventHandler OnPlayerSpellCastAction;
    public event EventHandler OnPlayerBreathSpellSelectedAction;
    public event EventHandler OnPlayerLaserSpellSelectedAction;
    public event EventHandler OnPlayerExplosionSpellSelectedAction;
    public event EventHandler OnPlayerDashAction;
    public event EventHandler OnDebugTogglePaneAction;
    public event EventHandler OnGeneralPauseGameAction;

    protected override void Awake() {
        base.Awake();
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();
        gameInputActions.Debug.Enable();
        gameInputActions.General.Enable();
    }

    private void Start() {
        // gameInputActions.Player.CastSpell.performed += PlayerCastSpell_Performed;
        gameInputActions.Player.BreathSpell.performed += PlayerBreathSpell_Performed;
        gameInputActions.Player.LaserSpell.performed += PlayerLaserSpell_Performed;
        gameInputActions.Player.ExplosionSpell.performed += PlayerExplosionSpell_Performed;
        gameInputActions.Player.Dash.performed += PlayerDash_Performed;
        gameInputActions.Debug.TogglePane.performed += DebugTogglePane_Performed;
        gameInputActions.General.PauseGame.performed += GeneralPauseGame_Performed;
    }

    private void OnDestroy() {
        // gameInputActions.Player.CastSpell.performed -= PlayerCastSpell_Performed;
        gameInputActions.Player.BreathSpell.performed -= PlayerBreathSpell_Performed;
        gameInputActions.Player.LaserSpell.performed -= PlayerLaserSpell_Performed;
        gameInputActions.Player.ExplosionSpell.performed -= PlayerExplosionSpell_Performed;
        gameInputActions.Player.Dash.performed -= PlayerDash_Performed;
        gameInputActions.Debug.TogglePane.performed -= DebugTogglePane_Performed;
        gameInputActions.General.PauseGame.performed -= GeneralPauseGame_Performed;

        gameInputActions.Dispose();
    }

    // private void PlayerCastSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //     OnPlayerSpellCastAction?.Invoke(this, EventArgs.Empty);
    // }

    private void PlayerBreathSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerBreathSpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerLaserSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerLaserSpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerExplosionSpell_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerExplosionSpellSelectedAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerDash_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlayerDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void DebugTogglePane_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDebugTogglePaneAction?.Invoke(this, EventArgs.Empty);
    }

    private void GeneralPauseGame_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnGeneralPauseGameAction?.Invoke(this, EventArgs.Empty);
    }

    public bool GetMousePressed() {
        return gameInputActions.Player.CastSpell.IsPressed();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = gameInputActions.Player.Movement.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetShotDirectionVector() {
        Vector2 shotVector = gameInputActions.Player.ShotDirection.ReadValue<Vector2>();
        return shotVector;
    }
}