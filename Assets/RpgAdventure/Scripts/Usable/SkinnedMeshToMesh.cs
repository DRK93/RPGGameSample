using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    // class to create radiants effects on character skin
    // for enemies use just one effect, but for players more.
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect HealingVFX;
    public VisualEffect SmallRadiatingVFX;
    public VisualEffect BigRadiatingVFX;
    public VisualEffect FireRadiantVFX;
    [SerializeField]
    private float refreshRate1;
    [SerializeField]
    private float particleSize1;
    [SerializeField]
    private float refreshRate2;
    [SerializeField]
    private float particleSize2;
    [SerializeField]
    private float refreshRate3;
    [SerializeField]
    private float particleSize3;
    [SerializeField]
    private float refreshRate4;
    [SerializeField]
    private float particleSize4;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.gameObject.tag == "Enemy")
            StartCoroutine(VFXGraphUpdate(BigRadiatingVFX, refreshRate3, particleSize3));
        else
        {
            StartCoroutine(VFXGraphUpdate(HealingVFX, refreshRate1, particleSize1));
            StartCoroutine(VFXGraphUpdate(SmallRadiatingVFX, refreshRate2, particleSize2));
            StartCoroutine(VFXGraphUpdate(BigRadiatingVFX, refreshRate3, particleSize3));
            StartCoroutine(VFXGraphUpdate(FireRadiantVFX, refreshRate4, particleSize4));
        }

    }
    IEnumerator VFXGraphUpdate(VisualEffect VFXGraph, float refreshRate, float particleSize)
    {
        while(gameObject.activeSelf)
        {
            Mesh mesh1 = new Mesh();
            skinnedMesh.BakeMesh(mesh1);
            Vector3[] vertices = mesh1.vertices;
            Mesh mesh2 = new Mesh();
            mesh2.vertices = vertices;
            VFXGraph.SetMesh("PlayerMesh", mesh2);
            VFXGraph.SetFloat("ParticleSize", particleSize);
            yield return new WaitForSeconds(refreshRate);
            Destroy(mesh2);
            Destroy(mesh1);
        }
        
    }
}
