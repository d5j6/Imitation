using UnityEngine;
using System.Collections;
using System;

//This class takes a texture of a google earth terrain and converts it into a terrain where the roads (and other dark objects unfortunately) are raised by 10 units
//We need this code in the game to implement a "procedural" city generation system that isn't just a bunch of blocks.
//The input is a top-down view picture of a city or road system from google earth
//The output is a modified terrain that has dark areas raised 
//This code will potentially be used in the main gameplay scene to generate the road system for the procedural city. Maybe.
/*
public class Terrain : MonoBehaviour
{
    Terrain terr;
    int hmWidth;
    int hmHeight;
    int posX = 0;
    int posY = 0;
    public Texture2D tex;

    int desiredHeight = 10;

    void Start()
    {
        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapWidth;
        hmHeight = terr.terrainData.heightmapHeight;
   
        List<int> TexposX = new List<int>();
        List<int> TexposY = new List<int>();


        //loops through all the pixels of the texture
        for (int i = 0; i < hmWidth; i++)
            for (int j = 0; j < hmHeight; j++)
            {
                //finds the pixels that are dark enough to be considered roads(in this case darker than pure gray)
                if (tex.GetPixel(i, j).grayscale < .5){
                    //adds the position of the selected pixel to a list (in separate parts) (soon to be fixed)
                    TexposX.Add(i);
                    TexposY.Add(j);
                }
            }

        // get the heights of the terrain at these positions
        float[,] heights = terr.terrainData.GetHeights(TexposX, TexposY, 512, 512);
        // set each sample of the terrain in the size to the desired height
        for (int i = 0; i < hmWidth; i++)
            for (int j = 0; j < hmHeight; j++)
                heights[i, j] = desiredHeight;//sets the heightpoints all to an even height of 10

        // set the new heights to the terrain
        terr.terrainData.SetHeights(posX, posY, heights);



    }
}

// Question 4: I chose to use a List to capture the number of texture coordinates because I needed a dynamic structure that could grow, and you can't use Vectors in C#.
//There is no way to know how much of the terrain will be covered in roads, so any static data structure would not work.

// Question 5: The biggest challenge of this code was figuring out how to accurately distinguish roads on a google earth texture from the rest of the terrain.
//This was accomplished by using a greyscale function to find dark parts of the texture. This is still too rough to use as a final version, as it picks up other things such as dark trees 
//and whatnot, but it is a step in the right direction. 

// Question 6: So originally, I wanted to do an L-systems type generation of roads, but after weeks of getting nowhere I figured it was time to settle for using google earth images as heightmaps 
//to create the roads. I had no idea where to even start until I was introduced to using texture coordinates to build levels in a different class. This was perfect for this application and Unity
//already had internal libraries to support it. I originally wanted to be able to completely render a 3D google earth terrain with buildings and all, but as of now, this only works in 90 dollar
//asset packages or blender only. 

// Question 7: Sources:
//http://docs.unity3d.com/ScriptReference/TerrainData.SetHeights.html
//http://stackoverflow.com/questions/594853/dynamic-array-in-c-sharp
//http://answers.unity3d.com/questions/16913/reading-a-color-from-an-image.html
//
*/