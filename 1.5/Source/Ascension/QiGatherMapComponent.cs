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

        // Adds qi at the given position within the specified radius.
        public void AddQiGatherAt(int centerX, int centerY, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 2)); // Calculate grid size
                                                                            // Iterate through the grid
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    int index = y * gridSize + x;
                    int dx = x - centerX;
                    int dy = y - centerY;
                    // Check if the cell is within the circle
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        // Increase the qi amount for the cell
                        qiGrid[index * 2 + 1] += amount;
                    }
                }
            }
            LogQiGrid();
        }

        // Removes qi at the given position within the specified radius.
        public void RemoveQiGatherAt(int centerX, int centerY, int radius, int amount)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 2)); // Calculate grid size
                                                                            // Iterate through the grid
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    int index = y * gridSize + x;
                    int dx = x - centerX;
                    int dy = y - centerY;
                    // Check if the cell is within the circle
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        // Decrease the qi amount for the cell
                        qiGrid[index * 2 + 1] -= amount;
                    }
                }
            }
            LogQiGrid();
        }

        // Gets the qi amount at the specified position.
        public int GetQiGatherAt(int x, int y)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 2)); // Calculate grid size
            if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            {
                int index = y * gridSize + x;
                return qiGrid[index * 2 + 1];
            }
            return 0; // Return 0 if coordinates are out of bounds
        }

        // Logs the QiGrid
        public void LogQiGrid()
        {
            Log.Message("Updated Qi Grid:");
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / 2)); // Calculate grid size
            for (int y = 0; y < gridSize; y++)
            {
                string row = ""; // Initialize an empty string to store the qi amounts for the row
                for (int x = 0; x < gridSize; x++)
                {
                    int index = y * gridSize + x;
                    row += qiGrid[index * 2 + 1].ToString() + " "; // Append the qi amount followed by a space
                }
                Log.Message(row.TrimEnd()); // Log the row containing the qi amounts, trimming any trailing space
            }
        }

        public QiGatherMapComponent(Map map)
            : base(map)
        {
            int numGridCells = map.cellIndices.NumGridCells;
            qiGrid = new int[numGridCells * 2]; // Initialize QiGrid with the number of grid cells
        }
    }
}