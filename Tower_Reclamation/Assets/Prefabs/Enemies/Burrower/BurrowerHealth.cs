using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowerHealth : EnemyHealth {

    protected BurrowerMovement burrowerMove;
    [SerializeField] protected SandStorm sandStorm;
    
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        enemyName = "Burrower";
        burrowerMove = GetComponent<BurrowerMovement>();
        hitPoints = hitPoints * .65f;
        hitPointsMax = hitPoints;
        burrowed = burrowerMove.burrowed;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    public void Burrowed()
    {
        burrowed = true;
    }
    public void Unburrowed()
    {
        burrowed = false;
    }

    public void TellMovementToStartBurrow()
    {
        burrowerMove.IWasHit();
    }

    override public void HitByNonProjectile(float damage, string towerName)
    {
        if (burrowed) // cant shoot me im underground bitch.
        {
            return;
        }

        float dmg = damage;
        hitPoints = hitPoints - dmg;
        healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
        TellMovementToStartBurrow();
        hitparticleprefab.Play();
        Singleton.AddTowerDamage(towerName, damage);

        if (hitPoints <= 0)
        {
            // if it has already been killed and is waiting for cleanup / dlete, dont double dip gold.

            //Adds gold upon death, then deletes the enemy.
            KillsEnemyandAddsGold();
            damageLog.UpdateDamageAndKills(towerName, damage, enemyName);
        }
        else
        {
            damageLog.UpdateDamage(towerName, damage);
            GetComponent<AudioSource>().PlayOneShot(enemyHitAudio);
        }
    }

    protected override void OnTriggerEnter(Collider other)
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

            TellMovementToStartBurrow();

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
                TellMovementToStartBurrow();
            }
        }
    }

    override protected void OnParticleCollision(GameObject other)
    {
        if (burrowed) // cant shoot me im underground bitch.
        {
            return;
        }

        string towerName = "";
        float dmg = 0;
        dmg = other.GetComponentInParent<Tower>().Damage(ref towerName);
        ProcessHit(dmg, towerName);

        RefreshHealthBar();
        //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
        TellMovementToStartBurrow();
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

    override public IEnumerator Burning(float fireDmg)
    {
        //burning underground sucks, but ill allow it ATM.
        if (burrowed)
            yield return new WaitForSeconds(1f);

        if (hitPoints < 1)
        {
            KillsEnemyandAddsGold();
        }
        if (onFire && time > 0)
        {
            time -= 1 * Time.deltaTime;
            hitPoints -= burnDmg * Time.deltaTime;
            TellMovementToStartBurrow();

            RefreshHealthBar();
            //healthBars.SetHealthBarPercent(hitPoints / hitPointsMax);
            healthBars.SetBurnBarPercent(time / burnTime);
        }
        else
        {
            onFire = false;
        }
        yield return new WaitForSeconds(1f);
    }

    public override void KillEnemy()
    {
        if (volcanicEnhanced)
        {
            SandStorm SS = Instantiate(sandStorm, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    override public void GiveEnhancement(int biome)
    {
        switch (biome)
        {
            case (int)Biomes.Volcanic:
                hitPoints = hitPoints * 1.15f;
                hitPointsMax = hitPoints;
                volcanicEnhanced = true;

                break;
        }
    }

}
