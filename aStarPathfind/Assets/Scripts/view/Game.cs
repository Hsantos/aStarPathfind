using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : GameComponent,AStarServices {

    private Board board;
    private Target target;
    private Player player;
    private Canvas canvas;
    private AStar aStar;

    private Piece[,] pieces;

    void Awake()
    {
        Debug.Log("Game has started!");

        canvas = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>();
        board = ((GameObject)Instantiate(Resources.Load("Prefab/board"),canvas.transform)).gameObject.AddComponent<Board>();
        pieces = new Piece[board.board.GetLength(0), board.board.GetLength(1)];

        DrawView();

        aStar = gameObject.AddComponent<AStar>().Initiate(this,GetNodes(),player.node,target.node);

        aStar.StartAI();
    }


    private void DrawView()
    {
        float offSetX = 102f;
        float offSetY = 102f;
        float posX = 0;
        float posY = 0;

        for (int i = 0; i < board.board.GetLength(0); i++)
        {
            for (int j = 0; j < board.board.GetLength(1); j++)
            {
                Piece pc = null;
                switch(board.board[i,j])
                {
                    case "-1":
                        pc = LoadAndAdd<Piece>(board.transform, "block");
                        pieces[i, j] = pc;
                        pc.node  = new Node(board.board[i, j],i,j, new Vector3(posX, posY, 0));
                
                        break;
                    case "0":
                        pc = LoadAndAdd<Piece>(board.transform, "grass");
                        pieces[i, j] = pc;
                        pc.node = new Node(board.board[i, j],i,j, new Vector3(posX, posY, 0));
                
                         break;
                    case "1":
                        pc = LoadAndAdd<Piece>(board.transform, "grass");
                        player = LoadAndAdd<Player>(board.transform, "player");
                       
                        pieces[i, j] = pc;
                        pc.node = player.node = new Node(board.board[i, j],i,j, new Vector3(posX,posY,0));
                       
                        SetPosition(player.GetComponent<RectTransform>(), new Vector2(posX, posY));
                        break;
                    case "2":
                        pc = LoadAndAdd<Piece>(board.transform, "grass");
                        target = LoadAndAdd<Target>(board.transform, "target");

                        pieces[i, j] = pc;
                        pc.node = target.node = new Node(board.board[i, j],i,j, new Vector3(posX, posY, 0));


                        SetPosition(target.GetComponent<RectTransform>(), new Vector2(posX, posY));
                        break;
                }

                SetPosition(pc.GetComponent<RectTransform>(), new Vector2(posX, posY));
                posX += offSetX;
            }

            posY -= offSetY;
            posX = 0;
        }

        target.transform.SetAsLastSibling();
        player.transform.SetAsLastSibling();
    }

    public Node[,] GetNodes()
    {
        Node[,] nodes = new Node[pieces.GetLength(0), pieces.GetLength(1)];
        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                nodes[i, j] = pieces[i, j].node;
            }
        }

        return nodes;
    }

    private void ResetOpeneds()
    {
        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                pieces[i, j].icon.color = pieces[i,j].defaultColor;
            }
        }
    }

    public void NotifyOpenPath(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            pieces[nodes[i].line, nodes[i].collumn].icon.color = Color.red;
        }
    }
    public void NotifyMovement(Node playerNode)
    {
        ResetOpeneds();
        SetPosition(player.GetComponent<RectTransform>(), playerNode.pos);
    }

    public void NotifyFoundTarget(Node targetNode)
    {
        Debug.LogWarning("TARGET FOUND!");
    }
}