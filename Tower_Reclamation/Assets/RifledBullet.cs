using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RifledBullet : MonoBehaviour {

    // Use this for initialization
    public float maxLifetime = 6f;
    public float lifeTimer = 0f;
    public float moveSpeed;
    public float damage = 0f;
    public string towerName;
    Transform target = null;
    private bool instantiated = false;

	void Start () {
        moveSpeed = 60f;
        maxLifetime = 6.5f;
        lifeTimer = 0f;
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
        if(lifeTimer > maxLifetime)
        {
            Destroy(this.gameObject);
        }

        //try to move.  If target is null destroy this.  It prolly died.
        try
        {
            float movementPerFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, target.position, movementPerFrame);
        }
        catch (MissingReferenceException ms)
        {
            Destroy(this.gameObject);
        }
        catch (NullReferenceException nr)
        {
            Destroy(this.gameObject);
        }
        catch(Exception e)
        {
            print("=======bullets exception ======" + e.Message);
            Destroy(this.gameObject);
        }


	}

    RifledBullet(Transform enemyTransform, float towerDamage, string _towerName) // maybe add movespeed?
    {
        target = enemyTransform;
        damage = towerDamage;
        towerName = _towerName;
    }
     
    public void Instantiate(Transform enemyTransform, float towerDamage, string _towerName, float heightOffset)
    {
        instantiated = true;
        try
        {
            target = enemyTransform;
            damage = towerDamage;
            towerName = _towerName;
            this.transform.position += new Vector3(0f, heightOffset, 0f);
        }
        catch (Exception e )
        {
            //dont need to do anything.  If it errored it will kill itself prolly.
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("pop1");
    //    try
    //    {
    //        EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
    //        if (enemy != null)
    //        {
    //            print("pop2");
    //            Destroy(this.gameObject);
    //        }
    //    }
    //    catch (Exception e)
    //    {

    //    }
    //}

    // temporary work around.  When it hits an enemy, set its damage to zero.  Destroy hits at end of frame, atm it can hit multiple enemies if it luckily overlaps.
    public void SetDamageToZero()
    {
        damage = 0;
        Destroy(this.gameObject);
    }

}
