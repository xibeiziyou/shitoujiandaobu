using UnityEngine;

public class 滑动判断类 : MonoBehaviour
{
    enum 滑动方向
        {
            无方向,  
            向上,   
            向下,  
            向左,  
            向右  
        };

    [SerializeField] private bool 是否支持多次执行 = false;
    [SerializeField] private float 判断的时间间隔 = 0.1f;    
    [SerializeField] private float 滑动的最小距离 = 80f;      

    private Vector2 按下的位置 = Vector2.zero; 
    private Vector2 拖动的位置 = Vector2.zero; 

    private 滑动方向 当前滑动方向 = 滑动方向.无方向; 
    private float 时间计数器;

    private void Start()
    {
    }
    void Update()
    {
        处理触摸输入(); // 每帧处理触摸输入
    }

        
    void 处理触摸输入()
        {
            // 鼠标按下（或触摸开始）
            if (Input.GetMouseButtonDown(0))
            {
                按下的位置 = Input.mousePosition; // 记录起始位置
            }

            // 鼠标按住（或触摸中）
            if (Input.GetMouseButton(0))
            {
                拖动的位置 = Input.mousePosition; // 记录当前鼠标位置
                时间计数器 += Time.deltaTime; // 累加时间

                // 如果超过设定的时间间隔
                if (时间计数器 > 判断的时间间隔)
                {
                    Vector2 滑动方向 = 拖动的位置 - 按下的位置; // 计算滑动方向

                    // 如果滑动距离大于最小滑动距离
                    if (滑动方向.magnitude >= 滑动的最小距离)
                    {
                        判断滑动方向(滑动方向); // 判断滑动方向
                        时间计数器 = 0; // 重置计时器
                        按下的位置 = 拖动的位置; // 更新起始位置
                    }
                }
            }

            // 鼠标松开（或触摸结束）
            if (Input.GetMouseButtonUp(0))
            {
                当前滑动方向 = 滑动方向.无方向; // 重置滑动方向
            }
        }

    // 判断滑动方向    
    void 判断滑动方向(Vector2 滑动方向)
        {
            滑动方向.Normalize(); // 归一化滑动方向向量

            float x = 滑动方向.x; // 获取 x 分量
            float y = 滑动方向.y; // 获取 y 分量

            // 判断滑动方向
            if (Mathf.Abs(x) > Mathf.Abs(y)) // 水平方向滑动
            {
                if (x > 0) // 向右滑动
                {
                    if (!是否支持多次执行 && 当前滑动方向 == 滑动判断类.滑动方向.向右)
                    {
                        return; // 如果不允许多次执行且当前方向已经是右，则直接返回
                    }

                    //Debug.Log("滑动方向.向右"); // 输出日志
                当前滑动方向 = 滑动判断类.滑动方向.向右; // 更新当前方向
                }
                else // 向左滑动
                {
                    if (!是否支持多次执行 && 当前滑动方向 == 滑动判断类.滑动方向.向左)
                    {
                        return; // 如果不允许多次执行且当前方向已经是左，则直接返回
                    }

                    //Debug.Log("滑动方向.向左"); // 输出日志
                当前滑动方向 = 滑动判断类.滑动方向.向左; // 更新当前方向
                }
            }
            else // 垂直方向滑动
            {
                if (y > 0) // 向上滑动
                {
                    if (!是否支持多次执行 && 当前滑动方向 == 滑动判断类.滑动方向.向上)
                    {
                        return; // 如果不允许多次执行且当前方向已经是上，则直接返回
                    }
                //Debug.Log("滑动方向.向上"); // 输出日志
                当前滑动方向 = 滑动判断类.滑动方向.向上; // 更新当前方向
                }
                else // 向下滑动
                {
                    if (!是否支持多次执行 && 当前滑动方向 == 滑动判断类.滑动方向.向下)
                    {
                        return; // 如果不允许多次执行且当前方向已经是下，则直接返回
                    }

                    //Debug.Log("滑动方向.向下"); // 输出日志
                当前滑动方向 = 滑动判断类.滑动方向.向下; // 更新当前方向
                }
            }
        }
}
