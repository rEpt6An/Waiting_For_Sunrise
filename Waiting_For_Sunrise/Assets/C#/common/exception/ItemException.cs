using System;

public class ItemException : Exception
{
    // �������캯��
    public ItemException()
    {
    }

    // ������Ϣ�Ĺ��캯��
    public ItemException(string message)
        : base(message)
    {
    }

}