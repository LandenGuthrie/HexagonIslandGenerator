using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexagonIslandGenerator))]
public class HexgridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HexagonIslandGenerator generator = (HexagonIslandGenerator)target;

        DrawDefaultInspector(); // Draws the default inspector elements

        if (GUILayout.Button("Update Terrain"))
        {
            generator.ClearGrid();
            // Calls the method to regenerate the grid, if it's been added to the script
            generator.CreateGrid();
        }
    }
}
