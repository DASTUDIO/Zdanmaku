using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ZdanmakuEditor : Editor
{
    [MenuItem("Tools/Zdanmaku/Zdanmaku Utility Panel")]
    public static void ShowZdanmakuWindow() { EditorWindow.GetWindow<ZdanmakuPanel>(); }
}

public class ZdanmakuPanel : EditorWindow
{
    Zdanmaku handler;

    public ZdanmakuPanel() { this.titleContent = new GUIContent("Zdanmaku"); }

    void OnGUI()
    {

        GUILayout.Label("弹幕样式(Danmaku Style)");

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();


        if (handler == null)
            handler = FindObjectOfType<Zdanmaku>();


        if (handler == null)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("init")) { init(); }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            this.ShowNotification(new GUIContent("需要初始化才能使用，点击init按钮\n(Click init button)"));
            return;
        }


        GUILayout.Label(new GUIContent("弹幕字体(Font)"));

        handler.font = (Font)EditorGUILayout.ObjectField(handler.font, typeof(Font), false);

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        handler.direction = (Zdanmaku.Direction)EditorGUILayout.EnumPopup("弹幕方向(Direction)",handler.direction);

        GUILayout.EndHorizontal();


        GUILayout.EndVertical();

        GUILayout.Label("弹幕设置(Danmaku Setting)");


        GUILayout.BeginVertical("box");

        handler.mode = (Zdanmaku.Mode)EditorGUILayout.EnumPopup("弹幕模式(Mode)", handler.mode);

        handler.step = (int)EditorGUILayout.Slider("流畅度(Smooth)",handler.step, 32, 2048);

        handler.maxDisplay = (int)EditorGUILayout.Slider("同时显示(Concurrent Display)", handler.maxDisplay, 2, 512);

        handler.rate = (float)EditorGUILayout.Slider("消逝偏移(Disappear Offset)", handler.rate, 0.004f, 0.007f);

        GUILayout.EndVertical();


        GUILayout.Label("性能");

        GUILayout.BeginVertical("box");

        handler.maxPool = (int)EditorGUILayout.Slider("对象池容量(Objs Pool Size)", handler.maxPool, 2, 512);

        handler.maxObject = (int)EditorGUILayout.Slider("缓存(Cache)", handler.maxObject, 64, 512);

        handler.timeInterval = (float)EditorGUILayout.Slider("缓存检测间隔(Read Cache Sleep)", handler.timeInterval, 0.05f, 4f);

        this.ShowNotification(new GUIContent("显示弹幕(Display a Danmaku):\nZdanmaku.Show()\n\n参数为(Parameters):\n弹幕内容,颜色,偏移,大小,持续时间\nContent,Color,Offset,Size,Duration"));

        GUILayout.EndVertical();

    }

    void init()
    {
        if (FindObjectOfType<Zdanmaku>())
            return;
        new GameObject("Zdanmaku").AddComponent<Zdanmaku>();
    }

}
