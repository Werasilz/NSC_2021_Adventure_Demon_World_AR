using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    #region Component
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;                                                              // Component from Player
    private SpriteRenderer spriteRendererShadow;                                                        // Component from Shadow of Player
    private VariableJoystick variableJoystick;
    public Transform arCamera;
    #endregion

    #region Player Status
    private float speed = 0.1f;                                                                         // Player Walk Speed
    private bool isFlip;                                                                                // Check Sprite is Flip
    [HideInInspector] public bool isWalk;                                                               // Check Player Walk
    [HideInInspector] public bool isTriggerEnemy;                                                       // Check Enemy Trigger Player Area
    [HideInInspector] public bool isTriggerBoss;                                                        // Check Boss Trigger Player Area
    private bool isCollectShard;
    #endregion

    #region Value for Damage System
    [HideInInspector] public float stackHit;                                                            // Input Hit from Enemy
    [HideInInspector] public float stackHitBoss;                                                        // Input Hit from Boss
    private float damaged;                                                                              // Damage to Decrease HpPlayer
    private bool isPopup;                                                                               // Check Damage Popup is Show
    private bool isKnockback;
    #endregion

    #region Skill System
    public GameObject skill1Prefab;
    public GameObject skill2Prefab;
    public GameObject skill3Prefab;
    private Button skill1Button;
    private Button skill2Button;
    private Button skill3Button;
    #endregion

    #region GameObject
    private GameObject checkEnemy;                                                                      // GameObject for Checking Enemy Direction, Left and Right
    private GameObject spawnSkillPoint;                                                                 // Spawn Point for Skill
    private GameObject levelPlayerText;                                                                 // Level Player Text
    public GameObject hitEffect;                                                                        // Hit Effect when Attack Enemy
    public GameObject damagePopupPrefab;                                                                // Damage Popup when beat by enemy
    public GameObject shardPopup;
    private string shardText;
    public GameObject smokeEffect;
    public GameObject hpPotion;
    #endregion

    #region Check Direction Player
    [HideInInspector] public bool isLeft;                                                               // Checking Player Direction
    [HideInInspector] public bool isRight;                                                              // Checking Player Direction
    #endregion

    #region Hit Point For Effect Spawn
    private GameObject hitPoint;
    private GameObject leftHitPoint;
    private GameObject rightHitPoint;
    #endregion

    #region Audio
    public AudioClip attackSound;
    public AudioClip beatSound;
    public AudioClip boxSound;
    public AudioClip shardSound;
    public AudioClip skill1;
    public AudioClip skill2;
    public AudioClip skill3;
    #endregion

    void Awake()
    {
        instance = this;
    }

    public void GetComponent()
    {
        #region GetComponent
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        variableJoystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();
        spriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRendererShadow = gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>();
        checkEnemy = GameObject.Find("CheckEnemy");
        spawnSkillPoint = GameObject.Find("SpawnSkillPoint");
        levelPlayerText = GameObject.Find("LevelPlayerText");
        hitPoint = GameObject.Find("HitPoint");
        leftHitPoint = GameObject.Find("LeftHitPoint");
        rightHitPoint = GameObject.Find("RightHitPoint");
        skill1Button = GameObject.Find("Skill1Button").GetComponent<Button>();
        skill2Button = GameObject.Find("Skill2Button").GetComponent<Button>();
        skill3Button = GameObject.Find("Skill3Button").GetComponent<Button>();
        #endregion
    }

    void Update()
    {
        if (PlaceMapScript.instance.isSetPosition)
        {
            AnimationPlayerWalkJoyStick();
            FlipSprite();
        }
    }

    void FixedUpdate()
    {
        if (PlaceMapScript.instance.isSetPosition)
        {
            MovementJoyStick();
        }

        Knockback();
        BeatByEnemy();
        BeatByBoss();
    }

    #region Play With JoyStick
    void MovementJoyStick()
    {
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.Translate(movement * speed * Time.deltaTime);

        // Player Move forward by Camera
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, arCamera.eulerAngles.y, transform.eulerAngles.z);
    }

    void AnimationPlayerWalkJoyStick()
    {
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(horizontal));
        anim.SetFloat("VerticalSpeed", Mathf.Abs(vertical));

        #region Walking
        if (horizontal != 0 || vertical != 0)
        {
            isWalk = true;

            if (horizontal > 0)                                                                         // Right Direction
            {
                isFlip = true;
            }
            else if (horizontal < 0)                                                                    // Left Direction
            {
                isFlip = false;
            }
        }
        else
        {
            isWalk = false;
        }
        #endregion

        #region Check Direction by bool
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("LeftWalk") || anim.GetCurrentAnimatorStateInfo(0).IsName("LeftIdle"))
        {
            // Walk Left
            if (!isFlip)                                                                                // Checking Player Direction
            {
                isLeft = true;
                isRight = false;
            }
            // Walk Right
            else if (isFlip)
            {
                isLeft = false;
                isRight = true;
            }
        }
        #endregion
    }

    public void PlayerAttackJoyStick()
    {
        if (!isWalk && !GameManager.instance.isPause)                                                   // Attack when not walk only
        {
            if (isLeft || isRight)
            {
                anim.SetTrigger("Attack");                                                              // Play Animation Attack
                HitEffect();                                                                            // Spawn Hit Effect
                SoundManager.instance.PlaySingle(attackSound);
            }
        }
    }
    #endregion

    #region KnockBack
    void Knockback()
    {
        if (isKnockback)
        {
            isKnockback = false;
            float knockbackForce = 0.5f;

            if (checkEnemy.transform.GetChild(0).GetComponent<CheckEnemyDirection>().enemyOnLeft)       // KnockBack on the right
            {
                rb.AddForce(Vector3.right * knockbackForce, ForceMode.Impulse);
            }

            if (checkEnemy.transform.GetChild(1).GetComponent<CheckEnemyDirection>().enemyOnRight)      // KnockBack on the left
            {
                rb.AddForce(Vector3.left * knockbackForce, ForceMode.Impulse);
            }

            Invoke("ResetKnockback", 2);                                                                // Wait 2 sec and Call Reset 
        }
    }

    void ResetKnockback()
    {
        rb.velocity = Vector3.zero;                                                                     // Reset AddForce
    }
    #endregion

    #region Damage Section
    void BeatByEnemy()
    {
        if (stackHit > 0 && damaged == 0)                                                               // Enemy Hit to Player
        {
            int minDamage = 3;                                                                          // Set min Damage
            int maxDamage = 11;                                                                         // Set max Damage
            damaged = Random.Range(minDamage, maxDamage);                                               // Random Damage from min and max value
            stackHit = 0;                                                                               // Reset Hit
        }

        DamageToPlayer();
    }

    void BeatByBoss()
    {
        if (stackHitBoss > 0 && damaged == 0)                                                           // Boss Hit to Player
        {
            int minDamage = 10 * GameManager.instance.damageDouble;                                     // Set min Damage
            int maxDamage = 16 * GameManager.instance.damageDouble;                                     // Set max Damage
            damaged = Random.Range(minDamage, maxDamage);                                               // Random Damage from min and max value
            stackHitBoss = 0;                                                                           // Reset Hit
        }

        DamageToPlayer();
    }

    void DamageToPlayer()
    {
        if (damaged > 0 && !isPopup)                                                                    // Have Damage and Not have Damage Popup
        {
            SoundManager.instance.PlaySingle(beatSound);
            isPopup = true;
            GameManager.instance.hpPlayer -= damaged;                                                   // Decrease HpEnemy by Damage
            SpawnPopupDamage();                                                                         // Spawn Popup Damage
            anim.SetTrigger("isBeat");                                                                  // Play Animation isBeat
            isKnockback = true;                                                                         // Set isKnockback = true for Active function KnockBack
            StartCoroutine("BlinkEffect");                                                              // Start BlinkEffect
            Invoke("ResetPlayer", 2f);                                                                  // Wait 2 sec and Reset Player Value
        }
    }

    void SpawnPopupDamage()
    {
        GameObject popupClone = Instantiate(damagePopupPrefab, new Vector3(transform.position.x + 0.01f, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
        popupClone.GetComponent<TextMeshPro>().SetText("-" + damaged.ToString("0"));
        spriteRenderer.color = new Color(100, 0, 0, 255);                                               // Set Player Color to Red
    }

    void ResetPlayer()                                                                                  // Reset Player Value
    {
        StopCoroutine("BlinkEffect");                                                                   // Stop BlinkEffect
        damaged = 0;                                                                                    // Reset Damage
        stackHit = 0;                                                                                   // Reset Hit from enemy
        stackHitBoss = 0;                                                                               // Reset Hit from boss
        isPopup = false;
        spriteRenderer.color = new Color(255, 255, 255, 255);
        spriteRendererShadow.color = new Color(255, 255, 255, 200);
        levelPlayerText.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 255);
    }
    #endregion

    #region Skill
    public void Skill1()
    {
        CallCooldown();
        SoundManager.instance.PlaySingle(skill1);

        if (isRight)
        {
            GameObject skillClone = Instantiate(skill1Prefab, spawnSkillPoint.transform.position, skill1Prefab.transform.rotation);
        }

        if (isLeft)
        {
            GameObject skillClone = Instantiate(skill1Prefab, spawnSkillPoint.transform.position, skill1Prefab.transform.rotation);
        }
    }

    public void Skill2()
    {
        CallCooldown();
        SoundManager.instance.PlaySingle(skill2);

        if (isRight)
        {
            GameObject skillClone = Instantiate(skill2Prefab, spawnSkillPoint.transform.position, skill2Prefab.transform.rotation);
        }

        if (isLeft)
        {
            GameObject skillClone = Instantiate(skill2Prefab, spawnSkillPoint.transform.position, skill2Prefab.transform.rotation);
        }
    }

    public void Skill3()
    {
        CallCooldown();
        SoundManager.instance.PlaySingle(skill3);

        if (isRight)
        {
            GameObject skillClone = Instantiate(skill3Prefab, spawnSkillPoint.transform.position, skill3Prefab.transform.rotation);
        }

        if (isLeft)
        {
            GameObject skillClone = Instantiate(skill3Prefab, spawnSkillPoint.transform.position, skill3Prefab.transform.rotation);
        }
    }

    void CallCooldown()                                                                                 // CoolDown all Skill
    {
        skill1Button.GetComponent<CoolDownController>().StartCoolDown();
        skill2Button.GetComponent<CoolDownController>().StartCoolDown();
        skill3Button.GetComponent<CoolDownController>().StartCoolDown();
    }
    #endregion

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255, 0);                                         // Set Color Alpha to 0 sprite will invisible
            spriteRendererShadow.color = new Color(255, 255, 255, 0);
            levelPlayerText.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 0);

            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255, 255);                                       // Set Color Alpha to 255 sprite will visible
            spriteRendererShadow.color = new Color(255, 255, 255, 200);
            levelPlayerText.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 255);
        }
    }

    void HitEffect()
    {
        if (isLeft)
        {
            GameObject hitClone = Instantiate(hitEffect, leftHitPoint.transform.position, transform.rotation);
            hitClone.transform.SetParent(hitPoint.transform);
            hitClone.transform.localScale = new Vector3(1, 1, 1);
            hitClone.gameObject.GetComponent<Animator>().SetTrigger("hitLeft");
            Destroy(hitClone, 0.3f);
        }
        else if (isRight)
        {
            GameObject hitClone = Instantiate(hitEffect, rightHitPoint.transform.position, transform.rotation);
            hitClone.transform.SetParent(hitPoint.transform);
            hitClone.transform.localScale = new Vector3(1, 1, 1);
            hitClone.gameObject.GetComponent<Animator>().SetTrigger("hitRight");
            Destroy(hitClone, 0.3f);
        }
    }

    void FlipSprite()
    {
        if (isFlip)
        {
            transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);                                   // Filp Scale Player
            levelPlayerText.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);                   // Set Scale LevelPlayerText to not Filp follow parent
            checkEnemy.transform.GetChild(0).transform.localPosition = new Vector3(1f, 0, 0);           // Set Position to not Filp follow parent
            checkEnemy.transform.GetChild(1).transform.localPosition = new Vector3(-1f, 0, 0);          // Set Position to not Filp follow parent
        }
        else if (!isFlip)
        {
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);                                    // Filp Scale Player
            levelPlayerText.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);                    // Set Scale LevelPlayerText to not Filp follow parent
            checkEnemy.transform.GetChild(0).transform.localPosition = new Vector3(-1f, 0, 0);          // Set Position to not Filp follow parent
            checkEnemy.transform.GetChild(1).transform.localPosition = new Vector3(1f, 0, 0);           // Set Position to not Filp follow parent
        }
    }

    void SpawnSmoke(Collider other)
    {
        GameObject cloneSmoke = Instantiate(smokeEffect, other.transform.position, transform.rotation);
        Destroy(cloneSmoke, 0.5f);
    }

    void PlayerTriggerBoxItem(Collider other)
    {
        if (other.CompareTag("BoxItem"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack"))
            {
                int i = Random.Range(1, GameManager.instance.dropRate + 1);

                if (i == GameManager.instance.dropRate)
                {
                    GameObject hpPotionClone = Instantiate(hpPotion, other.transform.position, hpPotion.transform.rotation);
                    hpPotionClone.transform.SetParent(GameObject.Find("GamePlay").transform);
                }

                SpawnSmoke(other);
                SoundManager.instance.PlaySingle(boxSound);
                Destroy(other.gameObject);
            }
        }
    }

    void PlayerTriggerEnemy(bool isEnter, bool isExit, Collider other)
    {
        if (isEnter)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                isTriggerEnemy = true;
                isTriggerBoss = true;

                if (checkEnemy.transform.GetChild(0).GetComponent<CheckEnemyDirection>().enemyOnLeft)   // Attack to Enemy on the left
                {
                    if (!isFlip && anim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack"))            // Check Animation Attack for correct Direction to Attack Enemy
                    {
                        other.gameObject.GetComponent<EnemyController>().stackHit = 1;                  // Send StackHit to Enemy
                    }
                }

                if (checkEnemy.transform.GetChild(1).GetComponent<CheckEnemyDirection>().enemyOnRight)  // Attack to Enemy on the right
                {
                    if (isFlip && anim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack"))             // Check Animation Attack for correct Direction to Attack Enemy
                    {
                        other.gameObject.GetComponent<EnemyController>().stackHit = 1;                  // Send StackHit to Enemy
                    }
                }
            }

            if (other.CompareTag("EnemyArea") || other.CompareTag("CheckPlayer"))
            {
                isTriggerEnemy = false;
                isTriggerBoss = false;
            }
        }

        if (isExit)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                isTriggerEnemy = false;
                isTriggerBoss = false;
            }
        }
    }

    void PlayerTriggerEnemyArea(bool isIntoAreaCheck, Collider other)
    {
        if (other.CompareTag("EnemyArea"))                                                              // Check Player Enter Enemy Area
        {
            other.GetComponentInParent<EnemyController>().isIntoArea = isIntoAreaCheck;                 // Set Value to Enemy
        }
    }

    void PlayerTriggerShard(Collider other)
    {
        if (other.CompareTag("Shard"))
        {
            if (!isCollectShard)                                                                        // Can Collect Shard
            {
                SoundManager.instance.PlaySingle(shardSound);
                isCollectShard = true;
                ShardPopupText();
                Destroy(other.transform.parent.gameObject);                                             // Destroy Shard
                Invoke("AddShard", 0.5f);
                Invoke("ResetCollect", 1f);
            }
        }
    }

    void AddShard()
    {
        GameManager.instance.shardCollect = GameManager.instance.shardCollect + 1;                      // Add 1 to shardCollect
    }

    void ResetCollect()
    {
        isCollectShard = false;
    }

    void ShardPopupText()
    {
        if (GameManager.instance.level == 1)
        {
            if (GameManager.instance.shardCollect == 0)
            {
                shardText = "1. ทุกข์";
            }
            else if (GameManager.instance.shardCollect == 1)
            {
                shardText = "2. สมุทัย";
            }
            else if (GameManager.instance.shardCollect == 2)
            {
                shardText = "3. นิโรธ";
            }
            else if (GameManager.instance.shardCollect == 3)
            {
                shardText = "4. มรรค";
            }

            GameObject popupClone = Instantiate(shardPopup, new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
            popupClone.GetComponent<TextMeshPro>().text = shardText;
        }

        if (GameManager.instance.level == 2)
        {
            if (GameManager.instance.shardCollect == 0)
            {
                shardText = "1. ฉันทะ ความพอใจ";
            }
            else if (GameManager.instance.shardCollect == 1)
            {
                shardText = "2. วิริยะ ความเพียร";
            }
            else if (GameManager.instance.shardCollect == 2)
            {
                shardText = "3. จิตตะ ความคิด";
            }
            else if (GameManager.instance.shardCollect == 3)
            {
                shardText = "4. วิมังสา ความไตร่ตรอง";
            }

            GameObject popupClone = Instantiate(shardPopup, new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
            popupClone.GetComponent<TextMeshPro>().text = shardText;
        }

        if (GameManager.instance.level == 3)
        {
            if (GameManager.instance.shardCollect == 0)
            {
                shardText = "1. การดื่มน้ำเมา";
            }
            else if (GameManager.instance.shardCollect == 1)
            {
                shardText = "2. การเที่ยวกลางคืน";
            }
            else if (GameManager.instance.shardCollect == 2)
            {
                shardText = "3. เที่ยวดูการละเล่น";
            }
            else if (GameManager.instance.shardCollect == 3)
            {
                shardText = "4. เล่นการพนัน";
            }
            else if (GameManager.instance.shardCollect == 4)
            {
                shardText = "5. การคบคนชั่วเป็นมิตร";
            }
            else if (GameManager.instance.shardCollect == 5)
            {
                shardText = "6. เกียจคร้านการงาน ";
            }

            GameObject popupClone = Instantiate(shardPopup, new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
            popupClone.GetComponent<TextMeshPro>().text = shardText;
        }

        if (GameManager.instance.level == 4)
        {
            if (GameManager.instance.shardCollect == 0)
            {
                shardText = "1. เว้นจาการฆ่าสัตว์";
            }
            else if (GameManager.instance.shardCollect == 1)
            {
                shardText = "2. เว้นจากการลักทรัพย์";
            }
            else if (GameManager.instance.shardCollect == 2)
            {
                shardText = "3. เว้นจากการประพฤติผิดในกาม";
            }
            else if (GameManager.instance.shardCollect == 3)
            {
                shardText = "4. เว้นจากการพูดเท็จ";
            }
            else if (GameManager.instance.shardCollect == 4)
            {
                shardText = "5. เว้นจากการดื่มของมึนเมา";
            }

            GameObject popupClone = Instantiate(shardPopup, new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
            popupClone.GetComponent<TextMeshPro>().text = shardText;
        }

        if (GameManager.instance.level == 5)
        {
            if (GameManager.instance.shardCollect == 0)
            {
                shardText = "1. ความเมตตากรุณา";
            }
            else if (GameManager.instance.shardCollect == 1)
            {
                shardText = "2. การเลี้ยงชีพโดยชอบ";
            }
            else if (GameManager.instance.shardCollect == 2)
            {
                shardText = "3. ความสำรวมในกาม";
            }
            else if (GameManager.instance.shardCollect == 3)
            {
                shardText = "4. พูดความจริง";
            }
            else if (GameManager.instance.shardCollect == 4)
            {
                shardText = "5. ความระลึกรู้สึกตัวอยู่เสมอ";
            }

            GameObject popupClone = Instantiate(shardPopup, new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z + 0.01f), Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0));
            popupClone.GetComponent<TextMeshPro>().text = shardText;
        }

    }

    void PlayerTriggerCheckPlayer(bool isEnter, bool isExit, Collider other)
    {
        if (isEnter)
        {
            if (other.CompareTag("CheckPlayer"))
            {
                if (other.GetComponent<CheckPlayerDirection>().direction[0] && !other.GetComponent<CheckPlayerDirection>().playerOnRight)
                {
                    other.GetComponent<CheckPlayerDirection>().playerOnLeft = true;
                    other.GetComponent<CheckPlayerDirection>().playerOnRight = false;
                }

                if (other.GetComponent<CheckPlayerDirection>().direction[1] && !other.GetComponent<CheckPlayerDirection>().playerOnLeft)
                {
                    other.GetComponent<CheckPlayerDirection>().playerOnRight = true;
                    other.GetComponent<CheckPlayerDirection>().playerOnLeft = false;
                }
            }
        }

        if (isExit)
        {
            if (other.CompareTag("CheckPlayer"))
            {
                other.GetComponent<CheckPlayerDirection>().playerOnLeft = false;
                other.GetComponent<CheckPlayerDirection>().playerOnRight = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerTriggerEnemy(true, false, other);                                                         // Player Attack Enemy
        PlayerTriggerBoxItem(other);                                                                    // Check Box Item for spawn Potion
        PlayerTriggerEnemyArea(true, other);                                                            // Player Enter EnemyArea Enemy will Follow Player
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerTriggerShard(other);                                                                      // Collect Shard
        PlayerTriggerCheckPlayer(true, false, other);                                                   // Check Player for Flip Enemy
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTriggerEnemyArea(false, other);                                                           // Player Exit EnemyArea Enemy will not Follow Player
        PlayerTriggerCheckPlayer(false, true, other);                                                   // Check Player for Flip Enemy
        PlayerTriggerEnemy(false, true, other);                                                         // Check Player Exit Enemy
    }
}
