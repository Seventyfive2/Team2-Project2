using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitProjectile : MonoBehaviour
{
    [SerializeField] float angleSpread = 27.5f;

    [SerializeField] private int nbrOfProjectiles = 3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3[] GetDirections(int amt, float degrees)
    {
        Vector3[] result = new Vector3[amt];

        for (int i = 0; i < result.Length; i++)
        {
            Quaternion upRayRotation = Quaternion.AngleAxis(degrees * (i+1), transform.forward);

            result[i] = upRayRotation * transform.forward;
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        int lines = nbrOfProjectiles;

        if (nbrOfProjectiles % 2 != 0)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
            lines--;
        }

        Vector3[] posDir = GetDirections(lines / 2, angleSpread);

        Gizmos.color = Color.yellow;
        for (int n = 0; n < posDir.Length; n++)
        {
            Gizmos.DrawLine(transform.position,  posDir[n]);
            //Debug.Log(negDir[n]);
        }

        Gizmos.color = Color.blue;
        for (int n = 0; n < posDir.Length; n++)
        {
            Gizmos.DrawLine(transform.position,  transform.position - (posDir[n] + transform.forward));
            //Debug.Log(posDir[n]);
        }

        /*
        for (int i = 0; i < nbrOfProjectiles; i++)
        {
            if(i == 0 && nbrOfProjectiles%2 != 0)
            {
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
                lines--;
            }
            else
            {
                Vector3[] negDir = GetDirections(nbrOfProjectiles / 2, -angleSpread);
                Vector3[] posDir = GetDirections(nbrOfProjectiles / 2, angleSpread);

                for(int n = 0; n < negDir.Length; n++)
                {
                    Gizmos.DrawLine(transform.position, transform.position + negDir[n]);
                }

                for (int n = 0; n < negDir.Length; n++)
                {
                    Gizmos.DrawLine(transform.position, transform.position + negDir[n]);
                }
            }
        }
        */
    }
}
