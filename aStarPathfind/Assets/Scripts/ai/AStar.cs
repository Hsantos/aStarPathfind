using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class AStar:MonoBehaviour
{

    private Score score;
    private Goals goals;
    private Heuristic heuristic;
    private AStarServices services;

    private List<Node> openList;
    private List<Node> closeList;
    private List<Node> currentList;
      
    private Node[,] board;
    private Node playerPosition;
    private Node target;

    private float timeAI = 2f;
    private bool foundTarget;

    public AStar Initiate(AStarServices services,Node[,] board, Node playerPosition, Node target)
    {
        this.services = services;
        this.board = board;
        this.playerPosition = playerPosition;
        this.target = target;
        score = new Score();
        goals = new Goals();
        heuristic = new Heuristic();
        LogBoard();

        return this;
    }

    private void LogBoard()
    {
        string str = "";

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                str += j<board.GetLength(1)-1 ? board[i,j].value + ", " : board[i, j].value;
            }

            str += "\n";
        }

        Debug.Log("LOG BOARD" + "\n" + str);
    }


    public void StartAI()
    {
        StartCoroutine(OpenPath(1));
    }


    private IEnumerator OpenPath(int depth)
    {
        goals.UpdateGoals();
        currentList = new List<Node>();
        Node bestNode = null;

        int countDepth = 0;
        //check around the player
        countDepth = depth;
        for (int i = 0; i < depth; i++)
        {
            //check left
            Node left = board[playerPosition.line, playerPosition.collumn - countDepth] ?? null;
            Node right = board[playerPosition.line, playerPosition.collumn + countDepth] ?? null;
            Node up = board[playerPosition.line-countDepth, playerPosition.collumn] ?? null;
            Node down = board[playerPosition.line + countDepth, playerPosition.collumn] ?? null;

            CheckNode(left);
            CheckNode(right);
            CheckNode(up);
            CheckNode(down);

            countDepth++;
        }

        bestNode = currentList.OrderByDescending(x => x.score).Last();

        services.NotifyOpenPath(currentList);

        yield return new WaitForSeconds(timeAI);

        StartCoroutine(ExecuteMovement(bestNode));

        //calc de score aroud the player

    }


    private IEnumerator ExecuteMovement(Node bestNode)
    {
        board[playerPosition.line, playerPosition.collumn] = bestNode;
        playerPosition.line = bestNode.line;
        playerPosition.collumn = bestNode.collumn;
        board[bestNode.line, bestNode.collumn] = playerPosition;
        services.NotifyMovement(bestNode);
        LogBoard();

        yield return new WaitForSeconds(timeAI);


        //check
        if (foundTarget) services.NotifyFoundTarget(bestNode);
        else StartCoroutine(OpenPath(1));
    }

    private void CheckNode(Node node)
    {
        //TODO check close list and nodes to already open
        if (node != null)
        {
            currentList.Add(node);
            int calcHeuristic = (int)heuristic.CalculateManhattanDistance(node.pos, target.pos);
            node.score = score.calcScore(goals.cost, calcHeuristic);
            Debug.Log("CALC NODE : " +  goals.cost + " | " + calcHeuristic +  " | "+ score.calcScore(goals.cost, calcHeuristic));
        }

    }

}
