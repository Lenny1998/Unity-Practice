using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制角色
/// </summary>
public class CtrlHuman : BaseHuman
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider.tag == "Terrain")
            {
                MoveTo(hit.point);
                //NetManager.Send("Move|192.168.1.104,100,200,300,45");
            }
        }
    }

}
