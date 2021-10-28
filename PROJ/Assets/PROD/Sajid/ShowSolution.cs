using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSolution : MonoBehaviour
{
    private InputMaster inputMaster;
    private Animator anim;
    public GameObject[] objects;
    private int i;
    public int puzzleID;
    [SerializeField] private AudioSource source;

    void Awake()
    {
       anim = GetComponent<Animator>();
       inputMaster = new InputMaster();
    }
    
    private void OnEnable()
    {
        inputMaster.Enable();
        EventHandler<ExitPuzzleEvent>.RegisterListener(PlayAnimation);
    }
    private void OnDisable()
    {
        inputMaster.Disable();
        EventHandler<ExitPuzzleEvent>.UnregisterListener(PlayAnimation);

    }

    private void PlayAnimation(ExitPuzzleEvent eve) 
    {
        if (eve.info.ID == puzzleID && eve.success)
        {
            anim.SetBool("Solved", true);
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMaster.PuzzleDEBUGGER.ShowSolution.triggered)
        {
            if (i < objects.Length)
            {
                Debug.Log(i);

                objects[i].SetActive(true);
                i = i + 1;
                

            } else if (i==objects.Length)
            {
               foreach(GameObject obj in objects) {
                    obj.SetActive(false);
                }
                i = 0;
            }
        }
    }

   
}
