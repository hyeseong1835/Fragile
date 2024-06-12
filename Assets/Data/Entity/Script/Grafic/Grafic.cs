using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEditor;
using UnityEditor.Experimental;

public enum AnimationType
{
    Move, Attack, Special
}
public enum MoveAnimationType
{
    Stay, Walk, Jump, Dash, Charge
}
public enum SkillAnimationType
{
    Swing, Shoot, Throw, Cast, Summon
}
public enum ReadLineType
{
    Custom, One, Rotation4X
}
public class Grafic : MonoBehaviour
{
    [MenuItem("GameObject/Create Asset/Grafic", false, 0)]
    static void Create(MenuCommand menuCommand)
    {
        GameObject parentGameObject = (GameObject)menuCommand.context;
        Grafic grafic = Instantiate(EditorResources.Load<GameObject>("Preset/Grafic.prefab"), parentGameObject.transform).GetComponent<Grafic>();
        grafic.gameObject.name = "New Grafic";
        grafic.con = parentGameObject.GetComponent<Controller>();
        grafic.renderer = grafic.GetComponent<SpriteRenderer>();
    }

    [HorizontalGroup("Data")]
    #region Horizontal Data

        [LabelWidth(Editor.propertyLabelWidth)]
        public GraficData data;

        public bool DataIsNull { get => data == null; }
                                                                                [HorizontalGroup("Data", width: Editor.shortButtonWidth)]
        [EnableIf(nameof(DataIsNull))]
        [Button]
        void CreateData()
        {
            string folderPath = $"{con.ControllerData.FolderPath}/{gameObject.name}";

            if (Directory.Exists(folderPath) == false)
            {
                AssetDatabase.CreateFolder(con.ControllerData.FolderPath, gameObject.name);
            }

            string dataPath = $"{folderPath}/GraficData.asset";

        if (Directory.Exists(dataPath))
        {
            data = AssetDatabase.LoadAssetAtPath<GraficData>(dataPath);
        }
        else
        {
            data = ScriptableObject.CreateInstance<GraficData>();
            AssetDatabase.CreateAsset(data, dataPath);
        }

        if (Directory.Exists($"{folderPath}/Animation") == false)
        {
            AssetDatabase.CreateFolder(folderPath, "Animation");
        }
    }

    #endregion

    [Required]
    [LabelWidth(Editor.propertyLabelWidth)] 
    public Controller con;

    [ReadOnly]
    [LabelWidth(Editor.propertyLabelWidth)] 
    new public SpriteRenderer renderer;

    [Required]
    [LabelWidth(Editor.propertyLabelWidth)]
    public AnimationData curAnimation;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int lineOffset = 0;
    
    [LabelWidth(Editor.propertyLabelWidth)]
    public float time = 0;

    void Awake()
    {
        if (data.sprites == null)
        {
            data.sprites = new Sprite[data.spriteSheet.width / data.spritePixelWidth, data.spriteSheet.height / data.spritePixelHeight];
            for (int x = 0; x < data.sprites.GetLength(0); x++)
            {
                for (int y = 0; y < data.sprites.GetLength(1); y++)
                {
                    data.sprites[x, y] = data.spriteSheet.GetSprite(x, y, data.spritePixelWidth, data.spritePixelHeight);
                }
            }
        }
    }
    void Update()
    {
        if (curAnimation.length * (1 - time) > Time.deltaTime)
        {
            time += Time.deltaTime / curAnimation.length;
            if (time >= 1) time--;
        }
        else time = 0;

        switch (curAnimation.readLineType)
        { 
            case ReadLineType.Custom:
                break;
            case ReadLineType.One:
                lineOffset = 0;
                break;
            case ReadLineType.Rotation4X:
                lineOffset = con.moveRotate4;
                break;
        }
    }
    public void Set(int x, int y)
    {
        renderer.sprite = data.sprites[x, y];
    }
    public int GetIndex(int count) => Mathf.FloorToInt(time * count);
}