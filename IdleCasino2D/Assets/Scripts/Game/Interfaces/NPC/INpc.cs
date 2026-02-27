using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc : ISortable
{
    public Node CurrentNode { get; }
    void MoveTo(Node target, bool IsAbsolute);
    void SetMove(Node node);
    event Action<INpc, Node> OnEndDestination;

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum);

    public void Show();
    public void HideDestroy();

}
