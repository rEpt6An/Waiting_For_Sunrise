using System;

public class ResourceNotFoundException : Exception
{
    // 基本构造函数
    public ResourceNotFoundException()
    {
    }

    // 带有消息的构造函数
    public ResourceNotFoundException(string message)
        : base(message)
    {
    }

}