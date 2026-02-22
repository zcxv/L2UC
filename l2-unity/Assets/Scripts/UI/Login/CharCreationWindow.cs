using System.Collections;
using System.Collections.Generic;


using UnityEngine;

using UnityEngine.UIElements;

public class CharCreationWindow : L2Window {
    private VisualTreeAsset _arrowInputTemplate;
    private ArrowInputManipulator hairStyleManipulator;
    private ArrowInputManipulator hairColorManipulator;
    private ArrowInputManipulator faceManipulator;
    private ArrowInputManipulator genderManipulator;
    private ArrowInputManipulator classManipulator;
    private ArrowInputManipulator raceManipulator;
    private TextField userInputField;
    private VisualElement pawnRotateWindow;
    private Label errorLabel;
    private Button pawnZoominButton;
    private List<PlayerTemplates> _playerTemplates;
    private bool isInit = false;
    private static CharCreationWindow _instance;
    public static CharCreationWindow Instance { get { return _instance; } }

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

    protected override void LoadAssets() {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Login/CharCreationWindow");
        _arrowInputTemplate = LoadAsset("Data/UI/_Elements/Template/ArrowInput");
    }

    public void SetPlayerTemplates(List<PlayerTemplates> playerTemplates)
    {
        if(_playerTemplates != null)
        {
            if (_playerTemplates.Count > 0) _playerTemplates.Clear();
        }
        
        _playerTemplates = playerTemplates;
    }
    protected override IEnumerator BuildWindow(VisualElement root) {
        InitWindow(root);

        yield return new WaitForEndOfFrame();

        userInputField = (TextField) GetElementById("UserInputField");
        userInputField.AddManipulator(new BlinkingCursorManipulator(userInputField));

       

        Button createButton = (Button) GetElementById("CreateButton");
        createButton.AddManipulator(new ButtonClickSoundManipulator(createButton));
        createButton.RegisterCallback<ClickEvent>(evt => CreateButtonPressed());

        Button previousButton = (Button)GetElementById("PreviousButton");
        previousButton.AddManipulator(new ButtonClickSoundManipulator(previousButton));
        previousButton.RegisterCallback<ClickEvent>(evt => PreviousButtonPressed());

        VisualElement charDetailWindow = GetElementById("CharCreationDetailWindow");

        pawnRotateWindow = GetElementById("CharacterRotateWindow");
        HideRotatePawnWindow();
        errorLabel = (Label)GetElementById("LabelError");

        Button pawnRotateLeftButton = pawnRotateWindow.Q<Button>("TurnLeftButton");
        Button pawnRotateRightButton = pawnRotateWindow.Q<Button>("TurnRightButton");
        pawnZoominButton = pawnRotateWindow.Q<Button>("ZoomButton");

        pawnRotateLeftButton.AddManipulator(new ButtonClickSoundManipulator(pawnRotateLeftButton));
        pawnRotateRightButton.AddManipulator(new ButtonClickSoundManipulator(pawnRotateRightButton));
        pawnZoominButton.AddManipulator(new ButtonClickSoundManipulator(pawnZoominButton));

        pawnRotateLeftButton.RegisterCallback<PointerDownEvent>((evt) => {
            CreatorController.Instance.Rotate(Vector3.up);
        }, TrickleDown.TrickleDown);

        pawnRotateRightButton.RegisterCallback<PointerDownEvent>((evt) => {
            CreatorController.Instance.Rotate(Vector3.down);
        }, TrickleDown.TrickleDown);

        pawnRotateLeftButton.RegisterCallback<PointerUpEvent>((evt) => {
            CreatorController.Instance.Rotate(Vector3.zero);
        });

        pawnRotateRightButton.RegisterCallback<PointerUpEvent>((evt) => {
            CreatorController.Instance.Rotate(Vector3.zero);
        });

        pawnZoominButton.RegisterCallback<ClickEvent>((evt) => {
            ToggleZoomin(false);
        });

        errorLabel.text = string.Empty;
        VisualElement raceInput = _arrowInputTemplate.Instantiate()[0];
        VisualElement classInput = _arrowInputTemplate.Instantiate()[0];
        VisualElement genderInput = _arrowInputTemplate.Instantiate()[0];
        VisualElement hairstyleInput = _arrowInputTemplate.Instantiate()[0];
        VisualElement hairColorInput = _arrowInputTemplate.Instantiate()[0];
        VisualElement faceInput = _arrowInputTemplate.Instantiate()[0];


        hairStyleManipulator = new ArrowInputManipulator(hairstyleInput, "Hairstyle", new string[] { "Type A", "Type B", "Type C", "Type D", "Type E" }, -1, (index, value) => {
            if (!CreatorController.Instance.IsSelected) {
                hairStyleManipulator.ClearInput();
                return;
            }
            
            CreatorController.Instance.SetAppearance(faceManipulator.Index, hairColorManipulator.Index, index);
        });
        hairstyleInput.AddManipulator(hairStyleManipulator);

        hairColorManipulator = new ArrowInputManipulator(hairColorInput, "Hair Color", new string[] { "Type A", "Type B", "Type C", "Type D" }, -1, (index, value) => {
            if (!CreatorController.Instance.IsSelected) {
                hairColorManipulator.ClearInput();
                return;
            }
            
            CreatorController.Instance.SetAppearance(faceManipulator.Index, index, hairStyleManipulator.Index);
        });
        hairColorInput.AddManipulator(hairColorManipulator);

        faceManipulator = new ArrowInputManipulator(faceInput, "Face", new string[] { "Type A", "Type B", "Type C" }, -1, (index, value) => {
            if (!CreatorController.Instance.IsSelected) {
                faceManipulator.ClearInput();
                return;
            }
            
            CreatorController.Instance.SetAppearance(index, hairColorManipulator.Index, hairStyleManipulator.Index);
        });
        faceInput.AddManipulator(faceManipulator);

        genderManipulator = new ArrowInputManipulator(genderInput, "Gender", new string[] { "Male", "Female" }, -1, (index, value) => {
            if (classManipulator.Value == "") {
                genderManipulator.ClearInput();
                return;
            }

            Camera cam = LoginCameraManager.Instance.SelectGenderCamera(raceManipulator.Value, classManipulator.Value, value);
            if (cam != null) {
                LoginCameraManager.Instance.SwitchCamera(cam);
            }

            hairStyleManipulator.ResetInput();
            hairColorManipulator.ResetInput();
            faceManipulator.ResetInput();

            ShowRotatePawnWindow();
            CreatorController.Instance.Select(raceManipulator.Index, classManipulator.Index, index);
            
        });
        genderInput.AddManipulator(genderManipulator);

        classManipulator = new ArrowInputManipulator(classInput, "Class", new string[] { "Fighter", "Mystic" }, -1, (index, value) => {
            if(raceManipulator.Value == "Dwarf" && value == "Mystic") {
                classManipulator.ResetInput();
                return;
            }

            if (raceManipulator.Value == "") {
                classManipulator.ClearInput();
                return;
            }

            Camera cam = LoginCameraManager.Instance.SelectClassCamera(raceManipulator.Value, value);
            if (cam != null) {
                LoginCameraManager.Instance.SwitchCamera(cam);
            }

            genderManipulator.ClearInput();
            hairStyleManipulator.ClearInput();
            hairColorManipulator.ClearInput();
            faceManipulator.ClearInput();

            HideRotatePawnWindow();
            CreatorController.Instance.ResetSelection();
        });
        classInput.AddManipulator(classManipulator);

        raceManipulator = new ArrowInputManipulator(raceInput, "Race", new string[] { "Human", "Elf", "Dark Elf", "Orc", "Dwarf" }, -1, (index, value) => {
            LoginCameraManager.Instance.SwitchCamera(value);
            classManipulator.ClearInput();
            genderManipulator.ClearInput();
            hairStyleManipulator.ClearInput();
            hairColorManipulator.ClearInput();
            faceManipulator.ClearInput();

            HideRotatePawnWindow();
            CreatorController.Instance.ResetSelection();
 
        });
        raceInput.AddManipulator(raceManipulator);

        raceManipulator.ClearInput();

        charDetailWindow.Add(raceInput);
        charDetailWindow.Add(classInput);
        charDetailWindow.Add(genderInput);
        charDetailWindow.Add(hairstyleInput);
        charDetailWindow.Add(hairColorInput);
        charDetailWindow.Add(faceInput);
    }

