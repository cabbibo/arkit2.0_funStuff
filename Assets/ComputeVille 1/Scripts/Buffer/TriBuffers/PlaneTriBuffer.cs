using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{

[RequireComponent(typeof(PlaneVertBuffer))]
public class PlaneTriBuffer : TriangleBuffer {


    PlaneVertBuffer vertBuffer;

    private int triIndexCount = 0;

    public override void GetTriangles(){
        vertBuffer = GetComponent<PlaneVertBuffer>();
        count = (vertBuffer.Width - 1) * (vertBuffer.Height - 1) * 6;
        int[] triangles = new int[count];

        int index = 0;

        for (int i = 0; i < vertBuffer.Width - 1; i++){
            for (int j = 0; j < vertBuffer.Height - 1; j++){

                triangles[index++] = i * vertBuffer.Width + j;
                triangles[index++] = i * vertBuffer.Width + j + 1;
                triangles[index++] = (i + 1) * vertBuffer.Width + j + 1;
                triangles[index++] = i * vertBuffer.Width + j;
                triangles[index++] = (i + 1) * vertBuffer.Width + j + 1;
                triangles[index++] = (i + 1) * vertBuffer.Width + j;

            }
        }

        values = triangles;

    }
}
}
