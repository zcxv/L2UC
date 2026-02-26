using System;

[Serializable]
public class ServerName {
    public int Id { get; private set; }
    public string Name { get; private set; }

    public ServerName(int id, string name) {
        Id = id;
        Name = name;
    }
}