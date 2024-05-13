using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Ascension
{
    // This map component stores the amount of qi in a given cell.
    // By default, all cells have 0.
    // When things with the QiGather ThingComp are moved, created, or destroyed, this map should be updated by the ThingComp
    // using the ThingComp's range/radius and amount to calculate which cells get qi and how much qi the cells get.
    // Remember only Objects can have QiGather: Buildings, floors, and items. Nothing else.
    // Updates to the grid should work almost exactly like the GlowGrid.
    public class QiGatherMapComponent : MapComponent
    {
        public int[] qiGrid; // 1D array acting as the QiGrid

        // Override MapComponentOnGUI to draw Qi amounts on the map

        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();
            if (LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>().displayQiGrid == true)
            {
                int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 3));

                for (int z = gridSize - 1; z >= 0; z--)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        int qiAmount = GetQiGatherAt(x, z); // Access Qi amount using GetQiGatherAt method
                        if (qiAmount != 0)
                        {
                            Vector3 labelPos = (Vector3)GenMapUI.LabelDrawPosFor(new IntVec3(x, 0, z)); // Assuming y-coordinate is always 0
                            Color color = Color.Lerp(Color.white, Color.yellow, (float)qiAmount / 250f); // Example: Color based on qiAmount
                            GenMapUI.DrawThingLabel(labelPos, qiAmount.ToString(), color);
                        }
                    }
                }
            }
        }

        // Adds qi at the given position within the specified radius.
        public void AddQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / 3);
            // Iterate through the grid
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    int index = z * gridSize + x;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    // Check if the cell is within the circle
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        // Increase the qi amount for the cell
                        qiGrid[index * 3 + 2] += amount;
                    }
                }
            }
            LogQiGrid();
        }

        // Removes qi at the given position within the specified radius.
        public void RemoveQiGatherAt(int centerX, int centerZ, int radius, int amount)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / 3);
            // Iterate through the grid
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    int index = z * gridSize + x;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    // Check if the cell is within the circle
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        // Decrease the qi amount for the cell
                        qiGrid[index * 3 + 2] -= amount;
                    }
                }
            }
            LogQiGrid();
        }

        // Gets the qi amount at the specified position.
        public int GetQiGatherAt(int x, int z)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / 3);
            if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            {
                int index = z * gridSize + x;
                return qiGrid[index * 3 + 2];
            }
            return 0; // Return 0 if coordinates are out of bounds
        }

        // Logs the QiGrid
        public void LogQiGrid()
        {
            Log.Message("Updated Qi Grid:");
            int gridSize = (int)Math.Sqrt(qiGrid.Length / 3);
            for (int z = 0; z < gridSize; z++)
            {
                string row = ""; // Initialize an empty string to store the qi amounts for the row
                for (int x = 0; x < gridSize; x++)
                {
                    int index = z * gridSize + x;
                    row += qiGrid[index * 3 + 2].ToString() + " "; // Append the qi amount followed by a space
                }
                Log.Message(row.TrimEnd()); // Log the row containing the qi amounts, trimming any trailing space
            }
        }

        public QiGatherMapComponent(Map map)
            : base(map)
        {
            int numGridCells = map.cellIndices.NumGridCells;
            qiGrid = new int[numGridCells * 3]; // Initialize QiGrid with the number of grid cells
        }
    }
}