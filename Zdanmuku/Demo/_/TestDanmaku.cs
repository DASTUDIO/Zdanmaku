using System.Collections;
using UnityEngine;

public class TestDanmaku : MonoBehaviour
{
    void Start() { StartCoroutine(AddDanmaku()); Zdanmaku.Alpha = 0.5f; }

    IEnumerator AddDanmaku()
    {
        for (int i = 0; i < 500; i++)
        {
            yield return new WaitForSeconds(0.5f);
            float f = Random.Range(3, 8);
            Zdanmaku.Show(GetString(Random.Range(0, 6)), GetColor(Random.Range(0, 6)), Random.Range(0f, 0.89f), Random.Range(30, 70), f);
        }
    }


    bool isPause;

    private void OnGUI()
    {
        if (!isPause)
            if (GUI.Button(new Rect(0, 0, 200, 100), "Pause"))
            { Zdanmaku.Pause(); isPause = true; }

        if (isPause)
            if (GUI.Button(new Rect(0, 0, 200, 100), "Continue"))
            { Zdanmaku.Continue(); isPause = false; }
    }

    string GetString(int i)
    {
        switch (i)
        {
            case 0:
                return "你好吗？好好学习天天向上啦啦啦啦啦啦嘿嘿嘿嘻嘻巴扎嘿！呢呢呢，哈哈，这是一条很长的弹幕";

            case 1:
                return "欢迎来到Zdanmuku";

            case 2:
                return "配合Zarch插件使用更快捷哦";

            case 3:
                return "一句话能解决的事情就用一句话解决";

            case 4:
                return "迷之感动";

            case 5:
                return "长大后发现，那就是爱情";

            default:
                return "大人分不清欲望和感情";
        }
    }

    Color GetColor(int i)
    {
        switch (i)
        {
            case 0:
                return Color.yellow;

            case 1:
                return Color.white;
                
            case 2:
                return Color.white;

            case 3:
                return Color.red;

            case 4:
                return Color.yellow;

            case 5:
                return Color.green;
                
            default:
                return Color.magenta;
        }
    }
}
