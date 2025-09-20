using System;

public class ItemException : Exception
{
    // 基本构造函数
    public ItemException()
    {
    }

    // 带有消息的构造函数
    public ItemException(string message)
        : base(message)
    {
    }

}