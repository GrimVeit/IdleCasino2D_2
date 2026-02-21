using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc
{
    public Node CurrentNode { get; }
    void MoveTo(Node target, bool IsAbsolute);
    event Action<INpc, Node> OnEndDestination;

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum);

}
