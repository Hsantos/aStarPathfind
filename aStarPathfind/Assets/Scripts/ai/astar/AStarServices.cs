using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AStarServices {

    void NotifyOpenPath(List<Node> nodes);
    void NotifyMovement(Node playerNode);
    void NotifyFoundTarget(Node targetNode);
}
