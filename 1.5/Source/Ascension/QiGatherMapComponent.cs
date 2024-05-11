using RimWorld;
using RimWorld.Utility;
using System;
using System.Reflection;
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



        public override void MapComponentOnGUI()
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / 3); // Calculate grid size
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    int index = y * gridSize + x;
                    int qiAmount = GetQiGatherAt(x, y); // Get qi amount at the current position
                    Vector3 worldPos = new Vector3(x + 0.5f, Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays) + 0.1f, y + 0.5f); // Get world position of the cell
                    Vector2 labelPos = Find.Camera.WorldToScreenPoint(worldPos); // Convert world position to screen position
                    Color color = qiAmount > 0 ? Color.white : Color.clear; // Set color to white if qiAmount > 0, otherwise clear
                    string qiString = qiAmount.ToString(); // Convert qi amount to string
                                                           // Draw label with qi amount
                    Widgets.Label(new Rect(labelPos.x, labelPos.y, 100f, 100f), qiString);
                }
            }
        }
        // Adds qi at the given position within the specified radius.
        public void AddQiGatherAt(int centerX, int centerY, int radius, int amount)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length);
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
                        qiGrid[index + 2] += amount;
                    }
                }
            }
        }

        // Removes qi at the given position within the specified radius.
        public void RemoveQiGatherAt(int centerX, int centerY, int radius, int amount)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length);
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
                        qiGrid[index + 2] -= amount;
                    }
                }
            }
        }

        // Gets the qi amount at the specified position.
        public int GetQiGatherAt(int x, int y)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length);
            if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            {
                int index = y * gridSize + x;
                return qiGrid[index + 2];
            }
            return 0; // Return 0 if coordinates are out of bounds
        }

        public QiGatherMapComponent(Map map)
            : base(map)
        {
            int numGridCells = map.cellIndices.NumGridCells;
            qiGrid = new int[numGridCells]; // Initialize QiGrid with the number of grid cells
        }
    }
}