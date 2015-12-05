using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class PathFinding : MonoBehaviour {

	//Tile startTile;
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
            foreach(Tile t in openList)
            {
                if(t.pathfindingCost <= current.pathfindingCost)
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
                    if(openList.Contains(t) && newCost < t.pathfindingCost)
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
            if (t.walkable)
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
                    else
                    {
                        removeThee.Add(t);
                    }

              }
                else
                {
                    removeThee.Add(t);
                }
  
            }
            else
            {
                removeThee.Add(t);
            }
        }
        foreach (Tile t in removeThee)
        {
            WeighedNeighbours.Remove(t);
        }
        return WeighedNeighbours;
    }


   public static float GetHeuristic(Tile current, Tile goal)
    {
        return ((System.Math.Abs(current.positionRow - goal.positionRow) + System.Math.Abs(current.positionColumn - goal.positionColumn)));
    
    }
    
    static List<Tile> ReturnPath(Tile tile, Tile start)
    {
        List<Tile> path = new List<Tile>();
        path.Add(tile);
        while(tile.cameFrom != start.cameFrom)
        {
            tile = tile.cameFrom;
            path.Add(tile);
        }
        return path;
    }


    public static List<Tile> GetPossibleRange(Tile startTile, float energy)
    {
        List<Tile> closedList = new List<Tile>();
        List<Tile> openList = new List<Tile>();
        List<Tile> remove = new List<Tile>();
        startTile.gCost = 0;
        openList.Add(startTile);
                
        while(openList.Count > 0)
        {
            Tile current = openList[0];
            openList.Remove(current);
            closedList.Add(current);
           
            List<Tile> neighbourList = new List<Tile>(current.neighbours);
            foreach(Tile t in neighbourList)
            {
                if (CheckValidity(t)) //run all additional validitychecks here
                {
                       if (!closedList.Contains(t))
                       {
                            if (!openList.Contains(t))
                            {
                                t.cameFrom = current;
                                float heightcost = System.Math.Abs(t.cameFrom.height - t.height);
                                t.gCost = t.cameFrom.gCost + t.movementCost + heightcost;
                                if (t.gCost > energy)
                                {
                                    remove.Add(t);
                                }
                            }
                            else
                            {
                                float heightcost = System.Math.Abs(t.cameFrom.height - t.height);
                                float newCost = heightcost + t.movementCost + t.cameFrom.gCost;
                                if (newCost < t.gCost)
                                {
                                    t.gCost = newCost;
                                    t.cameFrom = current;
                                }
                            }
                        }
                        else
                        {
                            remove.Add(t);
                        }
                }
                else
                {
                    remove.Add(t);
                }
            }
            foreach(Tile r in remove)
            {
                neighbourList.Remove(r);
            }
            foreach(Tile t in neighbourList)
            {
                openList.Add(t);
            }
        }

        List<Tile> possibleTiles = new List<Tile>();
        foreach(Tile t in closedList)
        {
            if (!(possibleTiles.Contains(t)))
            {
                possibleTiles.Add(t);
            }
        }
        return possibleTiles;
    }

    public static List<Tile> GetAttackRange(Tile startTile, AttackType attackType)
    {
        List<Tile> range = new List<Tile>();
        switch (attackType)
        {
            case AttackType.Meelee:
                range = startTile.neighbours;
                break;
            case AttackType.AreaOfEffect:
                range = GetPossibleRange(startTile, startTile.character.characterRangeEnergy-3f);
                break;
            case AttackType.MeeleeReach:
                break;
            case AttackType.Ranged:
                range = GetPossibleRange(startTile, startTile.character.characterRangeEnergy);
                break;
            default:
                break;
        }
        return range;
    }

    public static List<Tile> GetAttackRange(Tile startTile, float energy)
    {
        return GetPossibleRange(startTile, energy);
    }

   static bool CheckValidity(Tile tile)
    {
        if (!tile.walkable)
        {
            return false;
        }
       
        return true;
    }

    public static Tile FindTarget(Tile tile, Team ownTeam)
    {

        List<Tile> closedList = new List<Tile>();
        List<Tile> openList = new List<Tile>();
        Tile next = tile;
        openList.Add(next);
        int cap = 0;
        while (openList.Count >0 && cap <1000)
        {
            openList.Remove(next);
            closedList.Add(next);
            foreach (Tile t in next.neighbours)
            {
                if (CheckValidity(t))
               {
                    if (!openList.Contains(t))
                    {
                        if (t.isOccupied)
                        {
                            if (t.character.team != ownTeam)
                            {
                                if (t.character.isAlive)
                                {
                                    return t;
                                }
                            }
                        }
                        openList.Add(t);
                    }
               }
            }
            next = openList[0];
            cap++;
        }
        Debug.Log("No possible target found");
        return null;
   }

   

}
