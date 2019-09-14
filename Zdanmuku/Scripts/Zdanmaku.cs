using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zdanmaku : MonoBehaviour
{
    [HideInInspector]
    public int maxDisplay = 50;                  // 最多 同时显示的条数

    public int step = 200;                       // 整个移动分为多少步 越大越流畅

    public float timeInterval = 0.1f;            // 多久检测一次缓存 性能优化用的 弹幕多设小点 弹幕少设多点

    public int maxPool = 100;                    // 对象池里最大数量 适量 根据内存多少

    public int maxObject = 500;                  // 场景里最多允许存在多少个弹幕物体 fullcontent模式会把不能显示的弹幕存在场景里

    public Mode mode = Mode.FullContent;         // 弹幕模式 realtime时 超出最大显示的弹幕不会显示 fullcontent模式时 会全部显示

    public Direction direction = Direction.Left; // 弹幕方向 位置会自动匹配

    public float rate = 0.006f;                  // 字符串和长度的比率

    [HideInInspector]
    public Font font;                            // 弹幕字体


    public enum Mode { FullContent = 0, Realtime = 1, }

    public enum Direction { Left =0, Right=1, /* Up=2, Down=3*/ }


    class _Danmaku { public int _id; public Text _text; public string _content; public Color _color; public int _size; public float _duration; public float _startOffset; }

    Queue<_Danmaku> dmPool = new Queue<_Danmaku>();


    static Zdanmaku z; static bool isInit;

    int count; int currentDisplay; Canvas c; 

    /// <summary>
    /// 显示一条弹幕
    /// </summary>
    /// <param name="content">字符出啊内容</param>
    /// <param name="color">颜色</param>
    /// <param name="start_offset">距离零点的位置 0～1之间 根据方向自适配</param>
    /// <param name="size">大小</param>
    /// <param name="duration">屏幕上停留的时间（飘过去的总时长）</param>
    public static void Show(string content, Color color , float start_offset = 0, int size = 15, float duration = 1f)
    {
        if (!isInit) { isInit = init(); }

        if (z.font == null)
            throw new System.Exception("需要设置字体！(Font required!)请在(Go to) : Tools/Zdanmaku/Zdanmaku Utility Panel (Font) 中设置弹幕字体");

        if (z.dmPool.Count > z.maxDisplay && z.mode == Mode.Realtime)
            return;
        
        _Danmaku dm = _DanmukuPool.New(z.count, content, color, start_offset, size, duration);

        if (dm != null)
            z.dmPool.Enqueue(dm);
    }

    float tmp_time;

    Vector2 v01 = new Vector2(0, 1);
    Vector2 v10 = new Vector2(1, 0);

    void Update()
    {
        if (Time.time < tmp_time)
            return;

        for (int i = 0; i < dmPool.Count; i++)
        {
            if (currentDisplay >= maxDisplay)
                return;
            
            _Danmaku dm = dmPool.Dequeue();

            dm._text.gameObject.SetActive(true);

            dm._text.rectTransform.sizeDelta = new Vector2(dm._text.rectTransform.sizeDelta.x * dm._content.Length * dm._size * rate, dm._text.rectTransform.sizeDelta.y);

            switch (direction)
            {
                case Direction.Left:
                    {
                        dm._text.rectTransform.anchorMin = Vector2.one;
                        dm._text.rectTransform.anchorMax = Vector2.one;
                        dm._text.rectTransform.pivot = v01 ;
                        dm._text.rectTransform.anchoredPosition = new Vector2(0,-1 * c.GetComponent<RectTransform>().sizeDelta.y * dm._startOffset);
                        dm._text.alignment = TextAnchor.UpperLeft;
                        float length = (c.GetComponent<RectTransform>().sizeDelta.x + dm._text.rectTransform.sizeDelta.x * 2) / step;
                        float time = dm._duration / step;
                        StartCoroutine(Move(dm, Vector2.left * length, time)); 
                        break;
                    }
                case Direction.Right:
                    {
                        dm._text.rectTransform.anchorMin = v01;
                        dm._text.rectTransform.anchorMax = v01;
                        dm._text.rectTransform.pivot = Vector2.one;
                        dm._text.rectTransform.anchoredPosition = new Vector2(0, -1 * c.GetComponent<RectTransform>().sizeDelta.y * dm._startOffset);
                        dm._text.alignment = TextAnchor.MiddleRight;
                        float length = (c.GetComponent<RectTransform>().sizeDelta.x + dm._text.rectTransform.sizeDelta.x * 2) / step;
                        float time = dm._duration / step;
                        StartCoroutine(Move(dm, Vector2.right * length, time));
                        break;
                    }
                //case Direction.Up:
                //    {
                //        dm._text.rectTransform.anchorMin = Vector2.zero;
                //        dm._text.rectTransform.anchorMax = Vector2.zero;
                //        dm._text.rectTransform.pivot = v01;
                //        dm._text.rectTransform.anchoredPosition = new Vector2(c.GetComponent<RectTransform>().sizeDelta.x * dm._startOffset, 0);
                //        dm._text.alignment = TextAnchor.UpperCenter;
                //        float length = (c.GetComponent<RectTransform>().sizeDelta.y + dm._text.rectTransform.sizeDelta.y) / step;
                //        float time = dm._duration / step;
                //        StartCoroutine(Move(dm, Vector2.up * length, time));
                //        break;
                //    }
                //case Direction.Down:
                //    {
                //        dm._text.rectTransform.anchorMin = Vector2.one;
                //        dm._text.rectTransform.anchorMax = Vector2.one;
                //        dm._text.rectTransform.pivot = v10;
                //        dm._text.rectTransform.anchoredPosition = new Vector2(-1 * c.GetComponent<RectTransform>().sizeDelta.x * dm._startOffset, 0);
                //        dm._text.alignment = TextAnchor.LowerCenter;
                //        float length = (c.GetComponent<RectTransform>().sizeDelta.y + dm._text.rectTransform.sizeDelta.y) / step;
                //        float time = dm._duration / step; 
                //        StartCoroutine(Move(dm, Vector2.down * length, time));
                //        break;
                //    }
            }

            currentDisplay++;

        } tmp_time = Time.time + timeInterval; 
    }

    IEnumerator Move(_Danmaku dm, Vector2 delta, float time)
    {
        dm._text.font = font;
        dm._text.text = dm._content;
        dm._text.color = dm._color;
        dm._text.fontSize = dm._size;
        dm._text.rectTransform.sizeDelta *= (dm._size / 14);
        
        for (int i = 0; i < step; i++)
        {
            dm._text.rectTransform.anchoredPosition += delta;
            yield return new WaitForSeconds(time);
        }
        dm._text.rectTransform.sizeDelta /= (dm._size / 14);
        dm._text.rectTransform.sizeDelta = new Vector2(dm._text.rectTransform.sizeDelta.x / dm._content.Length / dm._size / rate, dm._text.rectTransform.sizeDelta.y);

        currentDisplay--;

        _DanmukuPool.Recycle(dm);
    }

    static bool init()
    {
        if (z == null)
            z = FindObjectOfType<Zdanmaku>();

        //if (z == null)
        //    z = new GameObject("Zdanmuku").AddComponent<Zdanmaku>();

        if (z == null)
            throw new System.Exception("需要初始化(Initialize required!)，请打开(Go to) :Tools/Zdanmaku/Zdanmaku Utility Panel初始化");

        z.c = FindObjectOfType<Canvas>();

        //if (z.c == null)
        //    z.c = new GameObject("Canvas").AddComponent<Canvas>();

        if (z.c == null)
            throw new System.Exception("场景里至少需要一个canvas(Canvas required!) ,请点击(Go to) :GameObject/UI/Canvas新建一个canvas物体");

        return true;
    }

    class _DanmukuPool
    {
        static Stack<_Danmaku> p = new Stack<_Danmaku>();

        public static _Danmaku New(int id, string content, Color color, float start_offset, int size, float duration)
        {
            if (p.Count > 0)
            {
                _Danmaku dm = p.Pop();

                dm._id = id; dm._content = content; dm._color = color; dm._startOffset = start_offset; dm._size = size; dm._duration = duration;

                return dm;
            }

            if (z.dmPool.Count >= z.maxObject)
                return null;

            var text = new GameObject(id.ToString()).AddComponent<Text>();

            text.rectTransform.parent = z.c.GetComponent<RectTransform>();

            text.gameObject.SetActive(false);

            z.count++;

            return new _Danmaku { _text = text, _content = content, _color = color, _size = size, _duration = duration, _startOffset = start_offset };
        }

        public static void Recycle(_Danmaku dm)
        {
            if (p.Count < z.maxPool) { dm._text.gameObject.SetActive(false); dm._text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; p.Push(dm); } else { Destroy(dm._text.gameObject); }
        }
    }
}

