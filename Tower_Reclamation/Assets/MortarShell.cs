using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MortarShell : MonoBehaviour {

    // Use this for initialization
    public float maxLifetime = 8f;
    public float lifeTimer = 0f;
    public float moveSpeed;
    public float damage = 0f;
    public float burnDmg = 0f;
    public float flameDurationIncrease = 0f;
    public bool instantiated = false;
    Transform target = null;
    Vector3 targetsLastPosition = new Vector3(0, 0, 0);

    [SerializeField] FireBombBurnAOE fireAOE;
    private bool alreadyExploded = false;

    public string towerName = "";

    private float arcTime = 1.5f;
    private float straightUpTime = .75f;
    private float falloffTime = 1f;
    private float totalRisingTime = -1f;
    private float currentTime = 0f;

    private float scaledFireSize;
	// Use this for initialization
	void Start () {
        alreadyExploded = false;
        moveSpeed = 42f;
        maxLifetime = 8f;
        lifeTimer = 0f;
        totalRisingTime = straightUpTime + falloffTime;
    }
	
	// Update is called once per frame
	void Update () {
        // if not instantiated do nothing.  This is done via function call Instantiate
        if (!instantiated)
        {
            return;
        }

        // just have a timeout for weird stuff.
        lifeTimer += Time.deltaTime;
        currentTime += Time.deltaTime;

        if (lifeTimer > maxLifetime)
        {
            Destroy(this.gameObject);
        }

        //try to move.  If target is null destroy this.  It prolly died.
        try
        {
            float movementPerFrame = moveSpeed * Time.deltaTime;

            float percent = currentTime / arcTime;
            if (target != null)
            {
                targetsLastPosition = target.position;
                if (currentTime < arcTime)
                {
                    transform.Translate(Vector3.up * movementPerFrame * (1 - percent));
                    transform.position = Vector3.MoveTowards(this.transform.position, target.position, movementPerFrame * (percent));
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, target.position, movementPerFrame);
                }
            } else
            {
                if (currentTime < arcTime)
                {
                    transform.Translate(Vector3.up * movementPerFrame * (1 - percent));
                    transform.position = Vector3.MoveTowards(this.transform.position, targetsLastPosition, movementPerFrame * (percent));
                }
                else
                {
                    transform.position = Vector3.MoveTowards(this.transform.position, targetsLastPosition, movementPerFrame);
                }
                if(Vector3.Distance(this.transform.position, targetsLastPosition) < 1)
                {
                    SetDamageToZero();
                }
            }


        }
        catch (MissingReferenceException ms)
        {
            Destroy(this.gameObject);
        }
        catch (NullReferenceException nr)
        {
            Destroy(this.gameObject);
        }
        catch (Exception e)
        {
            print("=======bullets exception ======" + e.Message);
            Destroy(this.gameObject);
        }
    }

    public void Instantiate(Transform enemyTransform, float towerDamage, float _burnDmg, string _towerName, float scaledSizeIncrease, float _flameDurationIncrease, float heightOffset)
    {
        instantiated = true;
        try
        {
            target = enemyTransform;
            damage = towerDamage;
            towerName = _towerName;
            burnDmg = _burnDmg;
            scaledFireSize = scaledSizeIncrease;
            flameDurationIncrease = _flameDurationIncrease;
        }
        catch (Exception e)
        {
            //dont need to do anything.  If it errored it will kill itself prolly.
        }

    }


    public void SetDamageToZero()
    {
        if (!alreadyExploded) {
            alreadyExploded = true;
            damage = 0;
            SpawnFireAOE();
            Destroy(this.gameObject);
        }
    }

    private void SpawnFireAOE()
    {
        FireBombBurnAOE flames = Instantiate(fireAOE, transform.position, Quaternion.identity);
        flames.Initialize(burnDmg, scaledFireSize, flameDurationIncrease);
    }
}
