

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiamondSquareTerrain : MonoBehaviour
{
    public int mDivisions = 6; // number of divisions or squares that make up the terrain
    int numDivisions;

    public float mSize = 5;  // size of the terrain
    public float mHeight = 2; // max height of the terrain
    public float heightDecay = 0.5f;

    public int numCoins;

    public GameObject[] trees;

    public GameObject coin;

    Vector3[] vertices;
    float[] heightMap;
    int mVertCount;

    public Material material;

    [System.Serializable]
    public class SplatHeights
    {
        public int textureIndex;
        public int startingHeight;
    }
    public SplatHeights[] splatHeights;

    void Start()
    {
        numCoins = 0;
        MeshFilter terrain = this.gameObject.AddComponent<MeshFilter>();
        terrain.mesh = this.CreateTerrain();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material = material;

        //add mesh collider
        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;


        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {

                float height = terrainData.GetHeight(x, y);
                float[] splat = new float[splatHeights.Length];

                for (int i = 0; i < splatHeights.Length; i++)
                {
                    if (height >= splatHeights[i].startingHeight)
                        splat[i] = 1;
                }

                for (int j = 0; j < splatHeights.Length; j++)
                {
                    splatmapData[x, y, j] = splat[j];
                }

            }

         }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }



    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

    }

    Mesh CreateTerrain()
    {
        numDivisions = (int)Mathf.Pow(2, mDivisions); 
        mVertCount = (numDivisions + 1) * (numDivisions + 1);

        vertices = new Vector3[mVertCount];
        List<int> triangles = new List<int>();

        //size of each small grid, equals mountain size / number of segments
        float divisionSize = mSize / numDivisions; ;


        Mesh mesh = new Mesh();
        mesh.name = "Mesh";

        // the segments will be 1 larger than the vertices
        int offset = numDivisions + 1;
        for (int i = 0; i <= numDivisions; i++)
        {
            for (int j = 0; j <= numDivisions; j++)
            {
                vertices[i * offset + j] = new Vector3(i * divisionSize, 0.0f, j * divisionSize);

                //stops at the grid edges
                if (i == numDivisions || j == numDivisions)
                {
                    continue;
                }

                int v1 = i * offset + j;
                int v2 = i * offset + j + 1;
                int v3 = (i + 1) * offset + j + 1;
                int v4 = (i + 1) * offset + j;

                triangles.Add(v1);
                triangles.Add(v2);
                triangles.Add(v3);

                triangles.Add(v1);
                triangles.Add(v3);
                triangles.Add(v4);
            }
        }


        heightMap = new float[vertices.Length];
        HeightMap();


        //maping heightmap onto vertices
        int index = 0;
        while (index < vertices.Length)
        {
            vertices[index].y = heightMap[index];

            //tree placement
            if (vertices[index].y > 70 )
            {
                int randomIndex = Random.Range(0, trees.Length);
                //GameObject clone = Instantiate(itemsToPickFrom[randomIndex], transform.position, Quaternion.identity);
                Vector3 treePos = new Vector3(vertices[index].x+Random.Range(-10,10), vertices[index].y - 18 , vertices[index].z + Random.Range(-10, 10));
                Instantiate(trees[randomIndex], treePos, Quaternion.identity);
            }

            //coin placement
            if (vertices[index].y < 30 && numCoins <=5000)
            {
                Vector3 coinPos = new Vector3(vertices[index].x + Random.Range(-10, 10), vertices[index].y + this.transform.position.y, vertices[index].z + Random.Range(-10, 10));
                Instantiate(coin, coinPos, Quaternion.identity);
                numCoins++;
            }
            index++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;


    }
    void HeightMap()
    {


        //number of grids start from 0
        int verticesCount = vertices.Length - 1;

        heightMap[0] = restrictHeight(Random.Range(-mHeight, mHeight)); //topleft
        heightMap[numDivisions] = restrictHeight(Random.Range(-mHeight, mHeight)); //topright
        heightMap[verticesCount] = restrictHeight(Random.Range(-mHeight, mHeight)); //bottomright
        heightMap[verticesCount - numDivisions] = 40; //bottomleft fixed to prevent falling into trees


        int iterations = (int)Mathf.Log(numDivisions, 2);
        int numSquares = 1;
        int squareSize = numDivisions;

        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {

                    DiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;

                }

                row += squareSize;

            }

            numSquares *= 2;
            squareSize /= 2;

            // the value by which the height smoothness changes
            mHeight *= 0.5f;

        }

    }


    void DiamondSquare(int row, int col, int divisionSize, float maxHeight)
    {

        int offset = numDivisions + 1;

        int halfSize = (int)(divisionSize * 0.5f);

        //index for four corners
        int c1 = row * offset + col;
        int c2 = row * offset + col + divisionSize;
        int c3 = (row + divisionSize) * offset + col + divisionSize;
        int c4 = (row + divisionSize) * offset + col;

        //index for the middle (diamond step)
        int middle = (row + halfSize) * offset + col + halfSize;

        int v1 = row * offset + col + halfSize;
        int v2 = middle + halfSize;
        int v3 = (row + divisionSize) * offset + col + halfSize;
        int v4 = middle - halfSize;

        //middle = avg of four corners plus a random value(Diamond Step and Square Step Calculation)
        heightMap[middle] = restrictHeight(getAverage(new int[] { c1, c2, c3, c4 }) + Random.Range(-maxHeight, maxHeight));

        heightMap[v1] = restrictHeight(getAverage(new[] { c1, c2, middle }) + Random.Range(-maxHeight, maxHeight));
        heightMap[v2] = restrictHeight(getAverage(new[] { middle, c2, c3 }) + Random.Range(-maxHeight, maxHeight));
        heightMap[v3] = restrictHeight(getAverage(new[] { middle, c3, c4 }) + Random.Range(-maxHeight, maxHeight));
        heightMap[v4] = restrictHeight(getAverage(new[] { c1, middle, c4 }) + Random.Range(-maxHeight, maxHeight));

    }

    // calculates average
    float getAverage(int[] points)
    {
        float sum = 0.0f;
        foreach (int v in points)
        {
            sum += heightMap[v];
        }
        return sum / points.Length;
    }


    // restrict height to above ground
    float restrictHeight(float h)
    {
        return h < 0 ? 0 : h;
    }


}

