using UnityEditor;
using UnityEngine;

class FBXPostprocessor : AssetPostprocessor
{
    // This method is called just before importing an FBX.
    void OnPreprocessModel()
    {
        ModelImporter mi = (ModelImporter)assetImporter;
        mi.animationCompression = ModelImporterAnimationCompression.Off;

        // Materials for characters are created using the GenerateMaterials script.
        mi.importMaterials = false;
    }

}