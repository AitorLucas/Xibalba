using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Aim : ISingleton<Aim> {

    [SerializeField] private ProjectileSO shotProjectileSO;
    [SerializeField] private ProjectileSO laserProjectileSO;
    [SerializeField] private ProjectileSO explosionProjectileSO;
    [SerializeField] private ProjectileSO breathProjectileSO;


    private SpriteRenderer spriteRenderer;

    private SpellType currentSpellType = SpellType.None;
    private bool isClipped = true;
    private Vector3 rotationForSprite = Vector3.one;
    private bool isLaserGlobalRange = false;
    private bool isExplosionPositionFree = false;
    private float explosionRangeMultiplier = 1f;

    protected override void Awake() {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        Player player = Player.Instance;
        player.GetPlayerController().OnSpellTypeChanged += PlayerController_OnSpellTypeChanged;
        player.OnExplosionPositionFreeChanged += Player_OnExplosionPositionFreeChanged;
        player.OnLaserGlobalRangeChanged += Player_OnLaserGlobalRangeChanged;
        player.OnExplosionRangeMultiplierChanged += Player_OnExplosionRangeMultiplierChanged;

        UpdateAim(SpellType.None);
    }

    private void Update() {
        if (GameManager.Instance.IsPaused()) {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        Vector2 playerPosition = Player.Instance.transform.position;
        Vector2 playerToMouse = mouseInWorldPosition - playerPosition;

        Vector2 spellPosition = Vector2.zero;
        if (isClipped) {
            if (currentSpellType == SpellType.Explosion) {
                spellPosition = playerPosition;
            } else {
                spellPosition = playerPosition + (playerToMouse.normalized * 0.7f);
            }
        } else {
            spellPosition = mouseInWorldPosition;
        }
        transform.position = spellPosition;

        Quaternion spellRotation = Quaternion.identity;
        if (currentSpellType != SpellType.Explosion) {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, playerToMouse);
            spellRotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360) * Quaternion.Euler(rotationForSprite);
        }
        transform.rotation = spellRotation;
    }

    private void PlayerController_OnSpellTypeChanged(object sender, PlayerController.OnSpellTypeChangedArgs e) {
        UpdateAim(spellType: e.spellType);
    }

    private void Player_OnExplosionPositionFreeChanged(object sender, Player.OnExplosionPositionFreeChangedArgs e) {
        isExplosionPositionFree = e.isActive;
    }

    private void Player_OnLaserGlobalRangeChanged(object sender, Player.OnLaserGlobalRangeChangedArgs e) {
        isLaserGlobalRange = e.isActive;
    }

    private void Player_OnExplosionRangeMultiplierChanged(object sender, Player.OnExplosionRangeMultiplierChangedArgs e) {
        explosionRangeMultiplier = e.multiplier;
    }

    private void UpdateAim(SpellType spellType) {
        currentSpellType = spellType;
        ChooseCliped(spellType);
        ChooseSprite(spellType);
        ScaleSprite(spellType);
        RotateSprite(spellType);
    }

    private void ChooseCliped(SpellType spellType) {
        switch (spellType) {
            case SpellType.None:
                isClipped = true;
                break;
            case SpellType.Laser:
                isClipped = true;
                break;
            case SpellType.Breath:
                isClipped = true;
                break;
            case SpellType.Explosion:
                if (isExplosionPositionFree) {
                    isClipped = false;
                } else {
                    isClipped = true;
                }
                break;
        }
    }

    private void ChooseSprite(SpellType spellType) {
        Sprite newSprite = null;
        switch (spellType) {
            case SpellType.Laser:
                newSprite = laserProjectileSO.sprite;
                break;
            case SpellType.Breath:
                newSprite = breathProjectileSO.sprite;
                break;
            case SpellType.Explosion:
                newSprite = explosionProjectileSO.sprite;
                break;
            case SpellType.None:
                newSprite = shotProjectileSO.sprite;
                break;
        }
        spriteRenderer.sprite = newSprite;
    }

    private void ScaleSprite(SpellType spellType) {
        Vector3 newScale = Vector3.one;
        switch (spellType) {
            case SpellType.Laser:
                if (isLaserGlobalRange) {
                    newScale = new Vector3(2f, 0.3f, 0.3f);
                } else {
                    newScale = laserProjectileSO.projectile.transform.localScale;
                }
                break;
            case SpellType.Breath:
                newScale = breathProjectileSO.projectile.transform.localScale;
                break;
            case SpellType.Explosion:
                newScale = explosionProjectileSO.projectile.transform.localScale * explosionRangeMultiplier;
                break;
            case SpellType.None:
                newScale = shotProjectileSO.projectile.transform.localScale;
                break;
        }
        transform.localScale = newScale;
    }

    private void RotateSprite(SpellType spellType) {
        Vector3 newRotation = Vector3.one;
        switch (spellType) {
            case SpellType.Laser:
                newRotation = laserProjectileSO.projectile.transform.rotation.eulerAngles;
                break;
            case SpellType.Breath:
                newRotation = breathProjectileSO.projectile.transform.rotation.eulerAngles;
                break;
            case SpellType.Explosion:
                newRotation = explosionProjectileSO.projectile.transform.rotation.eulerAngles;
                break;
            case SpellType.None:
                newRotation = shotProjectileSO.projectile.transform.rotation.eulerAngles;
                break;
        }
        rotationForSprite = newRotation;
    }
}
