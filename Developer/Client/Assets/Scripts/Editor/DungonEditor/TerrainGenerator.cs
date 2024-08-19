using AStar;
using AStar.Options;
using System.Collections.Generic;
using UnityEngine;
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	TerrainGenerator
作    者:	HappLI
描    述:	关卡随机地图生成算法
*********************************************************************/

namespace TopGame.ED
{
    public class TerrainGenerator
    {
        HashSet<long> m_vReds = new HashSet<long>();
        HashSet<long> m_vGreens = new HashSet<long>();
        HashSet<long> m_vBlues = new HashSet<long>();
        HashSet<long> m_vPathPoints = new HashSet<long>();

        List<long> m_vPaths = new List<long>();
        int m_nScanPathSize = 1;
        Vector3Int m_GridSize; 
        TerrainSceneLogic m_pLogic;
        //-----------------------------------------------------
        public void Enable(TerrainSceneLogic pEditor)
        {
            m_pLogic = pEditor;
        }
        //-----------------------------------------------------
        public void Disable()
        {
            Clear();
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_vReds.Clear();
            m_vGreens.Clear();
            m_vBlues.Clear();
            m_vPaths.Clear();
            m_vPathPoints.Clear();
            m_nScanPathSize = 1;
        }
        //-----------------------------------------------------
        public void reload()
        {
        }
        //-----------------------------------------------------
        public HashSet<long> GetStautsPoints(int type)
        {
            if (type == 0) return m_vReds;
            else if (type == 1) return m_vGreens;
            else if (type == 2) return m_vBlues;
            else if (type == 3) return m_vPathPoints;
            return null;
        }
        //-----------------------------------------------------
        public List<long> GetPaths()
        {
            return m_vPaths;
        }
        //-----------------------------------------------------
        public void Generator(Vector3Int gridSize, int cellSize,int scanSize=1)
        {
            m_GridSize = gridSize/cellSize;
            int totalCnt = m_GridSize.x * m_GridSize.z;
            if (totalCnt <= 0)
                return;

            Clear();

            HashSet<long> vRandomed = new HashSet<long>();

            //! random red point in grid
            int randomCnt = 0;
            int redCnt = Random.Range(0, totalCnt);
            while(redCnt>0 && randomCnt< totalCnt)
            {
                randomCnt++;
                int x = Random.Range(0, m_GridSize.x);
                int z = Random.Range(0, m_GridSize.z);
                long index = GridToIndex(x, z);
                if (vRandomed.Contains(index))
                    continue;

                //! check neighboring
                if (x > 0 && vRandomed.Contains(GridToIndex(x - 1, z)))
                    continue;
                if (z+1 < m_GridSize.z && vRandomed.Contains(GridToIndex(x, z+1)))
                    continue;
                if (x + 1 < m_GridSize.x && vRandomed.Contains(GridToIndex(x+1, z)))
                    continue;
                if (z>0 && vRandomed.Contains(GridToIndex(x, z-1)))
                    continue;

                redCnt--;
                m_vReds.Add(index);
                vRandomed.Add(index);
            }
            foreach (var db in m_vReds)
            {
                Vector2Int grid = IndexToGrid(db);
                bool neighboringGreen = Random.Range(0, 2)%2==0;
                long neightborIndex = GridToIndex(grid.x - 1, grid.y);
                if (grid.x > 0 && !vRandomed.Contains(neightborIndex))
                {
                    if (neighboringGreen) m_vGreens.Add(neightborIndex);
                    else m_vBlues.Add(neightborIndex);
                    vRandomed.Add(neightborIndex);
                    neighboringGreen = !neighboringGreen;
                }
                neightborIndex = GridToIndex(grid.x, grid.y + 1);
                if (grid.y + 1 < m_GridSize.z && vRandomed.Contains(neightborIndex))
                {
                    if (neighboringGreen) m_vGreens.Add(neightborIndex);
                    else m_vBlues.Add(neightborIndex);
                    vRandomed.Add(neightborIndex);
                    neighboringGreen = !neighboringGreen;
                }
                neightborIndex = GridToIndex(grid.x + 1, grid.y);
                if (grid.x + 1 < m_GridSize.x && vRandomed.Contains(neightborIndex))
                {
                    if (neighboringGreen) m_vGreens.Add(neightborIndex);
                    else m_vBlues.Add(neightborIndex);
                    vRandomed.Add(neightborIndex);
                    neighboringGreen = !neighboringGreen;
                }
                neightborIndex = GridToIndex(grid.x, grid.y - 1);
                if (grid.y > 0 && vRandomed.Contains(neightborIndex))
                {
                    if (neighboringGreen) m_vGreens.Add(neightborIndex);
                    else m_vBlues.Add(neightborIndex);
                    vRandomed.Add(neightborIndex);
                    neighboringGreen = !neighboringGreen;
                }
            }

            var worldGrid = new WorldGrid(m_GridSize.x, m_GridSize.z);
            for (int x =0; x < m_GridSize.x; ++x)
            {
                for(int z =0; z < m_GridSize.z; ++z)
                {
                    long index = GridToIndex(x, z);
                    if (m_vReds.Contains(index))
                    {
                        worldGrid[x, z] = 0;
                    }
                    else
                        worldGrid[x, z] = 1;
                    if (vRandomed.Contains(index)) continue;
                    int random = Random.Range(0, 4);
                    if (random == 0) m_vReds.Add(index);
                    else if (random == 1) m_vGreens.Add(index);
                    else m_vBlues.Add(index);
                }
            }

            m_vPathPoints.Clear();
            var pathfinder = new PathFinder(worldGrid, new PathFinderOptions { UseDiagonals = false });
            //! build path
            for(int j =0; j < 5; ++j)
            {
                Vector2Int start = Vector2Int.zero, end = Vector2Int.zero;
                if (Random.Range(0, 2) == 0)
                {
                    for (int x = 0; x < m_GridSize.x; ++x)
                    {
                        long index = GridToIndex(x, 0);
                        if (m_vReds.Contains(index))
                            continue;
                        start = IndexToGrid(index);
                        break;
                    }

                    for (int x = m_GridSize.x - 1; x >= 0; --x)
                    {
                        long index = GridToIndex(x, m_GridSize.z - 1);
                        if (m_vReds.Contains(index))
                            continue;
                        end = IndexToGrid(index);
                        break;
                    }
                }
                else
                {
                    for (int z = 0; z < m_GridSize.z; ++z)
                    {
                        long index = GridToIndex(0, z);
                        if (m_vReds.Contains(index))
                            continue;
                        start = IndexToGrid(index);
                        break;
                    }

                    for (int z = m_GridSize.z - 1; z >= 0; --z)
                    {
                        long index = GridToIndex(m_GridSize.x - 1, z);
                        if (m_vReds.Contains(index))
                            continue;
                        end = IndexToGrid(index);
                        break;
                    }
                }



                m_vPaths.Clear();
                var path = pathfinder.FindPath(new Position(start.x, start.y), new Position(end.x, end.y));
                for (int i = 0; i < path.Length; ++i)
                {
                    m_vPaths.Add(GridToIndex(path[i].Row, path[i].Column));
                }
                if (m_vPaths.Count > 0) break;
            }
            if(m_vPaths.Count<=0)
            {
                m_pLogic.ShowNotification("没有有效的路径");
            }
            RefreshScan(scanSize, true);
        }
        //-----------------------------------------------------
        public void RefreshScan(int scanSize, bool bForce = false)
        {
            if (m_vPaths.Count <= 0) return;
            if (m_nScanPathSize == scanSize)
            {
                if(!bForce) return;
            }
            m_vPathPoints.Clear();
            m_nScanPathSize = scanSize;
            for (int i = 0; i < m_vPaths.Count; ++i)
            {
                Vector2Int grid = IndexToGrid(m_vPaths[i]);
                m_vPathPoints.Add(m_vPaths[i]);

                for (int s = 1; s <= scanSize; ++s)
                {
                    if (grid.x - s >= 0)
                    {
                        m_vPathPoints.Add(GridToIndex(grid.x - s, grid.y));
                    }
                    if (grid.y + s < m_GridSize.z)
                    {
                        m_vPathPoints.Add(GridToIndex(grid.x, grid.y + s));
                    }
                    if (grid.x + s < m_GridSize.x)
                    {
                        m_vPathPoints.Add(GridToIndex(grid.x + s, grid.y));
                    }
                    if (grid.y - s >= 0)
                    {
                        m_vPathPoints.Add(GridToIndex(grid.x, grid.y - s));
                    }
                }
            }
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
        }
        //-----------------------------------------------------
        public long GridToIndex(int x, int y)
        {
            return x * 100000 + y;
        }
        //-----------------------------------------------------
        public Vector2Int IndexToGrid(long index)
        {
            return new Vector2Int((int)(index/ 100000), (int)(index% 100000));
        }
    }
}