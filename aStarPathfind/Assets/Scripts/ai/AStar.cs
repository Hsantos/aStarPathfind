using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class AStar : MonoBehaviour
{
    public const string INVALID_TILE = "-1";
    public const string VALID_TILE = "0";
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

    public AStar Initiate(AStarServices services,Node[,] board, Node playerPosition, Node target)
    {
        this.services = services;
        this.board = board;
        this.playerPosition = playerPosition;
        this.target = target;
        score = new Score();
        goals = new Goals();
        closeList = new List<Node>();
        openList = new List<Node>();
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
                str += j < board.GetLength(1) - 1 ? board[i,j].value + ", " : board[i, j].value;
                //str += j < board.GetLength(1) - 1 ? "["+board[i, j].line+board[i, j].collumn + "]" +  ", " : "[" + board[i, j].line + board[i, j].collumn + "]";
            }

            str += "\n";
        }

        Debug.Log("LOG BOARD" + "\n" + str);
        //Debug.Log("PLAYER POSITION: " + playerPosition.line + " | " + playerPosition.collumn);

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
        int pLine = playerPosition.line;
        int pCollumn = playerPosition.collumn;

        //Debug.LogWarning(playerPosition.line + " | " + playerPosition.collumn);
       
        for (int i = 0; i < depth; i++)
        {
            //check left
            Node left = CheckArray(pLine, pCollumn - countDepth);
            Node right = CheckArray(pLine, pCollumn + countDepth);
            Node up = CheckArray(pLine - countDepth, pCollumn);
            Node down = CheckArray(pLine + countDepth, pCollumn);

            //diagonals
            Node leftUp = CheckArray(pLine - countDepth, pCollumn - countDepth);
            Node leftDown = CheckArray(pLine + countDepth, pCollumn - countDepth);
            Node rightUp = CheckArray(pLine - countDepth, pCollumn + countDepth);
            Node rightDown = CheckArray(pLine + countDepth, pCollumn + countDepth);

            CheckNode(left);
            CheckNode(right);
            CheckNode(up);
            CheckNode(down);

            CheckNode(leftUp);
            CheckNode(leftDown);
            CheckNode(rightUp);
            CheckNode(rightDown);

            countDepth++;
        }

        bestNode = currentList.OrderByDescending(x => x.score).Last();

        services.NotifyOpenPath(currentList);

        yield return new WaitForSeconds(timeAI);

        StartCoroutine(ExecuteMovement(bestNode));

        //calc de score aroud the player

    }

    private Node CheckArray(int i, int j)
    {
        if (i < board.GetLength(0) &&  j < board.GetLength(1) && i >= 0 && j >= 0) return board[i, j];
        return null;
    }

    private IEnumerator ExecuteMovement(Node bestNode)
    {
        board[bestNode.line, bestNode.collumn].value = playerPosition.value;
        board[playerPosition.line, playerPosition.collumn].value = VALID_TILE;

        Node aux = bestNode;
        playerPosition = bestNode;

        services.NotifyMovement(bestNode);
        LogBoard();

        yield return new WaitForSeconds(timeAI);

        //check
        if (foundTarget()) services.NotifyFoundTarget(bestNode);
        else StartCoroutine(OpenPath(1));
    }

    private void CheckNode(Node node)
    {
        if (node == null || closeList.Contains(node)) return;
        if (node.value == INVALID_TILE)
        {
            closeList.Add(node);
            return; 
        } 

        //TODO check close list and nodes to already open
        if (node != null && !openList.Contains(node))
        {
            currentList.Add(node);
            //int calcHeuristic = (int)heuristic.CalculateManhattanDistance(node.pos, target.pos);
            openList.Add(node);

            Vector3 vectorTarget = new Vector3(target.line, target.collumn, 0f);
            Vector3 vectorNode = new Vector3(node.line, node.collumn, 0f);

            int calcHeuristic = (int)heuristic.CalculateManhattanDistance(vectorNode, vectorTarget);
            node.score = score.calcScore(goals.cost, calcHeuristic);
            Debug.Log("CALC NODE [" + node.line + "," + node.collumn +"] " + goals.cost + " | " + calcHeuristic +  " | "+ score.calcScore(goals.cost, calcHeuristic));
        }

    }

    private bool foundTarget()
    {
        return playerPosition.line == target.line && playerPosition.collumn == target.collumn;
    }

}
