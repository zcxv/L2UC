using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class RecipeTable {
    public static RecipeTable Instance { get; private set; } = new RecipeTable();
    
    private readonly string dataPath = Path.Combine(Application.streamingAssetsPath, "Data/Meta/Recipes.json");
    private Dictionary<int, RecipeData> recipes;

    public RecipeTable() {
        Initialize();
    }
    
    private void Initialize() {
        using StreamReader file = new StreamReader(dataPath);
        var settings = new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        recipes = JsonConvert.DeserializeObject<List<RecipeData>>(file.ReadToEnd(), settings)
            .GroupBy(e => e.Id)
            .ToDictionary(group => group.Key, group => group.First());
    }

    public RecipeData GetById(int id) {
        recipes.TryGetValue(id, out var recipe);
        return recipe;
    }
    
    public RecipeData this[int id] => GetById(id);
    
}