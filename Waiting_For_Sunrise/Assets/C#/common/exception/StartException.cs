using System;

public class StartException : Exception
{
    // �������캯��
    public StartException()
    {
    }

    // ������Ϣ�Ĺ��캯��
    public StartException(string message)
        : base(message)
    {
    }

}