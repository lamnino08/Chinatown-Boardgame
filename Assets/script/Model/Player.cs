using Colyseus.Schema;

public class Player : Schema
{
    [Type(0, "string")]
    public string sessionId = default(string);

    [Type(1, "string")] 
    public string name = default(string);

    [Type(2, "boolean")]
    public bool isReady = default(bool);

    [Type(3, "int32")]
    public int color = default(int);

    [Type(4, "array", typeof(ArraySchema<float>), "number")]
    public ArraySchema<float> tile = new ArraySchema<float>();

    [Type(5, "array", typeof(ArraySchema<float>), "number")]
    public ArraySchema<float> store = new ArraySchema<float>();

    [Type(6, "int32")]
    public int money = default(int);
    [Type(7, "int32")]
    public int index = default(int);
}
