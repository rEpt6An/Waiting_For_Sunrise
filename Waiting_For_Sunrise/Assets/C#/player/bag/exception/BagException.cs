using System;

public class BagException : Exception
{
    // �������캯��
    public BagException()
    {
    }

    // ������Ϣ�Ĺ��캯��
    public BagException(string message)
        : base(message)
    {
    }

}