
public record RecipeData(
    int Id,
    int Level,
    int RecipeId,
    string Name,
    int SuccessRate,
    int MpCost,
    bool IsCommon,
    ItemCountEntry[] Materials,
    ItemCountEntry Product
);