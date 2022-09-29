
using UnityEngine;

public class RandomeObjectColor : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponentInChildren<MeshRenderer>().material.color = Random.ColorHSV(0.0f, 1.0f, 0.75f, 1.0f, 0.5f, 1.0f);


    }
}
