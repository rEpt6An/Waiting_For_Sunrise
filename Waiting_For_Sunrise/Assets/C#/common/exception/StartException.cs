using System;

public class StartException : Exception
{
    // 基本构造函数
    public StartException()
    {
    }

    // 带有消息的构造函数
    public StartException(string message)
        : base(message)
    {
    }

}