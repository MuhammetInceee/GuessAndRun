using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBangBang : MonoBehaviour
{
    public List<Rigidbody> piece;
    private GameObject BrickParent;

    public string wallName;
    
    private void Awake()
    {
        BrickParent = transform.GetChild(1).gameObject;

        foreach (Transform tr in BrickParent.transform)
        {
            piece.Add(tr.gameObject.GetComponent<Rigidbody>());
           
        }
        
        foreach (Rigidbody rb in piece)
        {
            rb.isKinematic = true;
        }
    }
    
    public void BangBang(float Force)
    {
        foreach (Rigidbody rb in piece)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * Force, ForceMode.Force);
            Destroy(rb.gameObject, Random.Range(1f,3f));
        }
    }

    public void MaterialChanger(Material mat)
    {
        foreach (Rigidbody rb in piece)
        {
            rb.GetComponent<MeshRenderer>().material = mat;
        }
    }
}
