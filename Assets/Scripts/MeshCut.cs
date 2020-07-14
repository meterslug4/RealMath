using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCut
{

    private static Plane blade;
    private static Transform victim_transform;
    private static Mesh victim_mesh;

    private static bool[] sides = new bool[3];

    // Gathering values
    private static List<int>[] left_Gather_subIndices = new List<int>[] { new List<int>(), new List<int>() };
    private static List<int>[] right_Gather_subIndices = new List<int>[] { new List<int>(), new List<int>() };

    private static List<Vector3>[] left_Gather_added_Points = new List<Vector3>[] { new List<Vector3>(), new List<Vector3>() };
    private static List<Vector2>[] left_Gather_added_uvs = new List<Vector2>[] { new List<Vector2>(), new List<Vector2>() };
    private static List<Vector3>[] left_Gather_added_normals = new List<Vector3>[] { new List<Vector3>(), new List<Vector3>() };

    private static List<Vector3>[] right_Gather_added_Points = new List<Vector3>[] { new List<Vector3>(), new List<Vector3>() };
    private static List<Vector2>[] right_Gather_added_uvs = new List<Vector2>[] { new List<Vector2>(), new List<Vector2>() };
    private static List<Vector3>[] right_Gather_added_normals = new List<Vector3>[] { new List<Vector3>(), new List<Vector3>() };



    // face cutting temps
    private static Vector3 leftPoint1 = Vector3.zero;
    private static Vector3 leftPoint2 = Vector3.zero;
    private static Vector3 rightPoint1 = Vector3.zero;
    private static Vector3 rightPoint2 = Vector3.zero;

    private static Vector2 left_uv1 = Vector3.zero;
    private static Vector2 left_uv2 = Vector3.zero;
    private static Vector2 right_uv1 = Vector3.zero;
    private static Vector2 right_uv2 = Vector3.zero;

    private static Vector3 left_normal1 = Vector3.zero;
    private static Vector3 left_normal2 = Vector3.zero;
    private static Vector3 right_normal1 = Vector3.zero;
    private static Vector3 right_normal2 = Vector3.zero;


    // final arrays
    private static List<int>[] left_Final_subIndices = new List<int>[] { new List<int>(), new List<int>() };

    private static List<Vector3> left_Final_vertices = new List<Vector3>();
    private static List<Vector3> left_Final_normals = new List<Vector3>();
    private static List<Vector2> left_Final_uvs = new List<Vector2>();

    private static List<int>[] right_Final_subIndices = new List<int>[] { new List<int>(), new List<int>() };

    private static List<Vector3> right_Final_vertices = new List<Vector3>();
    private static List<Vector3> right_Final_normals = new List<Vector3>();
    private static List<Vector2> right_Final_uvs = new List<Vector2>();

    // capping stuff
    private static List<Vector3> createdVertexPoints = new List<Vector3>();

    public static GameObject[] Cut(GameObject victim, Vector3 anchorPoint, Vector3 normalDirection, Material capMaterial)
    {
        Vectexs.Get.vertexs.Clear();//자르기전에 배열 초기화
        Vectexs.Get.vertexs_RemoveDuple.Clear();
        Vectexs.Get.removeIndex.Clear();

        victim_transform = victim.transform;

        blade = new Plane(victim.transform.InverseTransformDirection(-normalDirection),
                          victim.transform.InverseTransformPoint(anchorPoint));

        victim_mesh = victim.GetComponent<MeshFilter>().mesh;
        victim_mesh.subMeshCount = 2;

        ResetGatheringValues();


        int p1 = 0;
        int p2 = 0;
        int p3 = 0;

        sides = new bool[3];

        int sub = 0;
        int[] indices = victim_mesh.triangles; //메쉬의 모든 삼격면 정보를 가지고 있는 배열임 정점 배열의 정보를 가지고 있는 삼각면들의 목록을 나타냄 배열의 크기는 3의 배수여야함
        int[] secondIndices = victim_mesh.GetIndices(1);//Returns the index buffer for the submesh.
        //Debug.Log("트라이앵글 갯수 = "+ indices.Length);
        for (int i = 0; i < indices.Length; i += 3) //3씩 증가 
        {

            p1 = indices[i];   //한번 루프돌때 i 와 i+1 i+2 까지 담아서 3개씩 검하를하기때문에 루프 끝나고 i+=3을해서 3씩 증가
            p2 = indices[i + 1];
            p3 = indices[i + 2];

            sides[0] = blade.GetSide(victim_mesh.vertices[p1]);//평면의 표면이 어느방향인지 확인 방향이 양의 방향인지 여부
            sides[1] = blade.GetSide(victim_mesh.vertices[p2]);
            sides[2] = blade.GetSide(victim_mesh.vertices[p3]);


            sub = 0;
            for (int k = 0; k < secondIndices.Length; k++)
            {
                if (secondIndices[k] == p1)
                {
                    sub = 1;
                    break;
                }
            }

            //해당 물체의 triangle들을 순차적으로 돌면서 그에 매치되는 점들을 가져와 3점이 모두 P의 양(+)/음(-)의 방향에 있다면 Plane 기준으로 Mesh 저장
            // 만일 모두 음/양의 방향이 아니라면 그 삼각형은 평면에 의해 잘리는 것으로 판단되어, 동일한 방향의 점 S1,S2 와 한개의 다른 점 O1 를 잇는 선을 그어 평면과의 교점을 C1,C2라함
            //시계방향 재정렬?
            if (sides[0] == sides[1] && sides[0] == sides[2])
            { // whole face

                if (sides[0])
                { // left side
                    left_Gather_subIndices[sub].Add(p1);
                    left_Gather_subIndices[sub].Add(p2);
                    left_Gather_subIndices[sub].Add(p3);

                }
                else
                {

                    right_Gather_subIndices[sub].Add(p1);
                    right_Gather_subIndices[sub].Add(p2);
                    right_Gather_subIndices[sub].Add(p3);

                }

            }
            else
            { // cut face
                ResetFaceCuttingTemps();
                Cut_this_Face(sub, p1, p2, p3);
            }
        }
       

        // set final arrays
        ResetFinalArrays();
        SetFinalArrays_withOriginals();
        AddNewTriangles_toFinalArrays();
        MakeCaps();

        Mesh left_HalfMesh = new Mesh();
        left_HalfMesh.name = "Split Mesh Left";
        left_HalfMesh.vertices = left_Final_vertices.ToArray(); //잘린면의 매쉬의 버텍스들?
        
        left_HalfMesh.subMeshCount = 2;
        left_HalfMesh.SetIndices(left_Final_subIndices[0].ToArray(), MeshTopology.Triangles, 0);
        left_HalfMesh.SetIndices(left_Final_subIndices[1].ToArray(), MeshTopology.Triangles, 1);

        left_HalfMesh.normals = left_Final_normals.ToArray();
        left_HalfMesh.uv = left_Final_uvs.ToArray();

        //Vectexs.Get.vertexs = left_HalfMesh.vertices;
        //Vectexs.Get.vertexs = left_HalfMesh.normals;
        //Vectexs.Get.isGet = true;


        Mesh right_HalfMesh = new Mesh();
        right_HalfMesh.name = "Split Mesh Right";
        right_HalfMesh.vertices = right_Final_vertices.ToArray();

        right_HalfMesh.subMeshCount = 2;
        right_HalfMesh.SetIndices(right_Final_subIndices[0].ToArray(), MeshTopology.Triangles, 0);
        right_HalfMesh.SetIndices(right_Final_subIndices[1].ToArray(), MeshTopology.Triangles, 1);

        right_HalfMesh.normals = right_Final_normals.ToArray();
        right_HalfMesh.uv = right_Final_uvs.ToArray();

        victim.name = "leftSide";
        victim.GetComponent<MeshFilter>().mesh = left_HalfMesh;

        Material[] mats = new Material[] { victim.GetComponent<MeshRenderer>().material, capMaterial };

        GameObject leftSideObj = victim;

        GameObject rightSideObj = new GameObject("rightSide", typeof(MeshFilter), typeof(MeshRenderer));
        rightSideObj.transform.position = victim_transform.position;
        rightSideObj.transform.rotation = victim_transform.rotation;
        rightSideObj.GetComponent<MeshFilter>().mesh = right_HalfMesh;


        leftSideObj.GetComponent<MeshRenderer>().materials = mats;
        rightSideObj.GetComponent<MeshRenderer>().materials = mats;


        return new GameObject[] { leftSideObj, rightSideObj };

    }

    static void ResetGatheringValues()
    {

        left_Gather_subIndices[0].Clear();
        left_Gather_subIndices[1].Clear();
        left_Gather_added_Points[0].Clear();
        left_Gather_added_Points[1].Clear();
        left_Gather_added_uvs[0].Clear();
        left_Gather_added_uvs[1].Clear();
        left_Gather_added_normals[0].Clear();
        left_Gather_added_normals[1].Clear();

        right_Gather_subIndices[0].Clear();
        right_Gather_subIndices[1].Clear();
        right_Gather_added_Points[0].Clear();
        right_Gather_added_Points[1].Clear();
        right_Gather_added_uvs[0].Clear();
        right_Gather_added_uvs[1].Clear();
        right_Gather_added_normals[0].Clear();
        right_Gather_added_normals[1].Clear();

        createdVertexPoints.Clear();

    }

    static void ResetFaceCuttingTemps()
    {

        leftPoint1 = Vector3.zero;
        leftPoint2 = Vector3.zero;
        rightPoint1 = Vector3.zero;
        rightPoint2 = Vector3.zero;

        left_uv1 = Vector3.zero;
        left_uv2 = Vector3.zero;
        right_uv1 = Vector3.zero;
        right_uv2 = Vector3.zero;

        left_normal1 = Vector3.zero;
        left_normal2 = Vector3.zero;
        right_normal1 = Vector3.zero;
        right_normal2 = Vector3.zero;

    }

    static void Cut_this_Face(int submesh, int index1, int index2, int index3)
    {

        int p = index1;
        for (int side = 0; side < 3; side++)
        {

            switch (side)
            {
                case 0:
                    p = index1;
                    break;
                case 1:
                    p = index2;
                    break;
                case 2:
                    p = index3;
                    break;

            }

            if (sides[side])
            {
                if (leftPoint1 == Vector3.zero)
                {

                    leftPoint1 = victim_mesh.vertices[p];
                    leftPoint2 = leftPoint1;
                    left_uv1 = victim_mesh.uv[p];
                    left_uv2 = left_uv1;
                    left_normal1 = victim_mesh.normals[p];
                    left_normal2 = left_normal1;

                }
                else
                {

                    leftPoint2 = victim_mesh.vertices[p];
                    left_uv2 = victim_mesh.uv[p];
                    left_normal2 = victim_mesh.normals[p];

                }
            }
            else
            {
                if (rightPoint1 == Vector3.zero)
                {

                    rightPoint1 = victim_mesh.vertices[p];
                    rightPoint2 = rightPoint1;
                    right_uv1 = victim_mesh.uv[p];
                    right_uv2 = right_uv1;
                    right_normal1 = victim_mesh.normals[p];
                    right_normal2 = right_normal1;

                }
                else
                {

                    rightPoint2 = victim_mesh.vertices[p];
                    right_uv2 = victim_mesh.uv[p];
                    right_normal2 = victim_mesh.normals[p];

                }
            }
        }
        //Debug.Log(left_normal1);//같은 값이 2쌍 나옴
        //Debug.Log(left_normal2);//같은 값이 2쌍 나옴

        float normalizedDistance = 0.0f;
        float distance = 0;// 원래는 0이 였음
        Ray ray=new Ray(leftPoint1, (rightPoint1 - leftPoint1).normalized);
        blade.Raycast(new Ray(leftPoint1, (rightPoint1 - leftPoint1).normalized), out distance);//레이가 평면을 교차했는지 여부
        //Debug.DrawRay(leftPoint1, (rightPoint1 - leftPoint1).normalized,Color.red,100.0f); //레이의 갯수가 꼭지점의 갯수이다?
        //Debug.Log("ray1의 교차점 :" +ray.GetPoint(distance));
        //Debug.Log(distance);
        
        //Vectexs.Get.vertexs.Add(ray.GetPoint(distance)); //교차점을 가져온다 완벽하지 않음 그대로 월드 좌표로 표한하면 같은 값이 중복됨 같은값은 거리가 같은데 방향없이 표시되는거 같음
        //Vectexs.Get.vertexsright.Add(ray.GetPoint(distance));
        //Vectexs.Get.isGet = true;
        //Debug.Log(leftPoint1);
        //Debug.Log(leftPoint2);//잘린 오브젝트의 꼭지점을 다가져옴?
        //Debug.Log(normalizedDistance);
        //Debug.Log((rightPoint1 - leftPoint1));
        normalizedDistance = distance / (rightPoint1 - leftPoint1).magnitude;
        //Debug.Log(normalizedDistance);
        Vector3 newVertex1 = Vector3.Lerp(leftPoint1, rightPoint1, normalizedDistance);

        //Vectexs.Get.vertexs.Add(newVertex1);
        // Vectexs.Get.isGet = true;

        //Debug.Log(newVertex1);
        Vector2 newUv1 = Vector2.Lerp(left_uv1, right_uv1, normalizedDistance);

       // Debug.Log("uvVector2 = "+newUv1);

        Vector3 newNormal1 = Vector3.Lerp(left_normal1, right_normal1, normalizedDistance);

        createdVertexPoints.Add(newVertex1);

        //Vectexs.Get.vertexs = createdVertexPoints; //단면의 버텍스들을 받아오는데 폴리곤에 들어가는 버텍스까지 포함되는거 같음
        //Vectexs.Get.isGet = true;

        Ray ray2=new Ray(leftPoint2, (rightPoint2 - leftPoint2).normalized);
        blade.Raycast(ray2, out distance);
        //Debug.Log("ray2의 교차점 :" +ray2.GetPoint(distance));
        //Vectexs.Get.vertexsright.Add(ray2.GetPoint(distance));
        //Vectexs.Get.vertexsright.Add(ray2.GetPoint(distance)); 
        //Vectexs.Get.isGet2 = true;

        normalizedDistance = distance / (rightPoint2 - leftPoint2).magnitude;
        Vector3 newVertex2 = Vector3.Lerp(leftPoint2, rightPoint2, normalizedDistance);
        Vector2 newUv2 = Vector2.Lerp(left_uv2, right_uv2, normalizedDistance);
        Vector3 newNormal2 = Vector3.Lerp(left_normal2, right_normal2, normalizedDistance);


        //Debug.Log(newVertex2);
        createdVertexPoints.Add(newVertex2);
        //Vectexs.Get.vertexs.Clear();
        Vectexs.Get.vertexs.Add(newVertex1);
        Vectexs.Get.vertexs.Add(newVertex2);
        Vectexs.Get.isGet = true;
        // first triangle
        Add_Left_triangle(submesh, newNormal1, new Vector3[] { leftPoint1, newVertex1, newVertex2 },
        new Vector2[] { left_uv1, newUv1, newUv2 },
        new Vector3[] { left_normal1, newNormal1, newNormal2 });

        // second triangle
        Add_Left_triangle(submesh, newNormal2, new Vector3[] { leftPoint1, leftPoint2, newVertex2 },
        new Vector2[] { left_uv1, left_uv2, newUv2 },
        new Vector3[] { left_normal1, left_normal2, newNormal2 });

        // first triangle
        Add_Right_triangle(submesh, newNormal1, new Vector3[] { rightPoint1, newVertex1, newVertex2 },
        new Vector2[] { right_uv1, newUv1, newUv2 },
        new Vector3[] { right_normal1, newNormal1, newNormal2 });

        // second triangle
        Add_Right_triangle(submesh, newNormal2, new Vector3[] { rightPoint1, rightPoint2, newVertex2 },
        new Vector2[] { right_uv1, right_uv2, newUv2 },
        new Vector3[] { right_normal1, right_normal2, newNormal2 });

    }

    static void Add_Left_triangle(int submesh, Vector3 faceNormal, Vector3[] points, Vector2[] uvs, Vector3[] normals)
    {

        int p1 = 0;
        int p2 = 1;
        int p3 = 2;
       // Debug.Log(points[0]);
        //Vectexs.Get.vertexs.Add(points[0]);
        //Vectexs.Get.isGet = true;
        Vector3 calculated_normal = Vector3.Cross((points[1] - points[0]).normalized, (points[2] - points[0]).normalized);//point0이 기준이 되는 꼭지점??
        //Debug.Log(calculated_normal); //법선벡터 법선벡터와 수직이 되는 벡터가 선분의 방향?
        //Vectexs.Get.vertexs.Add(calculated_normal);
        //Vectexs.Get.isGet = true;

        if (Vector3.Dot(calculated_normal, faceNormal) < 0)
        {

            p1 = 2;
            p2 = 1;
            p3 = 0;
        }

        left_Gather_added_Points[submesh].Add(points[p1]);
        left_Gather_added_Points[submesh].Add(points[p2]);
        left_Gather_added_Points[submesh].Add(points[p3]);

        left_Gather_added_uvs[submesh].Add(uvs[p1]);
        left_Gather_added_uvs[submesh].Add(uvs[p2]);
        left_Gather_added_uvs[submesh].Add(uvs[p3]);

        left_Gather_added_normals[submesh].Add(normals[p1]);
        left_Gather_added_normals[submesh].Add(normals[p2]);
        left_Gather_added_normals[submesh].Add(normals[p3]);

        //Vectexs.Get.vertexs = left_Gather_added_Points[0];
        //Vectexs.Get.isGet = true;
    }

    static void Add_Right_triangle(int submesh, Vector3 faceNormal, Vector3[] points, Vector2[] uvs, Vector3[] normals)
    {


        int p1 = 0;
        int p2 = 1;
        int p3 = 2;

        Vector3 calculated_normal = Vector3.Cross((points[1] - points[0]).normalized, (points[2] - points[0]).normalized);

        if (Vector3.Dot(calculated_normal, faceNormal) < 0)
        {

            p1 = 2;
            p2 = 1;
            p3 = 0;
        }


        right_Gather_added_Points[submesh].Add(points[p1]);
        right_Gather_added_Points[submesh].Add(points[p2]);
        right_Gather_added_Points[submesh].Add(points[p3]);

        right_Gather_added_uvs[submesh].Add(uvs[p1]);
        right_Gather_added_uvs[submesh].Add(uvs[p2]);
        right_Gather_added_uvs[submesh].Add(uvs[p3]);

        right_Gather_added_normals[submesh].Add(normals[p1]);
        right_Gather_added_normals[submesh].Add(normals[p2]);
        right_Gather_added_normals[submesh].Add(normals[p3]);

    }


    static void ResetFinalArrays()
    {

        left_Final_subIndices[0].Clear();
        left_Final_subIndices[1].Clear();
        left_Final_vertices.Clear();
        left_Final_normals.Clear();
        left_Final_uvs.Clear();

        right_Final_subIndices[0].Clear();
        right_Final_subIndices[1].Clear();
        right_Final_vertices.Clear();
        right_Final_normals.Clear();
        right_Final_uvs.Clear();

    }

    static void SetFinalArrays_withOriginals()
    {

        int p = 0;

        for (int submesh = 0; submesh < 2; submesh++)
        {

            for (int i = 0; i < left_Gather_subIndices[submesh].Count; i++)
            {

                p = left_Gather_subIndices[submesh][i];

                left_Final_vertices.Add(victim_mesh.vertices[p]);
                left_Final_subIndices[submesh].Add(left_Final_vertices.Count - 1);
                left_Final_normals.Add(victim_mesh.normals[p]);
                left_Final_uvs.Add(victim_mesh.uv[p]);

            }

            for (int i = 0; i < right_Gather_subIndices[submesh].Count; i++)
            {

                p = right_Gather_subIndices[submesh][i];

                right_Final_vertices.Add(victim_mesh.vertices[p]);
                right_Final_subIndices[submesh].Add(right_Final_vertices.Count - 1);
                right_Final_normals.Add(victim_mesh.normals[p]);
                right_Final_uvs.Add(victim_mesh.uv[p]);

            }

        }
        //잘려서 생성된 오브젝트 전체의 버텍스를 가져옴
        //Vectexs.Get.vertexs = left_Final_vertices;
        //Vectexs.Get.isGet = true;

    }

    static void AddNewTriangles_toFinalArrays()
    {

        for (int submesh = 0; submesh < 2; submesh++)
        {

            int count = left_Final_vertices.Count;
            // add the new ones
            for (int i = 0; i < left_Gather_added_Points[submesh].Count; i++)
            {

                left_Final_vertices.Add(left_Gather_added_Points[submesh][i]);
                left_Final_subIndices[submesh].Add(i + count);
                left_Final_uvs.Add(left_Gather_added_uvs[submesh][i]);
                left_Final_normals.Add(left_Gather_added_normals[submesh][i]);

            }
            //Vectexs.Get.vertexs = left_Final_vertices;
            //Vectexs.Get.isGet = true;
            count = right_Final_vertices.Count;

            for (int i = 0; i < right_Gather_added_Points[submesh].Count; i++)
            {

                right_Final_vertices.Add(right_Gather_added_Points[submesh][i]);
                right_Final_subIndices[submesh].Add(i + count);
                right_Final_uvs.Add(right_Gather_added_uvs[submesh][i]);
                right_Final_normals.Add(right_Gather_added_normals[submesh][i]);

            }
        }

    }

    private static List<Vector3> capVertTracker = new List<Vector3>();
    private static List<Vector3> capVertpolygon = new List<Vector3>();

    static void MakeCaps()
    {

        capVertTracker.Clear();

        for (int i = 0; i < createdVertexPoints.Count; i++)
            if (!capVertTracker.Contains(createdVertexPoints[i]))
            {
                capVertpolygon.Clear();
                capVertpolygon.Add(createdVertexPoints[i]);
                capVertpolygon.Add(createdVertexPoints[i + 1]);

                capVertTracker.Add(createdVertexPoints[i]);
                capVertTracker.Add(createdVertexPoints[i + 1]);


                bool isDone = false;
                while (!isDone)
                {
                    isDone = true;

                    for (int k = 0; k < createdVertexPoints.Count; k += 2)
                    { // go through the pairs

                        if (createdVertexPoints[k] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(createdVertexPoints[k + 1]))
                        { // if so add the other

                            isDone = false;
                            capVertpolygon.Add(createdVertexPoints[k + 1]);
                            capVertTracker.Add(createdVertexPoints[k + 1]);

                        }
                        else if (createdVertexPoints[k + 1] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(createdVertexPoints[k]))
                        {// if so add the other

                            isDone = false;
                            capVertpolygon.Add(createdVertexPoints[k]);
                            capVertTracker.Add(createdVertexPoints[k]);
                        }
                    }
                }

                //Vectexs.Get.vertexs = createdVertexPoints;
                //Vectexs.Get.isGet = true;
                
                FillCap(capVertpolygon);

            }

    }

    static void FillCap(List<Vector3> vertices)
    {

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        Vector3 center = Vector3.zero;
        foreach (Vector3 point in vertices)
            center += point;

        center = center / vertices.Count;


        Vector3 upward = Vector3.zero;
        // 90 degree turn
        upward.x = blade.normal.y;
        upward.y = -blade.normal.x;
        upward.z = blade.normal.z;
        Vector3 left = Vector3.Cross(blade.normal, upward);

        Vector3 displacement = Vector3.zero;
        Vector3 relativePosition = Vector3.zero;

        for (int i = 0; i < vertices.Count; i++)
        {

            displacement = vertices[i] - center;
            relativePosition = Vector3.zero;
            relativePosition.x = 0.5f + Vector3.Dot(displacement, left);
            relativePosition.y = 0.5f + Vector3.Dot(displacement, upward);
            relativePosition.z = 0.5f + Vector3.Dot(displacement, blade.normal);

            uvs.Add(new Vector2(relativePosition.x, relativePosition.y));
            normals.Add(blade.normal);
        }
        //Debug.Log(uvs);

        vertices.Add(center);
        normals.Add(blade.normal);
        uvs.Add(new Vector2(0.5f, 0.5f));

        //Debug.Log(blade.normal);

        Vector3 calculated_normal = Vector3.zero;
        int otherIndex = 0;
        for (int i = 0; i < vertices.Count; i++)
        {

            otherIndex = (i + 1) % (vertices.Count - 1);

            calculated_normal = Vector3.Cross((vertices[otherIndex] - vertices[i]).normalized,
                                              (vertices[vertices.Count - 1] - vertices[i]).normalized);

            if (Vector3.Dot(calculated_normal, blade.normal) < 0)
            {

                triangles.Add(vertices.Count - 1);
                triangles.Add(otherIndex);
                triangles.Add(i);
            }
            else
            {

                triangles.Add(i);
                triangles.Add(otherIndex);
                triangles.Add(vertices.Count - 1);
            }

        }

        int index = 0;
        for (int i = 0; i < triangles.Count; i++)
        {

            index = triangles[i];
            right_Final_vertices.Add(vertices[index]);
            right_Final_subIndices[1].Add(right_Final_vertices.Count - 1);
            right_Final_normals.Add(normals[index]);
            right_Final_uvs.Add(uvs[index]);

        }
        //Debug.Log(right_Final_normals.Count);
        //Vectexs.Get.pointVertex = right_Final_uvs;
        //Vectexs.Get.isGet = true;
        //Vectexs.Get.vertexs = right_Final_vertices;// 자른 단면이 아니라 자른 오브젝트 전체의 버텍스를 가져옴
        //Vectexs.Get.isGet = true;

        for (int i = 0; i < normals.Count; i++)
        {
            normals[i] = -normals[i];
        }

        int temp1, temp2;
        for (int i = 0; i < triangles.Count; i += 3)
        {

            temp1 = triangles[i + 2];
            temp2 = triangles[i];

            triangles[i] = temp1;
            triangles[i + 2] = temp2;
        }

        for (int i = 0; i < triangles.Count; i++)
        {

            index = triangles[i];
            left_Final_vertices.Add(vertices[index]);
            left_Final_subIndices[1].Add(left_Final_vertices.Count - 1);
            left_Final_normals.Add(normals[index]);
            left_Final_uvs.Add(uvs[index]);

        }

    }

}