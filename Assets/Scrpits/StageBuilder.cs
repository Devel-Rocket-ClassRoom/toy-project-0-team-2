using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;



public class StageBuilder : MonoBehaviour
{
    public GameObject Road;
    public GameObject Plain;
    public GameObject Tree;
    public GameObject Bridge;
    public GameObject River;

    
    public int ScreenWidth = 18;
    public int ScreenHeight = 15;
    public Color color; 
    public Color color2;

    public GameObject RedTeam;
    public GameObject BlueTeam;
    public GameObject RiverTeam;

    public List<GameObject> StageBlue = new List<GameObject>();
    public List<GameObject> StageRed = new List<GameObject>();
    public List<GameObject> Stageriver = new List<GameObject>();

    public NavMeshSurface navMeshSurface;


    private void Start()
    {
        ClearStage(StageRed);
        ClearStage(StageBlue);
        ClearStage(Stageriver);

        CreateRedStage();
        CreateBlueStage();
        CreateRiver(RiverTeam.transform);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClearStage(StageRed);
            ClearStage(StageBlue);
            ClearStage(Stageriver);

            CreateRedStage();
            CreateBlueStage();
            CreateRiver(RiverTeam.transform);

        }
    }
    private void CreateRedStage()
    {
        RedTeam.transform.localRotation = Quaternion.Euler(0, 180, 0);
        RedTeam.transform.localPosition = new Vector3(170, 0, 30);
        CreateStage(RedTeam.transform, StageRed);
    }

    private void CreateBlueStage()
    {
        BlueTeam.transform.localPosition = Vector3.zero;
        CreateStage(BlueTeam.transform, StageBlue);
    }

    private void CreateRiver(Transform parent)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < ScreenHeight+1; j++)
            {
                GameObject bridge;
                GameObject river;

                if (j == 3 || j == 15||j == 4 || j == 14)
                {
                    bridge = Instantiate(Bridge, parent);
                    bridge.transform.localPosition = new Vector3(j * 10, 0, -i * 10);
                    if (j == 14 || j == 3)
                    {
                        bridge.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    }


                    Stageriver.Add(bridge);
                }
                else
                {

                    river = Instantiate(River,parent);
                    river.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    Stageriver.Add(river);
                }

            }
        }

        RiverTeam.transform.localPosition = new Vector3(-5, 0, 20);
    }

    private void CreateStage(Transform parent, List<GameObject> stageList)
    {
        for (int i = 0; i < ScreenWidth; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                
               
                GameObject plain;
                GameObject road;

                if (i == 0 && (j == 0 || j == ScreenHeight - 1))
                {
                    road = Instantiate(Tree, parent);
                    road.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    stageList.Add(road);
                }
                else if(i == ScreenWidth - 4 && j >3 && j < ScreenHeight - 3)
                {
                    road = Instantiate(Road, parent);
                    road.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    stageList.Add(road);
                }
                else if ((j == 3 || j == ScreenHeight - 4) && i < 12)
                {

                    road = Instantiate(Road, parent);
                    road.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    stageList.Add(road);
                }
                else if (i == ScreenWidth - 1 && (j < 6 || j > 11))
                {
                    continue;
                }
                else
                {

                    plain = Instantiate(Plain, parent);
                    if ((i + j) % 2 == 0)
                    {
                        Renderer renderer = plain.GetComponent<Renderer>();
                        renderer.material.color = color;
                    }
                    else
                    {
                        Renderer renderer = plain.GetComponent<Renderer>();
                        renderer.material.color = color2;
                    }

                    plain.transform.localPosition = new Vector3(j * 10, 0, -i * 10);
                    stageList.Add(plain);
                }


          }
            
        
  
        
    }
}
    private void ClearStage(List<GameObject> stageList)
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (stageList[i] != null)
            {
                Destroy(stageList[i]);
            }
        }

        stageList.Clear();
    }
}
