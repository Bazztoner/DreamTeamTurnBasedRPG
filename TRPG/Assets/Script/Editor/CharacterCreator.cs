using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CharacterCreator : EditorWindow
{
    Texture2D _preview;
    GameObject _object;

    string _charName;
    float _moveTime;
    int _maxHp;
    int _maxMana;
    int _armor;

    bool _modifyTextures;

    Vector2 scrollPos;

    List<Material> _matsToModify = new List<Material>();
    List<Material> _oldMats = new List<Material>();
    List<Color> _colors = new List<Color>();

    [MenuItem("Editor Windows/Character Creator")]
    static void CreateWindow()
    {
        ((CharacterCreator)GetWindow(typeof(CharacterCreator))).Show();
    }

    void OnGUI()
    {
        scrollPos =
           EditorGUILayout.BeginScrollView(scrollPos, false, true);
        GUILayout.Label("Character creator/editor", EditorStyles.boldLabel);

        _object = (GameObject)EditorGUILayout.ObjectField("Selected Object: ", _object, typeof(GameObject), true);
        if (_object)
        {
            if (_object.GetComponent<Character>())
            {
                Repaint();
                if (!_preview)
                {
                    if (GUILayout.Button("Show Preview"))
                    {
                        _preview = Snapshotter.TakePhoto(_object);
                        MakeSnapshot();
                    }
                }
                else
                {
                    if (GUILayout.Button("Refresh Preview"))
                    {
                        _preview = Snapshotter.TakePhoto(_object);
                    }
                    MakeSnapshot();
                }
                MakeCharacterStatsPreview();
            }
            else if (_object.GetComponent<UsableModel>())
            {
                Repaint();
                if (!_preview)
                {
                    if (GUILayout.Button("Show Preview"))
                    {
                        _preview = Snapshotter.TakePhoto(_object);
                        MakeSnapshot();
                    }
                }
                else
                {
                    if (GUILayout.Button("Refresh Preview"))
                    {
                        _preview = Snapshotter.TakePhoto(_object);
                    }
                    MakeSnapshot();
                }
                MakeWannabeCharacterStatsPreview();
                //TextureEditor();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Insert a valid object");
        }
        EditorGUILayout.EndScrollView();
    }

    void MakePreview()
    {
        GUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(300, 300, 300, 300), _preview, ScaleMode.ScaleToFit);
        GUILayout.BeginVertical();
        GUILayout.Label(_object.name);
        GUILayout.Label(AssetDatabase.GetAssetPath(_object));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void MakeSnapshot()
    {
        GUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(300, 300, 300, 300), _preview, ScaleMode.ScaleToFit);
        GUILayout.Label(_object.name);
        GUILayout.Label(AssetDatabase.GetAssetPath(_object));
        GUILayout.EndHorizontal();
    }

    void MakeCharacterStatsPreview()
    {
        var targetCharacter = _object.GetComponent<Character>();
        EditorGUILayout.BeginVertical(GUIStyle.none);

        targetCharacter.stats.hp = EditorGUILayout.IntField("Health", targetCharacter.stats.hp);
        if (targetCharacter.stats.hp < 1) EditorGUILayout.HelpBox("Health can't be zero or less", MessageType.Error);

        targetCharacter.stats.armor = EditorGUILayout.IntField("Armor", targetCharacter.stats.armor);
        targetCharacter.stats.moveTime = EditorGUILayout.FloatField("Time to reach objective", targetCharacter.stats.moveTime);
        if (targetCharacter.stats.moveTime < 1) EditorGUILayout.HelpBox("Move time can't be zero or less", MessageType.Error);

        EditorGUILayout.EndVertical();
    }

    void MakeWannabeCharacterStatsPreview()
    {
        var data = new CharacterData();
        EditorGUILayout.BeginVertical(GUIStyle.none);

        _charName = EditorGUILayout.TextField("Character name", _charName);

        _maxHp = EditorGUILayout.IntField("Health", _maxHp);
        if (_maxHp < 1) EditorGUILayout.HelpBox("Health can't be zero or less", MessageType.Error);

        _maxMana = EditorGUILayout.IntField("Mana", _maxMana);
        if (_maxMana < 1) EditorGUILayout.HelpBox("Mana can't be zero or less", MessageType.Error);

        _armor = EditorGUILayout.IntField("Armor", _armor);
        _moveTime = EditorGUILayout.FloatField("Time to reach objective", _moveTime);
        if (_moveTime <= 0) EditorGUILayout.HelpBox("Move time can't be zero or less", MessageType.Error);

        EditorGUILayout.EndVertical();

        data.maxHp = _maxHp;
        data.hp = _maxHp;
        data.maxMana = _maxMana;
        data.mana = _maxMana;
        data.armor = _armor;
        data.moveTime = _moveTime;

        if (GUILayout.Button("Create Character"))
        {
            var usableModel = _object.GetComponent<UsableModel>();
            if (usableModel)
            {
                PrepareCharacterForCreation(usableModel, data);
                CreateAsset(data);
                //CreateAsset(character);
            }
        }
    }

    void TextureEditor()
    {
        _modifyTextures = EditorGUILayout.BeginToggleGroup("Modify texture", _modifyTextures);

        if (GUILayout.Button("Refresh Textures"))
        {
            var child = _object.GetComponentsInChildren<Transform>().Where(x => x != _object).First();
            var allMats = child.GetComponentsInChildren<MeshRenderer>().Select(x => x.sharedMaterial).ToList();

            _matsToModify = allMats;
            _oldMats = allMats;
            for (int i = 0; i < _matsToModify.Count; i++)
            {
                _colors.Add(new Color());
            }
            Debug.Log(_matsToModify.Count);
        }

        if (_matsToModify.Any())
        {
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < _matsToModify.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                var prw = AssetPreview.GetAssetPreview(_matsToModify[i].mainTexture);
                GUI.Label(GUILayoutUtility.GetRect(10, 25), _matsToModify[i].ToString());
                GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), prw);

                EditorGUILayout.BeginVertical();

                var cr = new Color();

                cr.r = EditorGUILayout.Slider(_colors[i].r, 0, 255);
                cr.g = EditorGUILayout.Slider(_colors[i].g, 0, 255);
                cr.b = EditorGUILayout.Slider(_colors[i].b, 0, 255);

                _colors[i] = cr;

                _matsToModify[i].color = _colors[i];
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            var child = _object.GetComponentsInChildren<Transform>().Where(x => x != _object).First();
            var allMats = child.GetComponentsInChildren<MeshRenderer>().Select(x => x.sharedMaterial).ToList();

            _matsToModify = allMats;
            _oldMats = allMats;
            for (int i = 0; i < _matsToModify.Count; i++)
            {
                _colors.Add(new Color());
            }
            Debug.Log(_matsToModify.Count);

        }

        EditorGUILayout.EndToggleGroup();
    }



    void PrepareCharacterForCreation(UsableModel obj, CharacterData data)
    {
        var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.GetAssetPath(obj.gameObject));
        var newObjPath = MakeNewAssetPath(obj.gameObject, true);

        var prnt = new GameObject();

        var toAdd = GameObject.Instantiate(obj.gameObject);

        toAdd.transform.SetParent(prnt.transform);

        prnt.transform.localScale = new Vector3(.1f, .1f, .1f);
        prnt.AddComponent<Character>();
        var charScript = prnt.GetComponent<Character>();
        charScript.stats = data;
        prnt.AddComponent<Rigidbody>();
        var rb = prnt.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        PrefabUtility.CreatePrefab(newObjPath, prnt);

        DestroyImmediate(prnt, false);
        DestroyImmediate(toAdd, false);
    }

    void CreateAsset(GameObject obj)
    {
        var charPath = "Assets/Resources/Character";
        if (!AssetDatabase.IsValidFolder(charPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Character");
        }

        AssetDatabase.CreateAsset(obj, charPath + _charName + ".asset");
    }

    void CreateAsset(ScriptableObject obj)
    {
        var soPath = "Assets/Resources/ScriptableObjects";
        if (!AssetDatabase.IsValidFolder(soPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "ScriptableObjects");
        }
        AssetDatabase.CreateAsset(obj, soPath + _charName + "Data" + ".asset");
    }

    string MakeNewAssetPath(GameObject obj, bool prefab)
    {
        var charPath = "Assets/Resources/Character/";
        var sufix = prefab ? ".prefab" : ".asset";
        return charPath + _charName + sufix;
    }

    string MakeNewAssetPath(ScriptableObject obj, bool prefab)
    {
        var soPath = "Assets/Resources/ScriptableObjects/";
        var sufix = prefab ? ".prefab" : ".asset";
        return soPath + _charName + "Data" + sufix;

    }

    [System.Obsolete("IDIOTA", true)]
    /// <summary>
    /// Soy un pelotudo.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    GameObject PrepareCharacterForCreation(UsableModel obj, CharacterData data, bool idiota)
    {
        var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.GetAssetPath(obj.gameObject));
        var newObjPath = MakeNewAssetPath(obj.gameObject, true);
        var prnt = new GameObject();
        AssetDatabase.CreateAsset(prnt, newObjPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(newObjPath);
        prnt = (GameObject)AssetDatabase.LoadAssetAtPath(newObjPath, typeof(GameObject));
        prnt.transform.localScale = new Vector3(.1f, .1f, .1f);
        prnt.AddComponent<Character>();
        var charScript = prnt.GetComponent<Character>();
        charScript.stats = data;
        var oldObj = prnt.GetComponent<UsableModel>();
        prnt.AddComponent<Rigidbody>();
        var rb = prnt.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        GameObject cols = new GameObject();

        string[] allPaths = AssetDatabase.FindAssets("CharacterCollider");
        foreach (var item in allPaths)
        {
            var temp = AssetDatabase.GUIDToAssetPath(item);
            cols = (GameObject)AssetDatabase.LoadAssetAtPath(temp, typeof(GameObject));
        }

        var colPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.GetAssetPath(cols));
        AssetDatabase.CreateAsset(cols, colPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(colPath);
        cols = (GameObject)AssetDatabase.LoadAssetAtPath(colPath, typeof(GameObject));
        //var copiedCol = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Character/Collider/" + cols.name + ".asset", typeof(GameObject));

        cols.transform.SetParent(prnt.transform);
        //DestroyImmediate(oldObj, false);
        return prnt;
    }
}
