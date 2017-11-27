using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator
{
    [MenuItem("Utilities/Create/Character/Config")]
    public static void CreateCharacterConfig()
    {
        ScriptableObjectUtility.CreateAsset<CharacterData>();
    }
}
