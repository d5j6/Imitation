// April 6th 2013
// TwoSamplePerlinTerrain.js by Jay Kay (Alucard Jay)
 #pragma strict
 
public var terrain : Terrain;
private var terrainData : TerrainData;
private var heightmapWidth : int;
private var heightmapHeight : int;
 
public var sampleOneOctave : float = 2.0;
public var sampleTwoOctave : float = 5.0;
 
public var sampleOneOffset : Vector2 = Vector2.zero;
public var sampleTwoOffset : Vector2 = Vector2.zero;
 
function Start() 
{
    if ( !terrain )
    {
        terrain = Terrain.activeTerrain;
    }
     
    terrainData = terrain.terrainData;
     
    heightmapWidth = terrain.terrainData.heightmapWidth;
    heightmapHeight = terrain.terrainData.heightmapHeight;
}
 
function Update() 
{
    if ( Input.GetMouseButtonDown(0) )
    {
        GeneratePerlinTerrain();
    }
}
 
function GeneratePerlinTerrain() 
{
    var heightmapData : float[,] = terrainData.GetHeights( 0, 0, heightmapWidth, heightmapHeight );
     
    for ( var y : int = 0; y < heightmapHeight; y ++ )
    {
        for ( var x : int = 0; x < heightmapWidth; x ++ )
    {
            var perlinSampleOne : Vector2 = new Vector2( ( ( sampleOneOctave / parseFloat( heightmapWidth ) ) * parseFloat( x ) ) + sampleOneOffset.x, ( ( sampleOneOctave / parseFloat( heightmapHeight ) ) * parseFloat( y ) ) + sampleOneOffset.y );
            var perlinHeightOne : float = Mathf.PerlinNoise( perlinSampleOne.x, perlinSampleOne.y );
             
            var perlinSampleTwo : Vector2 = new Vector2( ( ( sampleTwoOctave / parseFloat( heightmapWidth ) ) * parseFloat( x ) ) + sampleTwoOffset.x, ( ( sampleTwoOctave / parseFloat( heightmapHeight ) ) * parseFloat( y ) ) + sampleTwoOffset.y );
            var perlinHeightTwo : float = Mathf.PerlinNoise( perlinSampleTwo.x, perlinSampleTwo.y );
             
            heightmapData[y,x] = ( perlinHeightOne + perlinHeightTwo ) * 0.1;
    }
}
     
terrainData.SetHeights( 0, 0, heightmapData );
}