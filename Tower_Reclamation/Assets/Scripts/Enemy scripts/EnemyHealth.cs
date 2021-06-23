using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyHealth : MonoBehaviour {

    [SerializeField] protected Collider collisionMesh;

    [SerializeField] protected ParticleSystem hitparticleprefab;
    [SerializeField] protected ParticleSystem deathPrefab;
    [SerializeField] protected ParticleSystem endPrefab;
    [SerializeField] protected AudioClip enemyHitAudio;
    [SerializeField] protected AudioClip enemyDiedAudio;

    [SerializeField] public float hitPoints = 35;
    [SerializeField] public float hitPointsMax;
    [SerializeField] public EnemyHealthBars healthBars;
    [SerializeField] public Canvas enemyHealthBar;
    //protected Image healthImage;
    protected PostLevelSummaryScreen damageLog;

    protected float burnTime = 3f;
    protected float _healReduction = 0f;
    [SerializeField] protected bool onFire = false;
    [SerializeField] protected float time = 0;
    protected float burnDmg;

    public bool burrowed = false;
    protected bool healing = false;
    protected float healTimer = 1f;
    protected float healTime = 0f;
    protected float healPercent;
    protected float goldForMyHead = 3.5f;
    float healPerTick = 0f;
    protected string enemyName = "";
    protected bool hasGold = true;

    public bool isTargetable = true;
    public bool isBoss = false;

    public bool volcanicEnhanced = false;
    public bool forestEnhanced = false;

    protected bool noSpecialHealthThings = true;

    // Use this for initialization
    protected virtual void Start()
    {
        burrowed = false;
    //protected PostLevelSummaryScreen damageLog;
    damageLog = FindObjectOfType<PostLevelSummaryScreen>();
        if (noSpecialHealthThings)
        {
            // for each WAVE hit points go up a set amount.  In addition, for each level you are on, health ramps up.  Just base HP for now.
            //was 34, upping to 100 for easier adjustments and reading.  times all dmg / life by 3x
            hitPoints = 120;
            hitPoints += (5 * Singleton.Instance.level);
            // flat scaling (majority)
            int wavecount = FindObjectOfType<CurrentWave>().waveCount;
            float healthModifier = wavecount * 35;
            //plus percent scaling to make lateround harder.
            hitPoints += healthModifier;
            healthModifier = (hitPoints * (1 + ( .05f * (float)wavecount)));
            hitPointsMax = hitPoints;
            healthBars.SetHealthBarPercent(1.0f);
        }

        //only need to calculate once.  And if enemy is boss, reduce healing so its fair.
        healPerTick = (healPercent * hitPointsMax);
        if (isBoss)
        {
            healPerTick = healPerTick / 10f;
        }
        hasGold = true;
        StartCoroutine(CheckBiomeModifier());
        //CheckBiomeModifier();
        RegisterToEnemyList();
    }

    // make this a coroutine delayed start?
    private IEnumerator CheckBiomeModifier()
    {
        yield return null;
        switch (Singleton.biome)
        {
            case (int)Biomes.Ice:
                //ice, currently start and no modifier.
                break;
                  
            case (int)Biomes.Volcanic:
                // increase speed by 10% minus HP by 5%
                EnemyMovement movement = this.gameObject.GetComponentInChildren< EnemyMovement > ();
                movement.enemySpeed += movement.enemySpeed * .1f;
                movement.enemyBaseSpeed += movement.enemyBaseSpeed * .1f;

                hitPoints -= (hitPoints * .05f);
                hitPointsMax -= (hitPointsMax * .05f);
                break;

            case (int)Biomes.Forest:
                hitPoints += (hitPoints * .1f);
                hitPointsMax += (hitPointsMax * .1f);
                break;
            default:
                print("Biome hitting default? Value : " + Singleton.biome);
                break;
        }
        yield return null;
    }

    public void RegisterToEnemyList()
    {
        int amountPreAdd = EnemySpawner.EnemyAliveList.Count;
        EnemySpawner.EnemyAliveList.Add(this);
        //print("Amount of enemies before me = " + amountPreAdd + "  |||  Enemies after = " + EnemySpawner.EnemyAliveList.Count.ToString());
    }

    public float getBurnPercent()
    {
        return healthBars.GetBurnTimePercent();
    }

    public void IsBoss()
    {
        isBoss = true;
    }
     
    public float GetHPPercent()
    {
        return healthBars.GetHPPercent();
    }
    public float GetArmorPercent()
    {
        return healthBars.GetArmorPercent();
    }

    public void DontResethealthPlease()
    {
        noSpecialHealthThings = false;
    }

    protected virtual void Update()
    {
        if (onFire)
        {
            StartCoroutine(Burning(burnDmg));
        }
        if (healing)
        {
            StartCoroutine(Healing(healPercent));
        }
    }

    /// <summary>
    /// Refreshes the health bar percents, showing armor and life in its current amount.
    /// </summary>
    public void RefreshHealthBar()
    {
        // this means im in 'normal' health
        if ((hitPointsMax - hitPoints) > 0)
        {
            healthBars.SetArmorBarPercent(0);
            healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
        } else // this means im in armor state
        {
            healthBars.SetHealthBarPercent(1);
            healthBars.SetArmorBarPercent((hitPoints - hitPointsMax) / hitPointsMax);
        }
    }

    public virtual IEnumerator Burning(float fireDmg)
    {
        float burn = 0;
        if (onFire && time > 0)
        {
            burn = burnDmg * Time.deltaTime;
            time -= 1 * Time.deltaTime;
            hitPoints -= burn;
            //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
            RefreshHealthBar();
            healthBars.SetBurnBarPercent(time / burnTime);

            damageLog.UpdateDamage("Flame Tower", burn);
            Singleton.AddTowerDamage("Flame Tower", burn);
        }
        else
        {
            healthBars.SetBurnBarPercent(0f);
            onFire = false;
        }

        if (hitPoints < 1)
        {
            KillsEnemyandAddsGold();
            damageLog.UpdateKills("Flame Tower", enemyName);
        }
        yield return new WaitForSeconds(1f);
    }

    public void CaughtFire(float fireDmg, float healReduction)
    {
        _healReduction = healReduction;
        onFire = true;
        time += 2 * Time.deltaTime;
        if(time > burnTime)
        {
            time = burnTime;
        }

        burnDmg = fireDmg;
    }

    public virtual void GiveEnhancement(int biome)
    {
        print("Hitting default enhancement!! fill me in!");

        switch (biome)
        {
            case (int)Biomes.Volcanic:
                volcanicEnhanced = true;
                break;

            case (int)Biomes.Forest:
                forestEnhanced = true;
                break;
        }
    }

    ////Rifled tower bullet dmg.
    //protected void OnCollisionEnter(Collision collision) //    protected void OnCollisionEnter(Collision collision)
    //{
    //    print("im trhit!");
    //    if (collision.gameObject.GetComponent<RifledBullet>())
    //    {
    //        RifledBullet bullet = collision.gameObject.GetComponent<RifledBullet>();
    //        string towerName = "";
    //        float dmg = 0;
    //        dmg = bullet.damage;
    //        towerName = bullet.towerName;
    //        ProcessHit(dmg, towerName);
    //        if (hitPoints <= 0)
    //        {
    //            //Adds gold upon death, then deletes the enemy.
    //            damageLog.UpdateKills(towerName, enemyName);
    //            KillsEnemyandAddsGold();
    //        }
    //        else
    //        {
    //            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
    //        }
    //    }
    //}

    // maybe make this go from a single projectile?  it has aan enmum for its type? orr refactor some of this?
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RifledBullet>())
        {
            RifledBullet bullet = other.gameObject.GetComponent<RifledBullet>();
            string towerName = "";
            float dmg = 0;
            dmg = bullet.damage;
            bullet.SetDamageToZero();
            towerName = bullet.towerName;
            ProcessHit(dmg, towerName);
            if (hitPoints <= 0)
            {
                if (!hasGold) // debate this.  NOt sure if ishould use thi or try to find WHY more. l THis should fix it but its a meh fix.
                {
                    return;
                }
                hasGold = false;
                //Adds gold upon death, then deletes the enemy.
                damageLog.UpdateKills(towerName, enemyName);
                KillsEnemyandAddsGold();
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
            }
        }
        else if (other.gameObject.GetComponent<MortarShell>())
        {
            MortarShell bullet = other.gameObject.GetComponent<MortarShell>();
            string towerName = "";
            float dmg = 0;
            dmg = bullet.damage;
            bullet.SetDamageToZero();
            towerName = bullet.towerName;
            ProcessHit(dmg, towerName);
            if (hitPoints <= 0)
            {
                if (!hasGold) // debate this.  NOt sure if ishould use thi or try to find WHY more. l THis should fix it but its a meh fix.
                {
                    return;
                }
                hasGold = false;
                //Adds gold upon death, then deletes the enemy.
                damageLog.UpdateKills(towerName, enemyName);
                KillsEnemyandAddsGold();
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
            }
        }

    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<RifledBullet>())
        {
            print("im treated like a particle!");
        }

        string towerName = "";
        float dmg = 0;
        dmg = other.GetComponentInParent<Tower>().Damage(ref towerName);
        ProcessHit(dmg, towerName);
        if (hitPoints <= 0)
        {
            //Adds gold upon death, then deletes the enemy.
            damageLog.UpdateKills(towerName, enemyName);
            KillsEnemyandAddsGold();
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
        }
    }

    public void KillsEnemyandAddsGold()
    {
        FindObjectOfType<GoldManagement>().AddGold(goldForMyHead);

        Instantiate(deathPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(enemyDiedAudio, Camera.main.transform.position);
        EnemySpawner.EnemyAliveList.Remove(this);

        KillEnemy();
    }

    public virtual void KillEnemy()
    {
        Destroy(gameObject);
    }

    protected virtual void ProcessHit(float dmg, string towerName)
    {
        //string towerName = "";
        //float dmg = 0;
        //dmg = other.GetComponentInParent<Tower>().Damage(ref towerName);        
        hitPoints = hitPoints - dmg;
        hitparticleprefab.Play();
        RefreshHealthBar();
        //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);

        damageLog.UpdateDamage(towerName, dmg);
        Singleton.AddTowerDamage(towerName, dmg);
    }

    public virtual void HitByNonProjectile(float damage, string towerName)
    {
        float dmg = damage;
        hitPoints = hitPoints - dmg;
        RefreshHealthBar();
        //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);


        Singleton.AddTowerDamage(towerName, damage);
        hitparticleprefab.Play();

        if (hitPoints <= 0)
        {
            // if it has already been killed and is waiting for cleanup / dlete, dont double dip gold.
            if (!hasGold)
            {
                return;
            }
            hasGold = false;
            //Adds gold upon death, then deletes the enemy.
            damageLog.UpdateDamageAndKills(towerName, dmg, enemyName);
            KillsEnemyandAddsGold();          
        }
        else
        {
            damageLog.UpdateDamage(towerName, dmg);
            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
        }
    }

    public void HealingBuffed(float healPercent)
    {
        // if an enemy is standing in a puddle and by a healer, get puddles healing first.
        //if currenthealing is higher, keep higher healing.  on healing timeout it is reset to 0 (will trigger a reassessment)
        if (this.healPercent > healPercent)
        {
            return;
        }
        else if (this.healPercent != healPercent) // recalclate the healPerTick ONLY on new percentage.
        {
            this.healPercent = healPercent;
            healPerTick = (healPercent * hitPointsMax * (1 - _healReduction)); // move this only if different.
            if (isBoss) // bosses need to be killable as well 
            {
                healPerTick = healPerTick / 10f;
            }
        }


        healTime = 0f;
        healing = true;
        //float healPerTick = (healPercent * hitPoints);

    }

    public IEnumerator Healing(float healPercent)
    {

        //print("HPT: " + healPerTick + " HPerc: " + healPercent + " HPM: " + hitPointsMax);

        if (healing && healTime < healTimer)
        {
            healTime += 1 * Time.deltaTime;
            // if he is full or more its hald effective as armor, otherwise full heal.
            if (hitPoints >= hitPointsMax)
            {
                hitPoints += (healPerTick * Time.deltaTime) / 2;
            }
            else
            {
                hitPoints += (healPerTick * Time.deltaTime);
            }
            RefreshHealthBar();
            //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
            //print("I healed " + healPerTick + " HP!" + "   | healPercent: " + healPercent);
        }
        else
        {
            healing = false;
            this.healPercent = 0.0f;
        }
        yield return new WaitForSeconds(1f);
    }

    //public void GotToEnd()
    //{
    //    Instantiate(deathPrefab, transform.position, Quaternion.identity);
    //    endPrefab.Play();
    //    KillEnemy();
    //}




}
