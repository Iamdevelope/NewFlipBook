
using UnityEngine;

public class MessageData
{

    public string Type;
    public Color Color = Color.black;
    public string Message;
    public override string ToString()
    {
        return Message;
    }
}
