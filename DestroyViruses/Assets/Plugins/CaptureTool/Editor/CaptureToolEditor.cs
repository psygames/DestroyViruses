using System;
using UnityEditor;
using UnityEngine;

public class CaptureToolEditor : EditorWindow
{
    #region Filed

    public string outputDirectory = null;

    public string outputFileName = CaptureTool.DefaultOutputFileName;

    public int outputFileNameIndex = 0;

    public string outputFileExtension = ".png";

    public Camera camera = null;

    public TextureFormat textureFormat = TextureFormat.RGBA32;

    public int imageWidth = 0;

    public int imageHeight = 0;

    public float imageScale = 1;

    public bool clearBack = false;

    private Vector2 scrollPosition = Vector2.zero;

    #endregion Field

    #region Method

    [MenuItem("Tools/图像捕获")]
    static void Init()
    {
        EditorWindow.GetWindow<CaptureToolEditor>("Image Capture Tool");
    }

    protected void OnEnable()
    {
        EditorApplication.update += ForceOnGUI;
    }

    protected void OnDisable()
    {
        EditorApplication.update -= ForceOnGUI;
    }

    protected void OnGUI()
    {
        this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition, GUI.skin.box);
        int[] gameViewResolution = GetGameViewResolution();

        // Output directory.

        EditorGUILayout.BeginHorizontal();
        this.outputDirectory = EditorGUILayout.TextField("Output Directory", this.outputDirectory);
        if (GUILayout.Button("Open", GUILayout.Width(100)))
        {
            string tempPath = EditorUtility.SaveFolderPanel("Open", this.outputDirectory, "");

            if (!tempPath.Equals(""))
            {
                this.outputDirectory = EditorGUILayout.TextField(tempPath);
                base.Repaint();
                return;
            }
        }
        EditorGUILayout.EndHorizontal();

        // default output dir
        if (this.outputDirectory == null || this.outputDirectory.Equals(""))
        {
            this.outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            // NOTE:
            // Application.dataPath + "/"; is not so bad.
        }

        // Base setttings.

        this.outputFileName = EditorGUILayout.TextField("Base File Name", this.outputFileName);
        this.outputFileNameIndex = EditorGUILayout.IntField("File Name Index", this.outputFileNameIndex);

        var _extensionTitle = new GUIContent("File Extension", "File Extension (make matching with Texture Format).");
        this.outputFileExtension = EditorGUILayout.TextField(_extensionTitle, this.outputFileExtension);

        var _cameraTitle = new GUIContent("Camera", "Target camera. When 'null', use 'MainCamera' automatically.");
        this.camera = EditorGUILayout.ObjectField(_cameraTitle, this.camera, typeof(Camera), true) as Camera;

        var _textFormatTitle = new GUIContent("Texture Format", "Texture Format (make matching with file extension).");
        this.textureFormat = (TextureFormat)EditorGUILayout.EnumPopup(_textFormatTitle, this.textureFormat);

        var _widthTitle = new GUIContent("Width", "Image width(px).When '0', use GameView width '" + gameViewResolution[0] + "'");
        this.imageWidth = EditorGUILayout.IntSlider(_widthTitle, this.imageWidth, 0, 9999);

        var _heightTitle = new GUIContent("Height", "Image height(px). When '0', use GameView height '" + gameViewResolution[1] + "'");
        this.imageHeight = EditorGUILayout.IntSlider(_heightTitle, this.imageHeight, 0, 9999);

        var _scaleTitle = new GUIContent("Scale", "Image Size scale. Ex: When set '2', the result will twice size of width and height.");
        this.imageScale = EditorGUILayout.Slider(_scaleTitle, this.imageScale, 0.1f, 10);

        var _clearBackTitle = new GUIContent("Clear Background", "Clear the background when capture.");
        this.clearBack = EditorGUILayout.Toggle(_clearBackTitle, this.clearBack);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(2);
        if (GUILayout.Button("Click to Capture"))
        {
            HookAfterImageCaptured(Capture());
        }
        EditorGUILayout.Space();
    }

    protected void ForceOnGUI()
    {
        // NOTE:
        // Need periodic repaint to update Game View.Resolution info.

        if (System.DateTime.Now.Millisecond % 5 == 0)
        {
            Repaint();
        }
    }

    protected int[] GetGameViewResolution()
    {
        // NOTE:
        // Screen.width (& height) shows active window's resorution.
        // So in sometimes, it shows EditorWindow's resolution.

        string[] gameViewResolution = UnityStats.screenRes.Split('x');

        return new int[]
        {
            int.Parse(gameViewResolution[0]),
            int.Parse(gameViewResolution[1])
        };
    }

    protected CaptureTool.CaptureResult Capture()
    {
        Camera _camera = this.camera == null ? Camera.main : this.camera;

        int[] gameViewResolution = GetGameViewResolution();
        int _imageWidth = (int)((this.imageWidth == 0 ? gameViewResolution[0] : this.imageWidth) * this.imageScale);
        int _imageHeight = (int)((this.imageHeight == 0 ? gameViewResolution[1] : this.imageHeight) * this.imageScale);

        CaptureTool.CaptureResult result
        = CaptureTool.Capture(_camera,
                                   textureFormat,
                                   outputFileExtension,
                                   _imageWidth,
                                   _imageHeight,
                                   this.clearBack,
                                   this.outputDirectory,
                                   this.outputFileName
                                 + this.outputFileNameIndex.ToString());

        if (result.success)
        {
            this.ShowNotification(new GUIContent("SUCCESS : " + result.outputPath));
            this.outputFileNameIndex++;
        }
        else
        {
            if (camera == null)
            {
                this.ShowNotification(new GUIContent("ERROR : " + "Camera in null (also MainCamera is null)"));
            }
            else
            {
                this.ShowNotification(new GUIContent("ERROR : " + result.outputPath));
            }
        }

        return result;
    }

    protected virtual void HookAfterImageCaptured(CaptureTool.CaptureResult result)
    {
        // Nothing to do in here. This is used for inheritance.
    }

    #endregion Method
}