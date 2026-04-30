using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;



public class StageBuilder : MonoBehaviour
{
    // test
    public GameObject Lode;
    public GameObject Plain;

    public GameObject Bridge;
    public GameObject River;


    public int ScreenWidth=18;
    public int ScreenHeight=33;
    public Color color;
    public Color color2;

    public GameObject RedTeam;
    public GameObject BlueTeam;
    public GameObject RiverTeam;

    public List<GameObject> StageBlue = new List<GameObject>();
    public List<GameObject> StageRed = new List<GameObject>();
    public List<GameObject> Stageriver = new List<GameObject>();

    public NavMeshSurface navMeshSurface;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            CreateRedStage();
            CreateBlueStage();
            CreateRiver(RiverTeam.transform);

            navMeshSurface.BuildNavMesh();
        }
    }
    private void CreateRedStage()
    {
        RedTeam.transform.localRotation = Quaternion.Euler(0, 180, 0);
        RedTeam.transform.localPosition = new Vector3(310, 0, 39);
        CreateStage(RedTeam.transform, StageRed);
    }

    private void CreateBlueStage()
    {
        BlueTeam.transform.localPosition = Vector3.zero;
        CreateStage(BlueTeam.transform, StageBlue);
    }

    private void CreateRiver(Transform parent)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                GameObject bridge;
                GameObject river;

                if (j == 2 || j == 29)
                {
                    bridge = Instantiate(Bridge, parent);
                    bridge.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

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

        RiverTeam.transform.localPosition = new Vector3(0, 0, 30);
    }

    private void CreateStage(Transform parent, List<GameObject> stageList)
    {
        for (int i = 0; i < ScreenWidth; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                GameObject plain;
                GameObject lode;

                if (i == 15&&j!=0&&j!=1&&j!=30&&j!=31)
                {
                    lode = Instantiate(Lode, parent);
                    lode.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    stageList.Add(lode);
                }
                else if((j == 2|| j == 29)&&(i<15))
                {

                    lode = Instantiate(Lode, parent);
                    lode.transform.localPosition = new Vector3(j * 10, 0, -i * 10);

                    stageList.Add(lode);
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
}
