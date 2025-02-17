using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class UnscaledTimeShaderWrapper : MonoBehaviour
    {

        [SerializeField] private Material mat;
        void Start()
        {
            if (mat == null)
            {
                mat = GetComponent<MeshRenderer>().material;
            }
        }

        void Update()
        {
            mat.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}