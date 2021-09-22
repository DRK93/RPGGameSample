using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float refreshRate;
    public float particleSize;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VFXGraphUpdate());
    }

    IEnumerator VFXGraphUpdate()
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
        }
        
    }
}