    public void Init()
    {
        if (!isInit)
        {
            isInit = true;
            CreatorController.Instance.Init();
        }
    }

    public void SetErrorLabel(string error) {
        errorLabel.text = error;
    }
    
    private void CreateButtonPressed()
    {
        Debug.Log("event create button");

        string classType = classManipulator.Value;
        int sex = genderManipulator.Index;
        int hairStyle = hairStyleManipulator.Index;
        int hairColor = hairColorManipulator.Index;
        int face = faceManipulator.Index;
        string race = raceManipulator.Value;
        string name = userInputField.value;

        var packet = GameLobbyPacketFactory.CreateCharacter(
            _playerTemplates,
            classType,
            sex,
            hairColor,
            hairStyle,
            face,
            race,
            name
        );
        bool enable = GameClient.Instance.IsCryptEnabled();
        SendGameDataQueue.Instance().AddItem(packet, enable, enable);
        //Debug.Log("Button click ButtonPressed");
        //Debug.Log("");
    }

    public void Clear()
    {
        classManipulator.ClearInput();
        genderManipulator.ClearInput();
        hairStyleManipulator.ClearInput();
        hairColorManipulator.ClearInput();
        faceManipulator.ClearInput();
        raceManipulator.ClearInput();
        userInputField.value = "";
        SetErrorLabel("");
    }

    private void PreviousButtonPressed() {
        classManipulator.ClearInput();
        genderManipulator.ClearInput();
        hairStyleManipulator.ClearInput();
        hairColorManipulator.ClearInput();
        faceManipulator.ClearInput();
        raceManipulator.ClearInput();
        userInputField.value = "";
        SetErrorLabel("");
        GameManager.Instance.OnAuthAllowed();
    }

    private void ShowRotatePawnWindow() {
        pawnRotateWindow.style.display = DisplayStyle.Flex;

        if (pawnZoominButton.ClassListContains("toggle")) {
            pawnZoominButton.RemoveFromClassList("toggle");
        }
    }

    private void HideRotatePawnWindow() {
        pawnRotateWindow.style.display = DisplayStyle.None;
    }

    private void ToggleZoomin(bool removeOnly) {
        if(pawnZoominButton.ClassListContains("toggle")) {
            LoginCameraManager.Instance.ZoomOut();

            pawnZoominButton.RemoveFromClassList("toggle");
        } else {

            LoginCameraManager.Instance.ZoomIn();

            pawnZoominButton.AddToClassList("toggle");
        }
    }
}