using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    #region Component
    private Transform player;
    private Transform playerShadow;
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private SphereCollider enemyArea;
    private SphereCollider enemyCollider;
    private BoxCollider checkLeft;
    private BoxCollider checkRight;
    private Color spriteColor;
    #endregion

    #region Movement Value
    private Vector3 movement;
    private float speed = 0.4f;
    #endregion

    #region GameObject
    private GameObject shadowPrefab;                                                                        // Enemy Shadow
    private GameObject enemyHpBar;                                                                          // Enemy HP Bar in child of Enemy
    private GameObject specialHpBar;                                                                        // Hp Bar for special enemy can drop Shard
    public GameObject damagePopupPrefab;                                                                    // Show Normal Damage
    public GameObject criticalDamagePopupPrefab;                                                            // Show Critical Damage
    public GameObject skillPopupPrefab;                                                                     // Show Status from skill
    public GameObject warningPrefab;                                                                        // Spawn When Enter Enemy Area
    public GameObject smokeEffect;                                                                          // Spawn When Enemy Dead
    public GameObject hpPotion;                                                                             // Random Spawn after Enemy Dead
    #endregion

    #region Shard
    public GameObject[] shard1;
    public GameObject[] shard2;
    public GameObject[] shard3;
    public GameObject[] shard4;
    public GameObject[] shard5;
    #endregion

    #region Value for Damage System
    [HideInInspector] public float stackHit;                                                                // Input Hit from Player to Normal Enemy
    [HideInInspector] public float stackHitBoss;                                                            // Input Hit from Player to Boss
    private float damaged;                                                                                  // Damage to Decrease HpEnemy
    private float damagedBoss;
    [SerializeField] private float hpEnemy;                                                                 // Enemy Health
    private float hpOrigin;                                                                                 // First HP when start
    private float changeToHpBar;                                                                            // Calculate Value for change HpEnemy to HpBar
    private bool isCritical;                                                                                // Check Damage is Critical
    #endregion

    #region Skill Value
    private bool isKnockback;
    private bool isStun;
    private bool isSlow;
    #endregion

    #region Popup Check
    [HideInInspector] public bool isWarning;                                                                // Check Waring Show
    private bool isPopup;                                                                                   // Check Damage Popup is Show
    #endregion

    #region Enemy Status
    [HideInInspector] public bool isIntoArea;                                                               // Check Player is enter Enemy Area
    [HideInInspector] public bool isDead;                                                                   // Check Enemy Dead
    [HideInInspector] public float expEnemy;                                                                // Exp Of Enemy for give to Player
    [SerializeField] private int enemyType;                                                                 // Type of Enemy Set when Spawn by SpawnEnemy Script
    [SerializeField] public int demonType;                                                                  // Type of Enemy Demon
    [SerializeField] private bool isKeyman;                                                                 // Is Enemy have Shard to Drop
    private bool isDown;                                                                                    // Check Enemy Down when Hp = 0
    private bool isShowDemon;                                                                               // Check Show Demon after attack by skill correct
    private bool isRecover;                                                                                 // Check Enemy Recovery Value after isDown to normal
    private bool coolDownAttack;                                                                            // Check Cooldown after Attack to Player
    private float countDown;                                                                                // Time to Cooldown
    [HideInInspector] public bool isFlip;                                                                   // Check Enemy Filp get from child EnemyArea
    private RigidbodyConstraints originConstraints;
    #endregion

    #region Boss Status
    public bool isBossType;                                                                                 // Check for Boss 
    [HideInInspector] public bool isBossNormalAttack;                                                       // Check Boss NormalAttack
    [HideInInspector] public bool isBossHardAttack;                                                         // Check Boss HardAttack
    [HideInInspector] public bool isBossDanger;                                                             // Check DangerZone Spawn
    [HideInInspector] public bool isBossCooldown;                                                           // Check Boss Attack Cooldown
    private bool isBeatBySkill;
    #endregion

    #region Audio
    public AudioClip deadSound;
    public AudioClip beatSound;
    public AudioClip beatBossSound;
    #endregion

    public void Awake()
    {
        #region GetComponent
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerShadow = GameObject.Find("PlayerShadow").GetComponent<Transform>();
        shadowPrefab = gameObject.transform.GetChild(0).gameObject;
        enemyHpBar = gameObject.transform.GetChild(1).gameObject;
        spriteColor = enemyHpBar.GetComponent<SpriteRenderer>().color;                                      // Get Color from Hp Bar

        enemyArea = gameObject.transform.GetChild(2).GetComponent<SphereCollider>();                        // Enemy Area Collider
        enemyCollider = gameObject.GetComponent<SphereCollider>();                                          // Enemy Collider
        checkLeft = enemyArea.transform.GetChild(0).GetComponent<BoxCollider>();                            // Check Player in Enemy Area
        checkRight = enemyArea.transform.GetChild(1).GetComponent<BoxCollider>();                           // Check Player in Enemy Area

        if (!isBossType)                                                                                    // For Normal Enemy only
        {
            gameObject.transform.GetChild(3).GetComponent<TextMeshPro>().text = "";                         // For Show Demon Type
            enemyCollider.enabled = false;                                                                  // Disable Collider
            checkLeft.enabled = false;
            checkRight.enabled = false;
        }

        originConstraints = rb.constraints;                                                                 // Set Rigidbody Freeze to original for call back to reset Freeze
        #endregion

        if (isKeyman)                                                                                       // if Enemy is a Keyman can drop Shard will have bigger hp bar than other enemy
        {
            specialHpBar = gameObject.transform.GetChild(5).gameObject;

        }
    }

    public void Start()
    {
        SetUpEnemy();                                                                                       // Setup Hp, Exp of Enemy By Enemy Type
        changeToHpBar = hpEnemy / 40;                                                                       // Calculate Value for change HpEnemy to HpBar
    }

    void Update()
    {
        CheckEnemyArea();
        CoolDownAttack();
        FlipSprite();
    }

    void FixedUpdate()
    {
        BeatByPlayer();
        EnemyDead();
        RecoveryEnemy();
        BossControl();
    }

    #region Boss Section
    void BossControl()
    {
        if (isBossType)
        {
            if (isBossNormalAttack || isBossHardAttack || !isIntoArea)
            {
                StopCoroutine("BossRandomAttack");                                                          // Stop Call Attack When Boss Attack normal or hard
            }

            if (!isBossNormalAttack && !isBossHardAttack && !isBossCooldown && isIntoArea)
            {
                StartCoroutine("BossRandomAttack");                                                         // Call Attack When Boss Not Attack normal and hard
            }
        }
    }

    IEnumerator BossRandomAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 6));                                            // Random time to call Attack
            int i = Random.Range(1, 4);                                                                     // Random type of Attack, 1 is Normal Attack, 2 is Hard Attack

            #region Boss Normal Attack
            if (i == 1 || i == 3)                                                                           // Random 1,2,3 Normal is 1,3 Hard is 2
            {
                if (!isBossDanger && !isBossNormalAttack && !isBossHardAttack)
                {
                    isBossDanger = true;
                    anim.SetBool("isLeftIdle", true);                                                       // Play Prepare animation
                    yield return new WaitForSeconds(2);                                                     // Wait 2 Second
                    anim.SetBool("isLeftIdle", false);                                                      // Stop Play Prepare animation
                    isBossNormalAttack = true;                                                              // Set Boss Normal Attack
                    GameManager.instance.damageDouble = 1;                                                  // Set Damage Double, 1 is Damage to Player * 1
                    anim.SetTrigger("isAttack");                                                            // Play Animation
                    Invoke("BossCooldown", 1);                                                              // Wait 1 Second and Call Cooldown function
                    Invoke("ResetBossCooldown", 5);                                                         // Wait 3 Second and Call ResetAttack function
                }
            }
            #endregion

            #region Boss Hard Attack
            if (i == 2)
            {
                if (!isBossDanger && !isBossNormalAttack && !isBossHardAttack)
                {
                    isBossDanger = true;
                    anim.SetBool("isPrepare", true);                                                        // Play Prepare animation
                    anim.SetBool("isWalk", false);                                                          // Enemy Stop Walking
                    yield return new WaitForSeconds(2);                                                     // Wait 2 Second
                    anim.SetBool("isPrepare", false);                                                       // Stop Play Prepare animation
                    isBossHardAttack = true;                                                                // Set Boss Hard Attack
                    GameManager.instance.damageDouble = 2;                                                  // Set Damage Double, 2 is Damage to Player * 2
                    anim.SetTrigger("isHardAttack");                                                        // Play Animation
                    Invoke("BossCooldown", 1);                                                              // Wait 1 Second and Call Cooldown function
                    Invoke("ResetBossCooldown", 5);                                                         // Wait 3 Second and Call ResetAttack function
                }
            }
            #endregion
        }
    }

    void ResetBossCooldown()
    {
        isBossCooldown = false;
    }

    void BossCooldown()
    {
        isBossCooldown = true;                                                                              // When isCooldown = true Boss can't send stackHitBoss to player
        isBossNormalAttack = false;                                                                         // Reset All Boss variable 
        isBossHardAttack = false;
        isBossDanger = false;
    }

    void BossStayPlayerCollider(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if (isBossType)
            {
                if (!isBossCooldown)
                {
                    if (isBossNormalAttack && other.GetComponentInParent<PlayerController>().isTriggerBoss) // Boss Normal Attack Player Can't Attack Player Behind Boss
                    {
                        other.gameObject.GetComponentInParent<PlayerController>().stackHitBoss = 1;
                    }

                    if (isBossHardAttack)                                                                   // Boss Hard Attack Player Can Attack Player Behind Boss
                    {
                        other.gameObject.GetComponentInParent<PlayerController>().stackHitBoss = 1;
                    }
                }
            }
        }
    }

    void EndLevelBoss()
    {
        if (isBossType)
        {
            GameManager.instance.ShowCompleteTab();
        }
    }
    #endregion

    #region Enemy Area Section
    void CheckEnemyArea()
    {
        #region Player Walk into Enemy Area
        if (isIntoArea)
        {
            if (!isDead && !isDown && !isKnockback && !isStun)                                              // Check not have any status
            {
                if (isWarning == false)                                                                     // Spawn Warning Prefab
                {
                    isWarning = true;
                    GameObject warningClone = Instantiate(warningPrefab, new Vector3(transform.position.x, transform.position.y + 0.018f, transform.position.z), transform.rotation);
                    warningClone.transform.SetParent(gameObject.transform);
                }

                if (isBossType)                                                                             // For Boss
                {
                    if (isBossDanger)
                    {
                        if (GameManager.instance.level == 6)
                        {
                            enemyArea.radius = 6;                                                           // Increase Area Collider
                        }

                        if (GameManager.instance.level == 7)
                        {
                            enemyArea.radius = 7;                                                           // Increase Area Collider
                        }

                        if (GameManager.instance.level == 8)
                        {
                            enemyArea.radius = 8;                                                           // Increase Area Collider
                        }

                        if (GameManager.instance.level == 9)
                        {
                            enemyArea.radius = 9;                                                           // Increase Area Collider
                        }

                        if (GameManager.instance.level == 10)
                        {
                            enemyArea.radius = 10;                                                          // Increase Area Collider
                        }
                    }
                    else if (!isBossDanger)
                    {
                        enemyArea.radius = 7;
                    }

                    if (!isBossNormalAttack || !isBossHardAttack || !isBossCooldown)                        // Check not have any status for call FollowPlayer
                    {
                        if (!isBossDanger)
                        {
                            EnemyFollowPlayer();                                                            // Enemy Walking to Player
                        }
                    }
                }

                if (!isBossType)                                                                            // For normal Enemy
                {
                    enemyArea.radius = 15;                                                                  // Increase Area Collider
                    enemyCollider.enabled = true;                                                           // Enable All Collider
                    checkLeft.enabled = true;
                    checkRight.enabled = true;
                    EnemyFollowPlayer();                                                                    // Enemy Walking to Player
                }
            }
        }
        #endregion

        #region Player Walk out of Enemy Area
        if (!isIntoArea)
        {
            isWarning = false;                                                                              // Reset Warning
            anim.SetBool("isWalk", false);                                                                  // Enemy Stop Walking

            if (isBossType)
            {
                isBossDanger = false;                                                                       // Cancel Danger Boss
                anim.SetBool("isPrepare", false);                                                           // Cancel Animation when player exit area
                enemyArea.radius = 3;                                                                       // Increase Area Collider
            }

            if (!isBossType)
            {
                enemyArea.radius = 7;                                                                       // Increase Area Collider
                enemyCollider.enabled = false;                                                              // Disable All Collider
                checkLeft.enabled = false;
                checkRight.enabled = false;
            }
        }
        #endregion

    }
    #endregion

    #region Damage and Hp Section
    void BeatByPlayer()
    {
        if (stackHit > 0 && !isDown)
        {
            if (!isBeatBySkill)                                                                             // Normal Enemy
            {
                isCritical = false;                                                                         // Reset isCritical
                float minDamage = 50 * ExperieneManager.instance.increaseDamagePercent;                     // Set min Damage and multiply with DamgePercent
                float maxDamage = 66 * ExperieneManager.instance.increaseDamagePercent;                     // Set max Damage and multiply with DamgePercent
                int Critical = Random.Range(1, 11);                                                         // Random Critical 1 - 10

                if (Critical == 10)                                                                         // Critical when Random Number 10
                {
                    isCritical = true;
                    minDamage = 70 * ExperieneManager.instance.increaseDamagePercent;                       // Set min Critical Damage and multiply with DamgePercent
                    maxDamage = 86 * ExperieneManager.instance.increaseDamagePercent;                       // Set max Critical Damage and multiply with DamgePercent
                }

                damaged = Random.Range(minDamage, maxDamage);                                               // Random Damage from min and max value
                stackHit = 0;                                                                               // Reset Hit
            }
        }

        if (stackHitBoss > 0 && !isDown)                                                                    // Boss
        {
            if (isBeatBySkill)
            {
                isBeatBySkill = false;
                isCritical = false;                                                                         // Reset isCritical
                float minDamage = 100 * ExperieneManager.instance.increaseDamagePercent;                    // Set min Damage and multiply with DamgePercent
                float maxDamage = 116 * ExperieneManager.instance.increaseDamagePercent;                    // Set max Damage and multiply with DamgePercent
                int Critical = Random.Range(1, GameManager.instance.critical + 1);                          // Random Critical 1 - 10 (normal rate = 10)

                if (Critical == GameManager.instance.critical)                                              // Critical when Random Number 10
                {
                    isCritical = true;
                    minDamage = 120 * ExperieneManager.instance.increaseDamagePercent;                      // Set min Critical Damage and multiply with DamgePercent
                    maxDamage = 136 * ExperieneManager.instance.increaseDamagePercent;                      // Set max Critical Damage and multiply with DamgePercent
                }

                damagedBoss = Random.Range(minDamage, maxDamage);                                           // Random Damage from min and max value
                stackHitBoss = 0;                                                                           // Reset Hit
            }
        }

        if (!isPopup && !isDead)                                                                            // Have Damage and Not have Damage Popup
        {
            if (damaged > 0 || damagedBoss > 0)
            {
                DamagetoEnemy();
                EnemyHpCalculate();
                Invoke("ResetEnemy", 0.5f);
            }
        }
    }

    void DamagetoEnemy()
    {
        isPopup = true;
        spriteRenderer.color = new Color(100, 0, 0, 255);                                                   // Set Enemy Sprite to Red

        if (!isBossType)
        {
            SoundManager.instance.PlaySingle(beatSound);
            anim.SetTrigger("isBeat");                                                                      // Set Animation 
        }

        if (isBossType)
        {
            SoundManager.instance.PlaySingle(beatBossSound);
        }

        if (isCritical)                                                                                     // Spawn Critical Damage Popup
        {
            GameObject popupClone = Instantiate(criticalDamagePopupPrefab, new Vector3(transform.position.x + 0.01f, transform.position.y + 0.018f, transform.position.z), transform.rotation);
            popupClone.transform.SetParent(gameObject.transform);
            isCritical = false;                                                                             // Reset Critical

            if (damagedBoss > 0)
            {
                popupClone.GetComponent<TextMeshPro>().SetText("-" + damagedBoss.ToString("0"));
            }

            if (damaged > 0)
            {
                popupClone.GetComponent<TextMeshPro>().SetText("-" + damaged.ToString("0"));
            }
        }
        else if (!isCritical)                                                                               // Spawn Normal Damage Popup
        {
            GameObject popupClone = Instantiate(damagePopupPrefab, new Vector3(transform.position.x + 0.01f, transform.position.y + 0.018f, transform.position.z), transform.rotation);
            popupClone.transform.SetParent(gameObject.transform);

            if (damagedBoss > 0)
            {
                popupClone.GetComponent<TextMeshPro>().SetText("-" + damagedBoss.ToString("0"));
            }

            if (damaged > 0)
            {
                popupClone.GetComponent<TextMeshPro>().SetText("-" + damaged.ToString("0"));
            }
        }
    }

    void EnemyHpCalculate()
    {
        Vector3 barScaleX = enemyHpBar.transform.localScale;                                                // Save HpBar Scale to barScaleX

        if (damagedBoss > 0)
        {
            hpEnemy -= damagedBoss;                                                                         // Decrease HpEnemy by Damage
            barScaleX.x -= damagedBoss / changeToHpBar;                                                     // Decrease EnemyHpBar Scale X
        }

        if (damaged > 0)
        {
            hpEnemy -= damaged;                                                                             // Decrease HpEnemy by Damage
            barScaleX.x -= damaged / changeToHpBar;                                                         // Decrease EnemyHpBar Scale X
        }

        enemyHpBar.transform.localScale = barScaleX;                                                        // Set EnemyHpBar to new Scale
    }

    void ResetEnemy()
    {
        damaged = 0;
        damagedBoss = 0;
        isPopup = false;
        spriteRenderer.color = new Color(255, 255, 255, 255);
    }

    void EnemyDead()
    {
        if (hpEnemy <= 0 && !isDead)
        {
            if (!isShowDemon && !isDown)                                                                    // Enemy is Not Show demon
            {
                if (!isBossType)
                {
                    StartCoroutine("EnemyDown");                                                            // Call EnemyDown when Hp = 0 (not Dead)
                }

                if (isBossType)
                {
                    isShowDemon = true;
                }
            }
            else if (isShowDemon && !isDown)                                                                // Enemy is Show Demon After Down 
            {
                SoundManager.instance.PlaySingle(deadSound);                                                // Enemy Dead
                isDead = true;                                                                              // Set Checking Value to True
                anim.SetBool("isDead", true);                                                               // Play Animation to Dead

                ExperieneManager.instance.isAddExp = true;                                                  // Set Check Value
                ExperieneManager.instance.expPlayer += expEnemy;                                            // Add Exp To Player
                GameManager.instance.score += 1;                                                            // Score + 1
                enemyHpBar.gameObject.SetActive(false);                                                     // Hide Enemy Hp Bar

                StartCoroutine("BlinkEffect");                                                              // Play Blink Effect
                Invoke("StopBlinkEffect", 1);                                                               // Stop Blink Effect
                Invoke("SpawnShard", 1);                                                                    // Spawn Shard for Keyman
                Invoke("EndLevelBoss", 1);                                                                  // Level Complete for Boss
                Invoke("DestroyEnemy", 1);
            }
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);                                                                                // Destroy Enemy
    }

    IEnumerator EnemyDown()
    {
        while (true)
        {
            rb.velocity = Vector3.zero;                                                                     // Reset Value for knockback bug
            rb.constraints = originConstraints;

            isDown = true;                                                                                  // Set isDown to True
            enemyHpBar.SetActive(false);                                                                    // Hide HpBar when Enemy Down
            anim.SetTrigger("isDown");                                                                      // Play Animation isDown
            yield return new WaitForSeconds(2);                                                             // Wait 2 sec
            anim.SetTrigger("isConfuse");                                                                   // Play Animation isConfuse
            yield return new WaitForSeconds(2);                                                             // Wait 2 sec

            if (!isShowDemon)                                                                               // Not have correct Skill Attack to Enemy
            {
                hpEnemy = hpOrigin;                                                                         // Set full Hp and HpBar
                enemyHpBar.transform.localScale = new Vector3(40, 5, 0);
                isRecover = true;
            }
        }
    }

    void RecoveryEnemy()                                                                                    // Call after Enemy Down
    {
        if (isRecover)                                                                                      // Reset Value after Enemy Down
        {
            isRecover = false;
            isDown = false;
            stackHitBoss = 0;                                                                               // Clear StackHit from Player
            stackHit = 0;                                                                                   // Clear StackHit from Player
            enemyHpBar.SetActive(true);                                                                     // Show HpBar
            StopCoroutine("EnemyDown");                                                                     // Stop function EnemyDown
        }
    }
    #endregion

    #region Effect and Spawn Object Section
    IEnumerator BlinkEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255, 0);
            shadowPrefab.SetActive(false);
            gameObject.transform.GetChild(3).gameObject.SetActive(false);

            if (isKeyman)
            {
                specialHpBar.SetActive(false);
            }

            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255, 255);
            shadowPrefab.SetActive(true);
            gameObject.transform.GetChild(3).gameObject.SetActive(true);

            if (isKeyman)
            {
                specialHpBar.SetActive(true);
            }
        }
    }

    void StopBlinkEffect()
    {
        StopCoroutine("EnemyDeadAnimation");
        SpawnSmoke();                                                                                       // Spawn Smoke Effect
        SpawnItem();                                                                                        // Random Spawn Item
    }

    void SpawnSmoke()
    {
        GameObject cloneSmoke = Instantiate(smokeEffect, transform.position, transform.rotation);

        if (isBossType)
        {
            cloneSmoke.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        Destroy(cloneSmoke, 0.5f);
    }

    void SpawnItem()
    {
        int i = Random.Range(1, GameManager.instance.dropRate + 1);

        if (i == GameManager.instance.dropRate && !isBossType && !isKeyman)                                     // Spawn Item for Enemy Only
        {
            GameObject hpPotionClone = Instantiate(hpPotion, transform.position, hpPotion.transform.rotation);
            hpPotionClone.transform.SetParent(GameObject.Find("GamePlay").transform);
        }
    }

    void SpawnShard()
    {
        if (isKeyman)
        {
            if (GameManager.instance.level == 1)
            {
                GameObject shardClone = Instantiate(shard1[GameManager.instance.shardCollect], transform.position, transform.rotation); // Spawn Shard Array by ShardCollect start at 0
                shardClone.transform.SetParent(GameObject.Find("GamePlay").transform);
            }

            if (GameManager.instance.level == 2)
            {
                GameObject shardClone = Instantiate(shard2[GameManager.instance.shardCollect], transform.position, transform.rotation); // Spawn Shard Array by ShardCollect start at 0
                shardClone.transform.SetParent(GameObject.Find("GamePlay").transform);
            }

            if (GameManager.instance.level == 3)
            {
                GameObject shardClone = Instantiate(shard3[GameManager.instance.shardCollect], transform.position, transform.rotation); // Spawn Shard Array by ShardCollect start at 0
                shardClone.transform.SetParent(GameObject.Find("GamePlay").transform);
            }

            if (GameManager.instance.level == 4)
            {
                GameObject shardClone = Instantiate(shard4[GameManager.instance.shardCollect], transform.position, transform.rotation); // Spawn Shard Array by ShardCollect start at 0
                shardClone.transform.SetParent(GameObject.Find("GamePlay").transform);
            }

            if (GameManager.instance.level == 5)
            {
                GameObject shardClone = Instantiate(shard5[GameManager.instance.shardCollect], transform.position, transform.rotation); // Spawn Shard Array by ShardCollect start at 0
                shardClone.transform.SetParent(GameObject.Find("GamePlay").transform);
            }
        }
    }
    #endregion

    #region EnemyEnterSkill
    void EnemyEnterSkill1(Collider other)
    {
        if (other.CompareTag("Skill1"))
        {
            if (demonType == 1)                                                                             // Skill is same to the DemonType
            {
                if (!isShowDemon && isDown)                                                                 // When enemy isDown and StackDemon > 0
                {
                    isShowDemon = true;                                                                     // Set isShowDemon to true for cancel refill Hp function in EnemyDown
                    ShowDemon();                                                                            // Call ShowDemon Function for change hp color and show text demon 
                    RestoreHealthAfterSkill();                                                              // Restore Hp to half of full hp
                    isKnockback = true;
                    EnemyKnockback();                                                                       // Call KnockBack function
                }
            }

            if (isBossType)
            {
                isBeatBySkill = true;
                stackHitBoss = 1;
            }
        }
    }

    void EnemyEnterSkill2(Collider other)
    {
        if (other.CompareTag("Skill2"))
        {
            if (demonType == 2)                                                                             // Skill is same to the DemonType
            {
                if (!isShowDemon && isDown)                                                                 // When enemy isDown and StackDemon > 0
                {
                    isShowDemon = true;                                                                     // Set isShowDemon to true for cancel refill Hp function in EnemyDown
                    ShowDemon();                                                                            // Call ShowDemon Function for change hp color and show text demon 
                    RestoreHealthAfterSkill();                                                              // Restore Hp to half of full hp
                    isStun = true;
                    EnemyStun();                                                                            // Call Stun function
                }
            }

            if (isBossType)
            {
                isBeatBySkill = true;
                stackHitBoss = 1;
            }
        }
    }

    void EnemyEnterSkill3(Collider other)
    {
        if (other.CompareTag("Skill3"))
        {
            if (demonType == 3)                                                                             // Skill is same to the DemonType
            {
                if (!isShowDemon && isDown)                                                                 // When enemy isDown and StackDemon > 0
                {
                    isShowDemon = true;                                                                     // Set isShowDemon to true for cancel refill Hp function in EnemyDown
                    ShowDemon();                                                                            // Call ShowDemon Function for change hp color and show text demon 
                    RestoreHealthAfterSkill();                                                              // Restore Hp to half of full hp
                    isSlow = true;
                    EnemySlow();                                                                            // Call Slow function
                }
            }

            if (isBossType)
            {
                isBeatBySkill = true;
                stackHitBoss = 1;
            }
        }
    }
    #endregion

    #region Skill Effect to Enemy
    void EnemyKnockback()
    {
        if (isKnockback)
        {
            isKnockback = false;
            float knockbackForce = 0.1f;
            int i = Random.Range(0, 4);
            anim.SetTrigger("isSkill1");
            SpawnSkillPopup("Knockback");
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

            if (i == 0)
            {
                rb.AddForce(Vector3.forward * knockbackForce, ForceMode.Impulse);
            }
            else if (i == 1)
            {
                rb.AddForce(Vector3.back * knockbackForce, ForceMode.Impulse);
            }
            else if (i == 2)
            {
                rb.AddForce(Vector3.left * knockbackForce, ForceMode.Impulse);
            }
            else if (i == 3)
            {
                rb.AddForce(Vector3.right * knockbackForce, ForceMode.Impulse);
            }

            Invoke("ResetKnockback", 2);
        }
    }

    void ResetKnockback()
    {
        rb.velocity = Vector3.zero;
        rb.constraints = originConstraints;
    }

    void EnemyStun()
    {
        if (isStun)
        {
            anim.SetTrigger("isSkill2");
            SpawnSkillPopup("Stun");
            Invoke("ResetStun", 3);
        }
    }

    void ResetStun()
    {
        isStun = false;
    }

    void EnemySlow()
    {
        if (isSlow)
        {
            speed = 0.1f;
            SpawnSkillPopup("SlowDown");
            Invoke("ResetSpeed", 4f);
        }
    }

    void ResetSpeed()
    {
        isSlow = false;
        speed = 0.4f;
    }
    #endregion

    #region Skill System
    void SpawnSkillPopup(string skillText)
    {
        GameObject popupClone = Instantiate(skillPopupPrefab, new Vector3(transform.position.x + 0.01f, transform.position.y + 0.018f, transform.position.z), transform.rotation);
        popupClone.transform.SetParent(gameObject.transform);
        popupClone.GetComponent<TextMeshPro>().SetText(skillText);
    }

    void ShowDemon()
    {
        if (isShowDemon)
        {
            spriteColor = new Color(100, 0, 255, 255);
            enemyHpBar.GetComponent<SpriteRenderer>().color = spriteColor;
            enemyHpBar.transform.localScale = new Vector3(20, 5, 0);

            if (demonType == 1)
            {
                gameObject.transform.GetChild(3).GetComponent<TextMeshPro>().text = "โทสะ";
            }

            if (demonType == 2)
            {
                gameObject.transform.GetChild(3).GetComponent<TextMeshPro>().text = "โมหะ";
            }

            if (demonType == 3)
            {
                gameObject.transform.GetChild(3).GetComponent<TextMeshPro>().text = "โลภะ";
            }
        }
    }

    void RestoreHealthAfterSkill()
    {
        isRecover = true;
        hpEnemy = hpOrigin;                                                                                 // Set full hp
        hpEnemy -= hpOrigin / 2;                                                                            // Set Hp to half of Original HP
    }
    #endregion

    #region Movement
    void EnemyFollowPlayer()
    {
        Vector3 direction = player.position - transform.position;                                           // Find Player Direction
        movement = direction;
        rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));                          // Follow Player by Direction

        if (!isSlow)                                                                                        // Not have Effect Slow from Player
        {
            anim.SetBool("isWalk", true);                                                                   // Play Animation Walk Normal
        }
        else if (isSlow)                                                                                    // Have Effect Slow from Player
        {
            anim.SetTrigger("isSkill3");                                                                    // Play Animation Walk Slow
        }
    }

    void FlipSprite()
    {
        if (isFlip)
        {
            if (!isBossType)
            {
                spriteRenderer.flipX = true;
            }
            else if (isBossType && !isBossNormalAttack && !isBossDanger)
            {
                spriteRenderer.flipX = true;
                GetComponent<SphereCollider>().center = new Vector3(1.40f, 0, 0);
            }
        }

        if (!isFlip)
        {
            if (!isBossType)
            {
                spriteRenderer.flipX = false;
            }
            else if (isBossType)
            {
                spriteRenderer.flipX = false;
                GetComponent<SphereCollider>().center = new Vector3(-1.40f, 0, 0);
            }
        }
    }
    #endregion

    void SetUpEnemy()
    {
        // Type Of Enemy
        // Type 1 = Jelly
        // Type 2 = Snail
        // Type 3 = Skeleton
        // Type 4 = Boss

        // HP Enemy Table
        // Level    [1]         [2]         [3]         [4]         [5]
        // Type 1   200         300         ---         500         600
        // Type 2   400         500         400         700         800
        // Type 3   ---         ---         600         900         1000 
        // Type 4   4000        4500        5000        5500        6000

        // Exp Enemy Table
        // Level    [1]         [2]         [3]         [4]         [5]
        // Type 1   15,26       35,46       -----       75,86       95,106
        // Type 2   35,46       55,66       55,66       95,106      115,126
        // Type 3   -----       -----       75,86       115,126     135,146
        // Type 4   95,106      115,126     135,146     155,166     175,186

        demonType = Random.Range(1, 4);

        if (GameManager.instance.level == 1 || GameManager.instance.level == 6)
        {
            if (enemyType == 1)
            {
                hpEnemy = 200;
                expEnemy = Random.Range(15, 26);
            }
            else if (enemyType == 2)
            {
                hpEnemy = 400;
                expEnemy = Random.Range(35, 46);
            }
            else if (enemyType == 4 && isBossType)
            {
                hpEnemy = 4000;
                expEnemy = Random.Range(95, 106);
                speed = 0.1f;
            }
        }

        if (GameManager.instance.level == 2 || GameManager.instance.level == 7)
        {
            if (enemyType == 1)
            {
                hpEnemy = 300;
                expEnemy = Random.Range(35, 46);
            }
            else if (enemyType == 2)
            {
                hpEnemy = 500;
                expEnemy = Random.Range(55, 66);
            }
            else if (enemyType == 4 && isBossType)
            {
                hpEnemy = 4500;
                expEnemy = Random.Range(115, 126);
                speed = 0.1f;
            }
        }

        if (GameManager.instance.level == 3 || GameManager.instance.level == 8)
        {
            if (enemyType == 2)
            {
                hpEnemy = 400;
                expEnemy = Random.Range(55, 66);
            }
            else if (enemyType == 3)
            {
                hpEnemy = 600;
                expEnemy = Random.Range(75, 86);
            }
            else if (enemyType == 4 && isBossType)
            {
                hpEnemy = 5000;
                expEnemy = Random.Range(135, 146);
                speed = 0.1f;
            }
        }

        if (GameManager.instance.level == 4 || GameManager.instance.level == 9)
        {
            if (enemyType == 1)
            {
                hpEnemy = 500;
                expEnemy = Random.Range(75, 86);
            }
            else if (enemyType == 2)
            {
                hpEnemy = 700;
                expEnemy = Random.Range(95, 106);
            }
            else if (enemyType == 3)
            {
                hpEnemy = 900;
                expEnemy = Random.Range(115, 126);
            }
            else if (enemyType == 4 && isBossType)
            {
                hpEnemy = 5500;
                expEnemy = Random.Range(155, 166);
                speed = 0.1f;
            }
        }

        if (GameManager.instance.level == 5 || GameManager.instance.level == 10)
        {
            if (enemyType == 1)
            {
                hpEnemy = 600;
                expEnemy = Random.Range(95, 106);
            }
            else if (enemyType == 2)
            {
                hpEnemy = 800;
                expEnemy = Random.Range(115, 126);
            }
            else if (enemyType == 3)
            {
                hpEnemy = 1000;
                expEnemy = Random.Range(135, 146);
            }
            else if (enemyType == 4 && isBossType)
            {
                hpEnemy = 6000;
                expEnemy = Random.Range(175, 186);
                speed = 0.1f;
            }
        }

        hpOrigin = hpEnemy;                                                                                 // Save Original Hp for Recovery HP
    }

    void CoolDownAttack()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0)
        {
            coolDownAttack = false;
        }
    }

    void EnemyStayPlayerCollider(Collider other)
    {
        if (other.CompareTag("PlayerCollider") && other.GetComponentInParent<PlayerController>().isTriggerEnemy)
        {
            if (other.GetComponent<CheckEnemyDirection>().enemyOnLeft && player.GetComponent<PlayerController>().isRight || other.GetComponent<CheckEnemyDirection>().enemyOnRight && player.GetComponent<PlayerController>().isLeft)
            {
                if (!isBossType)
                {
                    if (!isDead && !isStun && !isKnockback && !isSlow && !isDown && !coolDownAttack)
                    {
                        if (other.gameObject.GetComponentInParent<PlayerController>().isWalk == false)
                        {
                            coolDownAttack = true;
                            countDown = 3f;
                            anim.SetTrigger("isAttack");
                            other.gameObject.GetComponentInParent<PlayerController>().stackHit = 1;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyEnterSkill1(other);                                                                            // Enter Skill that same the Demontype will Decrease hp and Show Demon Text
        EnemyEnterSkill2(other);                                                                            // if skill not same the Demontype Enemy will Recovery to full hp
        EnemyEnterSkill3(other);                                                                            // Enemy have to Enter Skill Object 2 time for Enemy dead
    }

    private void OnTriggerStay(Collider other)
    {
        EnemyStayPlayerCollider(other);                                                                     // Enter Player Collider and Attack Player
        BossStayPlayerCollider(other);                                                                      // Enter Player Collider and Attack Player
    }
}
