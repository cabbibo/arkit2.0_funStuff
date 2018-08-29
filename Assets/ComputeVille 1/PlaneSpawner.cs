using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using ComputeVille;

public class PlaneSpawner : MonoBehaviour {

    public GameObject SDF;
    public GameObject QuadPrefab;

    private MeshIntersection mesh;
    private BufferSDF vol;

    public int maxMeshes;
    public WrapQuadOnSDF[] quads;
    public VideoPlayer[] vids;
    public int currentQuad;

	// Use this for initialization
	void Start () {

        vol = SDF.GetComponent<BufferSDF>();
        mesh = SDF.GetComponent<MeshIntersection>();
        quads = new WrapQuadOnSDF[maxMeshes];
        vids = new VideoPlayer[maxMeshes];

        for( int i = 0; i < maxMeshes; i++ ){
            GameObject o = Instantiate( QuadPrefab );
            o.transform.parent = transform;
            WrapQuadOnSDF wq = o.GetComponent<WrapQuadOnSDF>();
            VideoPlayer v = o.GetComponent<VideoPlayer>();


            wq.mesh = mesh;
            wq.volBuffer = vol;

            quads[i] = wq;
            vids[i] = v;

        }

          mesh.OnTouchDown += SetPlane;
	}

	// Update is called once per frame
	void Update () {

	}

    void SetPlane( Transform t, Vector3 p , Vector3 n){

        quads[currentQuad].SetPlane(p,n);
        vids[currentQuad].frame = 0;
        vids[currentQuad].Play();

        currentQuad++;
        currentQuad%= maxMeshes;

    }
}
