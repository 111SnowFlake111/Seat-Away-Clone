using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    public LevelControllerNew currentLevel;
    public GameObject[,] map;
    public GameObject goal;

    public GameObject currentPos;

    int xDistance;
    int yDistance;

    int currentPos_row;
    int currentPos_col;

    bool isAtEntrance = false;

    List<Vector3> path;
    GameObject left;
    GameObject right;
    GameObject up;
    GameObject down;

    // Start is called before the first frame update
    void Start()
    {
        path = new List<Vector3>();

        map = currentLevel.map;
        xDistance = Mathf.Abs(Mathf.RoundToInt(map[0, 0].transform.position.x - map[0, 1].transform.position.x));
        yDistance = Mathf.Abs(Mathf.RoundToInt(map[0, 0].transform.position.z - map[1, 0].transform.position.z));

        path.Add(Vector3.zero);

        currentPos_row = 0;
        currentPos_col = map.GetLength(0) - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GoToEntrance();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindWay();
        }
    }

    void GoToEntrance()
    {
        currentPos_row = 0;
        currentPos_col = map.GetLength(0) - 1;
        currentPos = map[currentPos_col, currentPos_row];
        

        if (transform.position != currentPos.transform.position)
        {
            transform.DOMove(currentPos.transform.position + new Vector3(0, 0.5f, 0), 1f);
            isAtEntrance = true;
        }
        else
        {
            isAtEntrance = true;
        }
    }

    void FindWay()
    {
        if (!isAtEntrance)
        {
            return;
        }

        int currentX = currentPos_col;
        int currentY = currentPos_row;

        int left_costStartToGoal;
        int left_costStartToPos;
        int left_totalCost = 0;

        int right_costStartToGoal;
        int right_costStartToPos;
        int right_totalCost = 0;

        int up_costStartToGoal;
        int up_costStartToPos;
        int up_totalCost = 0;

        int down_costStartToGoal;
        int down_costStartToPos;
        int down_totalCost = 0;

        int sameCost = 0;

        while (path[path.Count - 1] != goal.transform.position)
        {
            //up
            if(currentY - 1 < 0)
            {
                up_totalCost = 99;
            }
            else if (map[currentX, currentY - 1] != null)
            {
                up = map[currentX, currentY - 1];
                up_costStartToPos = 
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - up.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - up.transform.position.z) / yDistance));
                up_costStartToGoal =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - goal.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - goal.transform.position.z) / yDistance));
                up_totalCost = up_costStartToGoal + up_costStartToPos;
            }

            //down
            if (currentY + 1 >= map.GetLength(1))
            {
                down_totalCost = 99;
            }
            else if (map[currentX, currentY + 1] != null)
            {
                down = map[currentX, currentY + 1];
                down_costStartToPos =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - down.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - down.transform.position.z) / yDistance));
                down_costStartToGoal =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - goal.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - goal.transform.position.z) / yDistance));
                down_totalCost = down_costStartToGoal + down_costStartToPos;
            }

            //left
            if (currentX - 1 < 0)
            {
                left_totalCost = 99;
            }
            else if (map[currentX - 1, currentY] != null)
            {
                left = map[currentX - 1, currentY];
                left_costStartToPos =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - left.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - left.transform.position.z) / yDistance));
                left_costStartToGoal =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - goal.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - goal.transform.position.z) / yDistance));
                left_totalCost = left_costStartToGoal + left_costStartToPos;
            }

            //right
            if (currentX + 1 >= map.GetLength(0))
            {
                right_totalCost = 99;
            }
            else if (map[currentX + 1, currentY] != null)
            {
                right = map[currentX + 1, currentY];
                right_costStartToPos =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - right.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - right.transform.position.z) / yDistance));
                right_costStartToGoal =
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.x - goal.transform.position.x) / xDistance)) +
                    Mathf.Abs(Mathf.RoundToInt((map[currentX, currentY].transform.position.z - goal.transform.position.z) / yDistance));
                right_totalCost = right_costStartToGoal + right_costStartToPos;
            }

            int[] stepCosts = new int[]{up_totalCost, down_totalCost, left_totalCost, right_totalCost};
            int stepTaken = stepCosts.Min();

            if (stepTaken == down_totalCost)
            {
                path.Add(down.transform.position);
                currentY++; 
            }
            else if (stepTaken == left_totalCost)
            {
                path.Add(left.transform.position);
                currentX--;
            }
            else if(stepTaken == right_totalCost)
            {
                path.Add(right.transform.position);
                currentX++;
            }
            else if (stepTaken == up_totalCost)
            {
                path.Add(up.transform.position);
                currentY--;
            }
        }

        gameObject.transform.DOPath(path.ToArray(), 3f, pathType: PathType.Linear);
    }
}
