

public class 单例类 <T> where T : new()
{
    private static T 单例;

    public static T 唯一单例
    {
        get{
            if (单例 == null)
            {
                return 单例 = new T();
            }
            else
            {
                return 单例;
            }
        }
    }
}
