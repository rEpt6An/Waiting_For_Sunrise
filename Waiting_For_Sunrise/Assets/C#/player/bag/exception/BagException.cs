using System;

public class BagException : Exception
{
    // 基本构造函数
    public BagException()
    {
    }

    // 带有消息的构造函数
    public BagException(string message)
        : base(message)
    {
    }

}