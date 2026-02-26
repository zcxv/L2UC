using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class ServerNameTable {
    
    public static ServerNameTable Instance { get; private set; } = new ServerNameTable();
    
    private readonly string dataPath = Path.Combine(Application.streamingAssetsPath, "Data/Meta/ServerNames.json");
    private Dictionary<int, ServerName> serverNames;

    private ServerNameTable() {
        Initialize();
    }

    private void Initialize() {
        using StreamReader file = new StreamReader(dataPath);
        serverNames = JsonConvert.DeserializeObject<List<ServerName>>(file.ReadToEnd())
            .GroupBy(e => e.Id)
            .ToDictionary(group => group.Key, group => group.First());
    }

    public ServerName GetById(int id) {
        return serverNames[id];
    }

    public ServerName this[int id] => serverNames[id];
}