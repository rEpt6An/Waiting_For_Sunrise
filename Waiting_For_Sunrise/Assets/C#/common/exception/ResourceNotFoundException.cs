using System;

public class ResourceNotFoundException : Exception
{
    // �������캯��
    public ResourceNotFoundException()
    {
    }

    // ������Ϣ�Ĺ��캯��
    public ResourceNotFoundException(string message)
        : base(message)
    {
    }

}