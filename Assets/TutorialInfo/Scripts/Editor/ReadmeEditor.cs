using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

/// <summary>
/// チュートリアルの表示と関連アセットの削除を管理するクラス
/// </summary>
[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor
{
    static string s_ShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";	 // 自動表示済みかを記録するセッションキー

    static string s_ReadmeSourceDirectory = "Assets/TutorialInfo";				 // チュートリアルのアセット保存先

    const float k_Space = 16f;													 // 表示要素間の余白

    /// <summary> エディター読み込み後のチュートリアル自動選択を登録するメソッド </summary>
    static ReadmeEditor()
    {
        EditorApplication.delayCall += SelectReadmeAutomatically;
    }

    /// <summary> 確認ダイアログで承認されたチュートリアルの関連アセットを削除するメソッド </summary>
    static void RemoveTutorial()
    {
        if (EditorUtility.DisplayDialog("Remove Readme Assets",

            $"All contents under {s_ReadmeSourceDirectory} will be removed, are you sure you want to proceed?",
            "Proceed",
            "Cancel"))
        {
            if (Directory.Exists(s_ReadmeSourceDirectory))
            {
                FileUtil.DeleteFileOrDirectory(s_ReadmeSourceDirectory);
                FileUtil.DeleteFileOrDirectory(s_ReadmeSourceDirectory + ".meta");
            }
            else
            {
                Debug.Log($"Could not find the Readme folder at {s_ReadmeSourceDirectory}");
            }

            var readmeAsset = SelectReadme();																 // 削除対象のチュートリアルアセット
            if (readmeAsset != null)
            {
                var path = AssetDatabase.GetAssetPath(readmeAsset);	 // チュートリアルアセットのパス
                FileUtil.DeleteFileOrDirectory(path + ".meta");
                FileUtil.DeleteFileOrDirectory(path);
            }

            AssetDatabase.Refresh();
        }
    }

    /// <summary> セッションで初めてチュートリアルを選択してレイアウトを読み込むメソッド </summary>
    static void SelectReadmeAutomatically()
    {
        if (!SessionState.GetBool(s_ShowedReadmeSessionStateName, false))
        {
            var readme = SelectReadme();																	 // 表示対象のチュートリアルアセット
            SessionState.SetBool(s_ShowedReadmeSessionStateName, true);

            if (readme && !readme.loadedLayout)
            {
                LoadLayout();
                readme.loadedLayout = true;
            }
        }
    }

    /// <summary> チュートリアル用のエディターレイアウトを読み込むメソッド </summary>
    static void LoadLayout()
    {
        var assembly = typeof(EditorApplication).Assembly;														 // Unityエディターのアセンブリ
        var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);								 // レイアウト読み込みを提供する型
        var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);	 // レイアウト読み込み用のメソッド情報
        method.Invoke(null, new object[] { Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false });
    }

    /// <summary> チュートリアルのアセットを検索して選択するメソッド </summary>
    /// <returns> 一つだけ見つかったReadmeアセット、特定できない場合はnull </returns>
    static Readme SelectReadme()
    {
        var ids = AssetDatabase.FindAssets("Readme t:Readme");													 // 検索で見つかったReadmeアセットのGUID一覧
        if (ids.Length == 1)
        {
            var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));	 // 検索で特定したチュートリアルアセット

            Selection.objects = new UnityEngine.Object[] { readmeObject };

            return (Readme)readmeObject;
        }
        else
        {
            Debug.Log("Couldn't find a readme");
            return null;
        }
    }

    /// <summary> チュートリアルのアイコンとタイトルを描画するメソッド </summary>
    protected override void OnHeaderGUI()
    {
        var readme = (Readme)target;																			 // 表示対象のチュートリアルアセット
        Init();

        var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);							 // 表示するアイコンの幅

        GUILayout.BeginHorizontal("In BigTitle");
        {
            if (readme.icon != null)
            {
                GUILayout.Space(k_Space);
                GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
            }
            GUILayout.Space(k_Space);
            GUILayout.BeginVertical();
            {

                GUILayout.FlexibleSpace();
                GUILayout.Label(readme.title, TitleStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary> チュートリアルの本文とリンクと削除ボタンを描画するメソッド </summary>
    public override void OnInspectorGUI()
    {
        var readme = (Readme)target;																			 // 表示対象のチュートリアルアセット
        Init();

        foreach (var section in readme.sections)																 // undefined
        {
            if (!string.IsNullOrEmpty(section.heading))
            {
                GUILayout.Label(section.heading, HeadingStyle);
            }

            if (!string.IsNullOrEmpty(section.text))
            {
                GUILayout.Label(section.text, BodyStyle);
            }

            if (!string.IsNullOrEmpty(section.linkText))
            {
                if (LinkLabel(new GUIContent(section.linkText)))
                {
                    Application.OpenURL(section.url);
                }
            }

            GUILayout.Space(k_Space);
        }

        if (GUILayout.Button("Remove Readme Assets", ButtonStyle))
        {
            RemoveTutorial();
        }
    }

    bool m_Initialized;															 // 表示スタイルを初期化済みか

    GUIStyle LinkStyle
    {
        get { return m_LinkStyle; }
    }

    [SerializeField]
    GUIStyle m_LinkStyle;														 // リンクの表示スタイル

    GUIStyle TitleStyle
    {
        get { return m_TitleStyle; }
    }

    [SerializeField]
    GUIStyle m_TitleStyle;														 // タイトルの表示スタイル

    GUIStyle HeadingStyle
    {
        get { return m_HeadingStyle; }
    }

    [SerializeField]
    GUIStyle m_HeadingStyle;													 // 見出しの表示スタイル

    GUIStyle BodyStyle
    {
        get { return m_BodyStyle; }
    }

    [SerializeField]
    GUIStyle m_BodyStyle;														 // 本文の表示スタイル

    GUIStyle ButtonStyle
    {
        get { return m_ButtonStyle; }
    }

    [SerializeField]
    GUIStyle m_ButtonStyle;														 // 削除ボタンの表示スタイル

    /// <summary> チュートリアルの表示スタイルを初期化するメソッド </summary>
    void Init()
    {
        if (m_Initialized)
            return;
        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 14;
        m_BodyStyle.richText = true;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontStyle = FontStyle.Bold;
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;

        // 明暗どちらのエディターテーマでも見やすい選択色を使用する
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        m_ButtonStyle = new GUIStyle(EditorStyles.miniButton);
        m_ButtonStyle.fontStyle = FontStyle.Bold;

        m_Initialized = true;
    }

    /// <summary> 下線付きのリンクを描画してクリックを検出するメソッド </summary>
    /// <param name="label"> リンクに表示する内容 </param>
    /// <param name="options"> リンクのレイアウト指定 </param>
    /// <returns> リンクがクリックされた場合はtrue、それ以外はfalse </returns>
    bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        var position = GUILayoutUtility.GetRect(label, LinkStyle, options);										 // リンクを描画する矩形領域

        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, LinkStyle);
    }
}
