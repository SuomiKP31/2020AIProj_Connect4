using UnityEngine;

public class HumanControlAI : BaseAI
{

    public override Vector3 GetAction()
    {
        // Not actually called. Human player is exclusively processed in GameController, because controller has to wait them to respond
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return spawnPos;
    }
}
