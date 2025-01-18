using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXMeshTrail : MonoBehaviour
{
    public float activeTime = 5f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive;
   [SerializeField] private SkinnedMeshRenderer[] SMR;

    public void ActivateFX()
    {

        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }

    }

    public void ActivateFX(float time)
    {
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(time));
        }

    }

    public void DeactivateFX()
    {
        StopAllCoroutines();
    }

    IEnumerator ActivateTrail(float _timeActive)
    {

        GameObject gObP = new GameObject(); 
        gObP.name = "Lowell Afterimage";

        while (_timeActive > 0)
        {

            _timeActive -= meshRefreshRate;

            if (SMR == null)
            {
                SMR = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < SMR.Length; i++)
            {
                GameObject gOb = new GameObject();

                gOb.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gOb.AddComponent<MeshRenderer>();
                MeshFilter mf = gOb.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                SMR[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                gOb.name = SMR[i].name;
                gOb.transform.parent = gObP.transform;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));
                
                Destroy(gOb, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {

        float valueaToAnimate = mat.GetFloat(shaderVarRef);

        while (valueaToAnimate > goal)
        {
            valueaToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueaToAnimate);
            yield return new WaitForSeconds(refreshRate);

        }

    }

}
