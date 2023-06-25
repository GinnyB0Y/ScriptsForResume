using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] CharacterController controller;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] AudioSource damageOnPlayer;

    [SerializeField] int playerHealth;

    [SerializeField] static bool gameOver;

    //Death
    [SerializeField] GameObject deathScreen;
    [SerializeField] AudioSource deadSound;
    [SerializeField] GameObject axeObject;
    [SerializeField] GameObject fireAxeObject;
    [SerializeField] public int deathCount;
    [SerializeField] DeathScript deathScript;

    //Health
    [SerializeField] int damage;
    [SerializeField] Health health;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        deathScreen.SetActive(false);

        playerHealth = 100;
        gameOver = false;
    }

    //Damage for Enemies
    public void Damage(int damageCount)
    {
        playerHealth -= damageCount;
        if (playerHealth <= 0)
        {
            gameOver = true;
            deathCount = UnityEngine.Random.Range(0, 4);
        }
    }
    public void DamageEffect()
    {
        StartCoroutine(cameraShake.ShakeOnDamage());
        damageOnPlayer.Play();
    }

    //Collider for death, if player fall under map
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Height"))
        {
            deathCount = UnityEngine.Random.Range(0, 4);
            deathScript.DeathChanger();
        }
    }
}
