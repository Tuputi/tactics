using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class Pathfinding : MonoBehaviour {

    static OrderedBag<Tile> openList = new OrderedBag<Tile>();
    static List<Tile> closedList = new List<Tile>();

    public static List<Tile> GetPath(Tile starTile, Tile endTile)
    {
        openList = new OrderedBag<Tile>();
        closedList = new List<Tile>();

        starTile.gCost = 0;
        starTile.pathfindingCost = starTile.gCost + GetHeuristic(starTile, endTile);
        starTile.cameFrom = null;
        openList.Add(starTile);

        while (openList.Count > 0)
        {
            if (closedList.Count > 1000)
            {
                Debug.Log("Too many in closed list");
                return null;
            }

            Tile current = openList[0];
            foreach (Tile t in openList)
            {
                if (t.pathfindingCost <= current.pathfindingCost)
                {
                    current = t;
                }
            }

            if (current == endTile)
            {
                Debug.Log("Path found!");
                return ReturnPath(current, starTile);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (Tile t in WeighNeighbours(current))
            {
                float newCost = t.gCost + GetHeuristic(t, endTile);
                if (!(closedList.Contains(t)))
                {
                    if (openList.Contains(t) && newCost < t.pathfindingCost)
                    {
                        openList.Remove(t);
                        t.pathfindingCost = newCost;
                        t.cameFrom = current;
                        openList.Add(t);
                    }
                    else if (!openList.Contains(t))
                    {
                        t.pathfindingCost = newCost;
                        openList.Add(t);
                    }

                }

            }
        }
        Debug.Log("No path found");
        return null;
    }

    static List<Tile> WeighNeighbours(Tile tile)
    {
        List<Tile> WeighedNeighbours = new List<Tile>(tile.neighbours);
        List<Tile> removeThee = new List<Tile>();
        foreach (Tile t in WeighedNeighbours)
        {
            if (t.isWalkable)
            {
                if (!t.isOccupied)
                {
                    if (!closedList.Contains(t))
                    {
                        if (!openList.Contains(t))
                        {
                            t.cameFrom = tile;
                            float heightcost = System.Math.Abs(t.cameFrom.height - t.height);
                            t.gCost = t.cameFrom.gCost + t.movementCost + heightcost;
                        }
                        else
                        {
                            float heightcost = System.Math.Abs(t.cameFrom.height - t.height);
                            float newCost = heightcost + t.movementCost + t.cameFrom.gCost;
                            if (newCost < t.gCost)
                            {
                                t.gCost = newCost;
                                t.cameFrom = tile;
                            }
                        }
                    }
                    else{removeThee.Add(t);}
                }
                else{removeThee.Add(t);}
            }
            else{removeThee.Add(t);}
        }
        foreach (Tile t in removeThee)
        {
            WeighedNeighbours.Remove(t);
        }
        return WeighedNeighbours;
    }


    public static float GetHeuristic(Tile current, Tile goal)
    {
        return ((System.Math.Abs(current.xPos - goal.xPos) + System.Math.Abs(current.yPos - goal.yPos)));

    }

    static List<Tile> ReturnPath(Tile tile, Tile start)
    {
        List<Tile> path = new List<Tile>();
        path.Add(tile);
        while (tile.cameFrom != start.cameFrom)
        {
            tile = tile.cameFrom;
            path.Add(tile);
        }
        return path;
    }
}
