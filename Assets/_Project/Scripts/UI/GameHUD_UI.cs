using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD_UI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image lifeBarImage;
    [SerializeField] private Image progressLevelBar1Image;
    [SerializeField] private Image progressLevelBar2Image;
    [SerializeField] private Image spell1RechargingImage;
    [SerializeField] private Image spell2RechargingImage;
    [SerializeField] private Image spell3RechargingImage;

    private float laserCount = 0;
    private float laserDelay = 0;
    private float breathCount = 0;
    private float breathDelay = 0;
    private float explosionCount = 0;
    private float explosionDelay = 0;

    private void Start() {
        Player.Instance.OnPlayerLifeChanged += Player_OnPlayerLifeChanged;
        Player.Instance.GetPlayerShot().OnPlayerCastLaser += PlayerShot_OnPlayerCastLaser;
        Player.Instance.GetPlayerShot().OnPlayerCastBreath += PlayerShot_OnPlayerCastBreath;
        Player.Instance.GetPlayerShot().OnPlayerCastExplosion += PlayerShot_OnPlayerCastExplosion;

        GameManager.Instance.OnLevelChanged += GameManager_OnLevelChanged;
        GameManager.Instance.OnTimeProgressChanged += GameManager_OnTimeProgressChanged;
    }

    private void FixedUpdate() {
        if (laserDelay == 0) {
            spell1RechargingImage.fillAmount = 0;
        } else {
            float amount = 1.0f - Mathf.Min(1, laserCount / laserDelay);
            spell1RechargingImage.fillAmount = amount;
            laserCount += Time.fixedDeltaTime;
        }

        if (breathDelay == 0) {
            spell2RechargingImage.fillAmount = 0;
        } else {
            float amount = 1.0f - Mathf.Min(1, breathCount / breathDelay);
            spell2RechargingImage.fillAmount = amount;
            breathCount += Time.fixedDeltaTime;
        }

        if (explosionDelay == 0) {
            spell3RechargingImage.fillAmount = 0;
        } else {
            float amount = 1.0f - Mathf.Min(1, explosionCount / explosionDelay);
            spell3RechargingImage.fillAmount = amount;
            explosionCount += Time.fixedDeltaTime;
        }
    }

    private void Player_OnPlayerLifeChanged(object sender, Player.OnPlayerLifeChangedArgs args) {
        lifeBarImage.fillAmount = args.currentLifeNormalized;
    }

    private void GameManager_OnLevelChanged(object sender, GameManager.OnLevelChangedArgs args) {
        levelText.text = "Level: " + args.level;
    }

    private void GameManager_OnTimeProgressChanged(object sender, GameManager.OnTimeProgressChangedArgs args) {
        progressLevelBar1Image.fillAmount = args.timeNormalized;
        progressLevelBar2Image.fillAmount = args.timeNormalized;
    }

    private void PlayerShot_OnPlayerCastLaser(object sender, PlayerShot.OnShotOrCastArgs args) {
        laserDelay = args.delay;
        laserCount = 0;
    }


    private void PlayerShot_OnPlayerCastBreath(object sender, PlayerShot.OnShotOrCastArgs args) {
        breathDelay = args.delay;
        breathCount = 0;
    }

    private void PlayerShot_OnPlayerCastExplosion(object sender, PlayerShot.OnShotOrCastArgs args) {
        explosionDelay = args.delay;
        explosionCount = 0;
    }
}
