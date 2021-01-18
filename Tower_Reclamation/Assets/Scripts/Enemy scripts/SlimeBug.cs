using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBug : MonoBehaviour {

    //obsolete block
    [SerializeField] Slime slime;

    //fancy new slime from wife.
    [SerializeField] Slime slime_Straight;
    [SerializeField] Slime slime_Curved;

    float slimeMultiplier = 1.4f;
    // Use this for initialization
    void Start()
    {
        GetComponent<EnemyMovement>().willSlime = true;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnSlime(Vector3 oldLoc, Vector3 newLoc) // average them together for the inbetween
    {
        Vector3 slimeLocation = new Vector3(((oldLoc.x + newLoc.x) / 2), ((oldLoc.y + newLoc.y) / 2), ((oldLoc.z + newLoc.z) / 2));
        Instantiate(slime, slimeLocation, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
        
        //ExperimentalSlime(slimeLocation);
    }
    private Color[] colors;
    

    public void SpawnSlime(Vector3 oldLoc, Vector3 newLoc, Vector3 nextLoc) // average them together for the inbetween
    {
        int dimensions = 0;
        Vector3 LocationalDifference = (oldLoc - nextLoc);
        if (LocationalDifference.x != 0f)
        {
            dimensions++;
        }
        if(LocationalDifference.z != 0f)
        {
            dimensions++; 
        }

        if (dimensions == 1)
        {
            if (LocationalDifference.x != 0f)
            {
                Slime slimeStraight = Instantiate(slime_Straight, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            } else
            {
                Slime slimeStraight = Instantiate(slime_Straight, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
            }
        } else
        {
            Vector3 LocationalDifference1 = (oldLoc - newLoc);
            Vector3 LocationalDifference2 = (newLoc - nextLoc);

            if (LocationalDifference1.x > 0f)
            {
                if (LocationalDifference2.z > 0f)
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                else
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                }
            }
            else if (LocationalDifference1.x < 0f)
            {
                if (LocationalDifference2.z > 0f)
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                else
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                }
            }
            else if (LocationalDifference1.z < 0f)
            {
                if (LocationalDifference2.x > 0f)
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                }
                else
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                }
            }
            else if (LocationalDifference1.z > 0f)
            {
                if (LocationalDifference2.x < 0f)
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else
                {
                    Slime slimeStraight = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
                    slimeStraight.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
            }


            //if (LocationalDifference.x < 0f)
            //{
            //    if (LocationalDifference.z < 0f)
            //    {
            //        Slime slimeCurved = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
            //    } else
            //    {
            //        Slime slimeCurved = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
            //        slimeCurved.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            //    }
            //} else
            //{
            //    if (LocationalDifference.z < 0f)
            //    {
            //        Slime slimeCurved = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
            //        slimeCurved.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            //    }
            //    else
            //    {
            //        Slime slimeCurved = Instantiate(slime_Curved, newLoc, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);
            //        slimeCurved.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            //    }
            //}
        }

        //Vector3 slimeLocation = new Vector3(((oldLoc.x + newLoc.x) / 2), ((oldLoc.y + newLoc.y) / 2), ((oldLoc.z + newLoc.z) / 2));
        //Instantiate(slime, slimeLocation, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);

        //ExperimentalSlime(slimeLocation);
    }

    private void ExperimentalSlime(Vector3 slimeLocation)
    {
        Slime slimeStraight = Instantiate(slime_Straight, slimeLocation, Quaternion.identity, GameObject.FindWithTag("Slime Container").gameObject.transform);

        ////Vector3 slimeLocation = new Vector3(((oldLoc.x + newLoc.x) / 2), ((oldLoc.y + newLoc.y) / 2), ((oldLoc.z + newLoc.z) / 2));
        //Renderer[] rendererObjects = slimeStraight.GetComponentsInChildren<Renderer>();
        ////create a cache of colors if necessary
        //colors = new Color[rendererObjects.Length];

        //// store the original colours for all child objects
        //for (int i = 0; i < rendererObjects.Length; i++)
        //{
        //    rendererObjects[i].
        //    colors[i] = rendererObjects[i].material.color;
        //}

        //for (int i = 0; i < rendererObjects.Length; i++)
        //{
        //    Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
        //    newColor.a =  .25f;//Mathf.Min(newColor.a, .25f);
        //    rendererObjects[i].material.SetColor("_Color", newColor);
        //}



        //Material m = slimeStraight.GetComponentInChildren<Material>();
        //m.shader.
    }
}
