using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface ICreatorTables
{
    public void InitTable(VisualElement content , VisualElement root);
    void LoadAsset(Func<string, VisualTreeAsset> loaderFunc);
    void CreateTable(List<TableColumn> headersName);
}
