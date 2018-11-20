using UnityEngine;
using System.Collections;

public class lightcontrol : MonoBehaviour
{


    public Light Lights;
    public GameObject bookcaseholder;
    void Awake()
    {
        bookcaseholder = GameObject.Find("BookChooseScript");

    }
    void LateUpdate()
    {
        if (bookcaseholder.GetComponent<Bookstore>().daylightson == true)
        {
            Lights.enabled = false;

        }
        if (bookcaseholder.GetComponent<Bookstore>().nightallbooklighton == true)
        {
            Lights.enabled = true;

        }
        if (bookcaseholder.GetComponent<Bookstore>().nighteachbooklighton == true)
        {
            if (this.name == bookcaseholder.GetComponent<Bookstore>().booknum.ToString())
            {
                Lights.enabled = true;
            }
            else
            {
                Lights.enabled = false;

            }
        }
    }
}
