using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class CharSelectWindow : L2Window {
    private VisualTreeAsset _arrowInputTemplate;
    private ArrowInputManipulator _charNameManipulator;
    private List<CharSelectInfoPackage> _characters;

    private Label _levelLabel;
    private Label _classLabel;
    private Label _hpLabel;
    private Label _mpLabel;
    private Label _expLabel;
    private Label _spLabel;
    private Label _karmaLabel;

    private VisualElement _HPBar;
    private VisualElement _HPBarBG;
    private VisualElement _MPBar;
    private VisualElement _MPBarBG;
    private VisualElement _ExpBarBG;
    private VisualElement _ExpBar;
    private CharSelectInfoPackage _selectChar;

    private static CharSelectWindow _instance;
    public static CharSelectWindow Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(this);
        }
    }

    private void OnDestroy() {
        _instance = null;
    }

    private void Update() {
        if (!_isWindowHidden) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                AudioManager.Instance.PlayUISound("click_01");
                ReLoginPressed();
            } else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
                AudioManager.Instance.PlayUISound("click_01");
                StartGamePressed();
            }
        }
    }

    protected override void LoadAssets() {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Login/CharSelectWindow"); 
        _arrowInputTemplate = LoadAsset("Data/UI/_Elements/Template/ArrowInput");
    }

    protected override IEnumerator BuildWindow(VisualElement root) {
        InitWindow(root);

        yield return new WaitForEndOfFrame();

        Button loginButton = (Button) GetElementById("StartButton");
        loginButton.AddManipulator(new ButtonClickSoundManipulator(loginButton));
        loginButton.RegisterCallback<ClickEvent>(evt => StartGamePressed());

        Button deleteButton = (Button)GetElementById("DeleteButton");
        deleteButton.AddManipulator(new ButtonClickSoundManipulator(deleteButton));
        deleteButton.RegisterCallback<ClickEvent>(evt => DeletePressed());

        Button createButton = (Button)GetElementById("CreateButton");
        createButton.AddManipulator(new ButtonClickSoundManipulator(createButton));
        createButton.RegisterCallback<ClickEvent>(evt => CreatePressed());

        Button reloginButton = (Button)GetElementById("ReloginButton");
        reloginButton.AddManipulator(new ButtonClickSoundManipulator(reloginButton));
        reloginButton.RegisterCallback<ClickEvent>(evt => ReLoginPressed());

        VisualElement userNameInputContainer = GetElementById("UserSelectContainer");
        VisualElement userNameInput = _arrowInputTemplate.Instantiate()[0];
        _charNameManipulator = new ArrowInputManipulator(userNameInput, "Name", new string[] { }, -1, (index, value) => {
            SelectorController.Instance.Select(index);
        });

        _levelLabel = (Label)GetElementById("LevelLabel");
        _classLabel = (Label)GetElementById("ClassLabel");
        _hpLabel = (Label)GetElementById("HPText");
        _mpLabel = (Label)GetElementById("MPText");
        _expLabel = (Label)GetElementById("ExpText");
        _spLabel = (Label)GetElementById("SPLabel");
        _karmaLabel = (Label)GetElementById("KarmaLabel");

        _HPBar = GetElementById("HPBar");
        _HPBarBG = GetElementById("HPBarBG");
        _MPBarBG = GetElementById("MPBarBG");
        _MPBar = GetElementById("MPBar");
        _ExpBar = GetElementById("ExpBar");
        _ExpBarBG = GetElementById("ExpBarBG");

        userNameInput.AddManipulator(_charNameManipulator);
        userNameInputContainer.Add(userNameInput);
        SetEmptyData();
    }

    public void SetCharacterList(List<CharSelectInfoPackage> characters)
    {
        _characters = characters;
        string[] charArray = characters.Select(character => character.Name).ToArray();
        _charNameManipulator.UpdateValues(charArray);
    }

    private void SetEmptyData()
    {
        _levelLabel.text = "0";
        _classLabel.text = "";
        _hpLabel.text = "0/0";
        _mpLabel.text = "0/0";
        _expLabel.text = "0.00%";
        _spLabel.text = "0";
        _karmaLabel.text = "0";
        _charNameManipulator.ClearInput();
    }

    public void SelectSlot(int slot) {
        if (slot == _charNameManipulator.Index) {
            return;
        }
        
        if (slot == -1) {
            _levelLabel.text = "0";
            _classLabel.text = "??";
            _hpLabel.text = "0/0";
            _mpLabel.text = "0/0";
            _expLabel.text = "0.00%";
            _spLabel.text = "0";
            _karmaLabel.text = "0";
            _charNameManipulator.ClearInput();
        }
        else if (slot < _characters.Count)
        {
            _selectChar = _characters[slot];
            _levelLabel.text = _characters[slot].Lvl.ToString();
            _classLabel.text = ((CharacterClass)(_characters[slot].ClassId)).ToString();
            _hpLabel.text = $"{_characters[slot].Hp}/{_characters[slot].MaxHp}";
            _mpLabel.text = $"{_characters[slot].Mp}/{_characters[slot].MaxMp}";
            _expLabel.text = _characters[slot].ExpProcent.ToString("0.00") + "%";
            _spLabel.text = _characters[slot].Sp.ToString();
            _karmaLabel.text = _characters[slot].Karma.ToString();

            StartCoroutine(UpdateBars(slot));

            _charNameManipulator.SelectIndex(slot);
        }
    }

    IEnumerator UpdateBars(int slot)
    {
        yield return new WaitForEndOfFrame();
        if (_HPBarBG != null && _HPBar != null)
        {
            float hpRatio = (float)_characters[slot].Hp / (float) _characters[slot].MaxHp;
            float bgWidth = _HPBarBG.resolvedStyle.width;
            float barWidth = bgWidth * hpRatio;
            _HPBar.style.width = barWidth;
        }

        if (_MPBarBG != null && _MPBar != null)
        {
            float mpRatio = (float)_characters[slot].Mp / (float) _characters[slot].MaxMp;
            float bgWidth = _MPBarBG.resolvedStyle.width;
            float barWidth = bgWidth * mpRatio;
            _MPBar.style.width = barWidth;
        }

        if (_ExpBarBG != null && _ExpBar != null)
        {
            float bgWidth = _ExpBarBG.resolvedStyle.width;
            //  float procent = (float)_charactersInterlude[slot].ExpProcent;
            float expRatio = (float)_characters[slot].Exp / (float)_characters[slot].MaxExp();
            float barWidth = bgWidth * expRatio;
            _ExpBar.style.width = barWidth;
        }
    }

    public CharSelectInfoPackage GetSelectChar()
    {
        return _selectChar;
    }
    
    private void StartGamePressed() {
        int slot = SelectorController.Instance.SelectedSlot;
        if (slot == SelectorController.UNSELECTED_SLOT) {
            return;
        }
        
        bool enable = GameClient.Instance.IsCryptEnabled();
        SendGameDataQueue.Instance().AddItem(GameLobbyPacketFactory.CharacterSelect(slot), enable, enable);
    }

    private void ReLoginPressed() {
        GameManager.Instance.OnRelogin();
        GameClient.Instance.Disconnect();
    }

    private void CreatePressed() {
        // GameManager.Instance.OnCreateUser();
        bool enable = GameClient.Instance.IsCryptEnabled();
        SendGameDataQueue.Instance().AddItem(GameLobbyPacketFactory.CreateNewCharacter(), enable, enable);
    }

    private void DeletePressed() {
        int slot = SelectorController.Instance.SelectedSlot;
        if (slot == SelectorController.UNSELECTED_SLOT) {
            return;
        }
        
        bool enable = GameClient.Instance.IsCryptEnabled();
        SendGameDataQueue.Instance().AddItem(GameLobbyPacketFactory.RequestCharacterDelete(slot), enable, enable);
        Debug.Log($"Requesting delete character , slot: {slot}");
    }
}