
public class 执子控制 : 单例类<执子控制>
{
    public bool 转否 = true;
    public bool 是否联机 = false;

    public enum 执子者
    {
        主方,
        客方
    }

    public 执子者 当前执子者 = 执子者.主方;

    public void 执子者切换() 
    {
        当前执子者 = (执子者)(((int)当前执子者 + 1) % 2);
        if (!是否联机 && 转否) 相机控制.唯一单例.相机旋转();
    }

    public bool 执子判断(int 所执子) 
    {
        return 所执子 == (int)当前执子者;
    }
}
