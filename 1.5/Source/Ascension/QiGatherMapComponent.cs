using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class QiGatherMapComponent : MapComponent
    {
        public int[] qiGrid;

        public QiGatherMapComponent(Map map) : base(map)
        {
            int numGridCells = map.cellIndices.NumGridCells;
            qiGrid = new int[numGridCells * 3];
        }

        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();

            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));

            for (int z = gridSize - 1; z >= 0; z--)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int index = z * gridSize + x;
                    int qiAmount = GetQiGatherAt(x, z);
                    Vector3 labelPos = (Vector3)GenMapUI.LabelDrawPosFor(new IntVec3(x, 0, z)); // Assuming y-coordinate is always 0
                    Color color = Color.Lerp(Color.black, Color.white, (float)qiAmount / 100f); // Example: Color based on qiAmount
                    GenMapUI.DrawThingLabel(labelPos, qiAmount.ToString(), color);
                }
            }
        }

        public int GetQiGatherAt(int x, int z)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));
            if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            {
                int index = z * gridSize + x;
                return qiGrid[index * 3 + 2];
            }
            return 0;
        }
        public void AddQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int index = z * gridSize + x;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[index * 3 + 2] += amount;
                    }
                }
            }
            LogQiGrid();
        }

        public void RemoveQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int index = z * gridSize + x;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[index * 3 + 2] -= amount;
                    }
                }
            }
            LogQiGrid();
        }

        public void LogQiGrid()
        {
            Log.Message("Updated Qi Grid:");
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));
            for (int z = gridSize - 1; z >= 0; z--)
            {
                string row = "";
                for (int x = 0; x < gridSize; x++)
                {
                    int index = z * gridSize + x;
                    row += qiGrid[index * 3 + 2].ToString() + " ";
                }
                Log.Message(row.TrimEnd());
            }
        }
    }
}