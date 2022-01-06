using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontChanger : MonoBehaviour {

    [SerializeField] private List<TextMeshProUGUI> textComponents;

    [SerializeField] private TMP_FontAsset dyslexiaFont;
    
    private Dictionary<int, float> defaultFontSizes;
    private Dictionary<int, TMP_FontAsset> defaultFonts;

   // [SerializeField] private DropDownItem dropDown;

    public void Awake() {
        
        EventHandler<SaveSettingsEvent>.RegisterListener(ChangeFont);

        defaultFontSizes = new Dictionary<int, float>();
        defaultFonts = new Dictionary<int, TMP_FontAsset>();
        foreach (TextMeshProUGUI text in textComponents) {
            defaultFonts.Add(text.GetInstanceID(), text.font);
            defaultFontSizes.Add(text.GetInstanceID(), text.fontSize);
        }
            
        
      //  dropDown.onValueChanged += ChangeFontSize;
    }
    private void Start()
    {
        (GameMenuController.Instance.RequestOption<ChangeFontSize>() as ChangeFontSize).AddListener(ChangeFontSize);
    }
    private void OnDisable() {
        EventHandler<SaveSettingsEvent>.UnregisterListener(ChangeFont);
    }

    private void ChangeFontSize(int choice)
    {

        switch (choice)
        {
            case 0: 
                break;
            case 1:
                break;
            case 2: 
                break;

        }
        /*if (newFontSize.Equals("Default")) {
            foreach (TextMeshProUGUI text in textComponents) 
                text.fontSize = defaultFontSizes[text.GetInstanceID()];
            
        }
        else {
            float size = int.Parse(newFontSize);
        
            foreach (TextMeshProUGUI text in textComponents)
                text.fontSize = size;
        }*/
        
    }

    private void ChangeFont(SaveSettingsEvent e) {
        
        if(e.settingsData.dyslexiaFont)
            foreach (TextMeshProUGUI text in textComponents) 
                text.font = dyslexiaFont;
        else 
            foreach (TextMeshProUGUI text in textComponents) 
                text.font = defaultFonts[text.GetInstanceID()];
        
    }
    
    
    
    

}
