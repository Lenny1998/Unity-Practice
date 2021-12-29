using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerInput : MonoBehaviour
{
    private GameActor m_Actor;

    private Command m_KeySpace;
    private Command m_KeyJ;
    private Command m_KeyE;
    private Command m_KeyLeftCtrl;
    private Command m_KeyW;
    // Start is called before the first frame update
    void Start()
    {
        m_Actor = GetComponent<GameActor>();
    }

    // Update is called once per frame
    void Update()
    {
        Command command = InputHandler();
        if (command != null)
        {
            command.Execute(m_Actor);
        }
    }

    private Command InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return new JumpCommand();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            return new FireCommand();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            return new SwapWeaponCommand();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            return new SquatCommand();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            return new MoveCommand(1,1,1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            return new MoveCommand(-1, -1, -1);
        }

        return null;
    }
}
