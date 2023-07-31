using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandPlace : MonoBehaviour
{
    public Transform tip;
    public Transform input_sphere;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = (input_sphere.position + tip.position) / 2;
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, (input_sphere.position - tip.position).magnitude/2f, this.gameObject.transform.localScale.z);
        this.gameObject.transform.up= (input_sphere.position-tip.position);
    }
}
