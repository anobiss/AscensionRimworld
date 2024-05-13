using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class QiGatherMapComponent : MapComponent
    {
        public IntGrid qiGrid; // Changed from int[] to IntGrid

        public QiGatherMapComponent(Map map) : base(map)
        {
            qiGrid = new IntGrid();
            qiGrid.ClearAndResizeTo(map); // Ensure the IntGrid matches the map size
        }

        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();

            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.CellsCount / 3));

            for (int z = gridSize - 1; z >= 0; z--)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int qiAmount = qiGrid[x, z]; // Access Qi amount using IntGrid indexer
                    if (qiAmount != 0)
                    {
                        Vector3 labelPos = (Vector3)GenMapUI.LabelDrawPosFor(new IntVec3(x, 0, z)); // Assuming y-coordinate is always 0
                        Color color = Color.Lerp(Color.black, Color.white, (float)qiAmount / 100f); // Example: Color based on qiAmount
                        GenMapUI.DrawThingLabel(labelPos, qiAmount.ToString(), color);
                    }
                }
            }
        }

        public int GetQiGatherAt(int x, int z)
        {
            return qiGrid[x, z]; // Access Qi amount using IntGrid indexer
        }

        public void AddQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.CellsCount / 3));
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[x, z] += amount; // Modify Qi amount using IntGrid indexer
                    }
                }
            }
            LogQiGrid();
        }

        public void RemoveQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.CellsCount / 3));
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[x, z] -= amount; // Modify Qi amount using IntGrid indexer
                    }
                }
            }
            LogQiGrid();
        }

        public void LogQiGrid()
        {
            Log.Message("Updated Qi Grid:");
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.CellsCount / 3));
            for (int z = gridSize - 1; z >= 0; z--)
            {
                string row = "";
                for (int x = 0; x < gridSize; x++)
                {
                    row += qiGrid[x, z].ToString() + " ";
                }
                Log.Message(row.TrimEnd());
            }
        }
    }
}