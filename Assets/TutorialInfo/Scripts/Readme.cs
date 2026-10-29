using System;
using UnityEngine;

/// <summary>
/// チュートリアルの表示内容を保持するクラス
/// </summary>
public class Readme : ScriptableObject
{
    public Texture2D icon;		 // チュートリアルのアイコン
    public string title;		 // チュートリアルのタイトル
    public Section[] sections;	 // チュートリアルの各節の表示内容
    public bool loadedLayout;	 // 専用レイアウトを読み込み済みか

    /// <summary>
    /// チュートリアルの各節の内容を保持するクラス
    /// </summary>
    [Serializable]
    public class Section
    {
        public string heading, text, linkText, url;	 // 見出し、本文、リンクの表示文字列、リンク先URL
    }
}
