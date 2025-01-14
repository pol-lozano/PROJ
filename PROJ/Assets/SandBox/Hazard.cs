using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private GameObject hazardObj;
    [SerializeField] private PuzzleGrid grid;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private bool movingHazard;

    [SerializeField] private List<HazardObject> hazardObjects = new List<HazardObject>();

    //Hazard Setup

    [SerializeField] private List<bool> customPattern = new List<bool>();


    private int hazardBoundsMAX;

    private float hazardOffset;

    private void OnEnable()
    {
        EventHandler<UpdateHazardEvent>.RegisterListener(OnUpdateHazard);
        EventHandler<ResetHazardEvent>.RegisterListener(OnResetHazard);      
    }
    private void OnDisable()
    {
        EventHandler<UpdateHazardEvent>.UnregisterListener(OnUpdateHazard);
        EventHandler<ResetHazardEvent>.UnregisterListener(OnResetHazard);
    }
    public void StartHazard(int puzzleID)
    {
        grid = GetComponentInParent<Puzzle>().grid;
        HazardSetup();
        InitializeHazardObjects(puzzleID);
    }

    private void OnResetHazard(ResetHazardEvent eve)
    {
        foreach (HazardObject ho in hazardObjects)
        {
            ho.ResetHazardObject();
        }
        //Reset etc
    }
    private void OnUpdateHazard(UpdateHazardEvent eve)
    {
        if (movingHazard)
        {
            if (eve.reverse == false)
            {
                foreach (HazardObject ho in hazardObjects)
                {
                    ho.CheckHazardBounds(hazardBoundsMAX, moveDirection.normalized, hazardOffset);
                    ho.UpdateHazard(hazardOffset, moveDirection);
                }
            }
            else
            {
                foreach (HazardObject ho in hazardObjects)
                {
                    ho.CheckHazardBounds(hazardBoundsMAX, -moveDirection.normalized, hazardOffset);
                    ho.ReverseHazard(hazardOffset, moveDirection);
                }
            }
            
        }
        
    }

    internal void DeleteHazardObjects()
    {
        for(int i = 0; i < hazardObjects.Count; i++)
        {
            //Animate hazardsobject instead
            Destroy(hazardObjects[i].gameObject);
        }
        hazardObjects.Clear();
    }

    private void HazardSetup()
    {

        hazardBoundsMAX = grid.Size / 2 * grid.NodeOffset;


        for (int i = 0; i < grid.Size; i++)
        {


            for (int j = 0; j < grid.Size; j++)
            {
                if(customPattern[j + i * grid.Size] == true)
                {
                    
                    //SPAWN SHIT H�R
                    GameObject instance = Instantiate(hazardObj, grid.allNodes[i, j].transform.position, new Quaternion(0,0,0,0), transform);
                    hazardObjects.Add(instance.GetComponentInChildren<HazardObject>());
                }
            }
        }


        /*
        foreach (HazardObject ho in GetComponentsInChildren<HazardObject>())
        {
            hazardObjects.Add(ho);
        }
        */
    }
    private void InitializeHazardObjects(int puzzleID)
    {

        hazardOffset = grid.NodeOffset;



        int hazardObjectCounter = 0; 
        foreach (HazardObject ho in hazardObjects)
        {
            ho.StartPos = ho.transform.position;
            ho.PuzzleID = puzzleID;
            hazardObjectCounter++;
        }
    }


}
