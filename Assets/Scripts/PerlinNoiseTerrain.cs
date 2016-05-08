using UnityEngine;
using System.Collections;
using System.Linq;

public class PerlinNoiseTerrain : MonoBehaviour
{
    public Terrain terrain;
    private TerrainData terrainData;
    private int heightmapWidth;
    private int heightmapHeight;

    public float sampleOneOctave = 2.0f;
    public float sampleTwoOctave = 5.0f;
    public float sampleThreeOctave = 10.0f;
    public float sampleFourOctave = 1f;

    public Vector2 sampleOneOffset = Vector2.zero;
    public Vector2 sampleTwoOffset = Vector2.zero;
    public Vector2 sampleThreeOffset = Vector2.zero;
    public Vector2 sampleFourOffset = Vector2.zero;
    void Start()
    {
        if (!terrain)
        {
            terrain = Terrain.activeTerrain;
        }

        terrainData = terrain.terrainData;

        heightmapWidth = terrain.terrainData.heightmapWidth;
        heightmapHeight = terrain.terrainData.heightmapHeight;

        GeneratePerlinTerrain();
        PaintTerrain();
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
       // {
            
       // }

    }

    void GeneratePerlinTerrain()
    {
        float[,] heightmapData = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        for (int y = 0; y < heightmapHeight; y++)
        {
            for (int x = 0; x < heightmapWidth; x++)
            {
                Vector2 perlinSampleOne = new Vector2(((sampleOneOctave / (float)(heightmapWidth)) * (float)(x)) + sampleOneOffset.x, ((sampleOneOctave / (float)(heightmapHeight)) * (float)(y)) + sampleOneOffset.y);
                float perlinHeightOne = Mathf.PerlinNoise(perlinSampleOne.x, perlinSampleOne.y);

                Vector2 perlinSampleTwo = new Vector2(((sampleTwoOctave / (float)(heightmapWidth)) * (float)(x)) + sampleTwoOffset.x, ((sampleTwoOctave / (float)(heightmapHeight)) * (float)(y)) + sampleTwoOffset.y);
                float perlinHeightTwo = Mathf.PerlinNoise(perlinSampleTwo.x, perlinSampleTwo.y);

                Vector2 perlinSampleThree = new Vector2(((sampleThreeOctave / (float)(heightmapWidth)) * (float)(x)) + sampleThreeOffset.x, ((sampleThreeOctave / (float)(heightmapHeight)) * (float)(y)) + sampleThreeOffset.y);
                float perlinHeightThree = Mathf.PerlinNoise(perlinSampleThree.x, perlinSampleThree.y);

                Vector2 perlinSampleFour = new Vector2(((sampleFourOctave / (float)(heightmapWidth)) * (float)(x)) + sampleFourOffset.x, ((sampleFourOctave / (float)(heightmapHeight)) * (float)(y)) + sampleFourOffset.y);
                float perlinHeightFour = Mathf.PerlinNoise(perlinSampleFour.x, perlinSampleFour.y);
         
                heightmapData[y, x] = (float)(perlinHeightOne / 4 + ((perlinHeightTwo + perlinHeightThree)/130 + (perlinHeightFour)/20));
            }
        }

        terrainData.SetHeights(0, 0, heightmapData);
    }


    void PaintTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = terrainData.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                //constant influence
                splatWeights[0] = 0.5f;

                //stronger at lower altitudes
                splatWeights[1] = 0;// Mathf.Clamp01((terrainData.heightmapHeight/10 - height*0.7f));

                // stronger on flatter terrain
                splatWeights[2] = 1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 5.0f));


                // Texture[3] increases with height but only on surfaces facing positive Z axis 
                /*int cutoff = 50;
                if (height < cutoff || height > 1000)
                    splatWeights[1] = 0;
                else
                    splatWeights[1] = ((height - cutoff)*(height - cutoff) / 10); //* Mathf.Clamp01(normal.z);
                */
        float z = splatWeights.Sum();
                
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    
                    splatWeights[i] /= z;
                    
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }
        
        terrainData.SetAlphamaps(0, 0, splatmapData);

    }
}