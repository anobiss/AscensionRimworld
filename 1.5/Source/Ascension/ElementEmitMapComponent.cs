using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class ElementEmitMapComponent : MapComponent
    {
        private const int ElementCount = 6; // Number of elements
        private int[] cellElements;
        private int gridSize;

        public enum Element
        {
            None = 0,
            Metal = 1,
            Water = 2,
            Wood = 3,
            Fire = 4,
            Earth = 5
        }

        public ElementEmitMapComponent(Map map) : base(map)
        {
            gridSize = Mathf.RoundToInt(Mathf.Sqrt(map.cellIndices.NumGridCells));
            cellElements = new int[gridSize * gridSize * ElementCount];
        }

        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();

            IntVec3 mouseCell = UI.MouseCell();
            int cellCount = GenRadial.NumCellsInRadius(8.9f);
            List<IntVec3> cellsInRadius = GenRadial.RadialCellsAround(mouseCell, 8.9f, true).ToList();

            foreach (IntVec3 cell in cellsInRadius)
            {
                if (cell.x >= 0 && cell.z >= 0 && cell.x < gridSize && cell.z < gridSize) // Ensure cell is within grid bounds
                {
                    IntVec2 intVec2Cell = new IntVec2(cell.x, cell.z);
                    if (HasElements(intVec2Cell))
                    {
                        int elementCount = CountElements(intVec2Cell);
                        float yOffset = -20f;

                        foreach (Element element in Enum.GetValues(typeof(Element)))
                        {
                            int amount = GetElementAt(intVec2Cell, element);

                            if (amount > 0)
                            {
                                Color color = GetElementColor(element, amount);
                                Vector3 labelPos = (Vector3)GenMapUI.LabelDrawPosFor(new IntVec3(intVec2Cell.x, 0, intVec2Cell.z)) + Vector3.up * yOffset;
                                GenMapUI.DrawThingLabel(labelPos, amount.ToString(), color);
                                yOffset += 10f; // Adjust vertical position for next element
                            }
                        }
                    }
                }
            }
        }

        private int GetIndex(IntVec2 cell, Element element)
        {
            return (cell.z * gridSize + cell.x) * ElementCount + (int)element;
        }

        public void AddElementAt(IntVec2 centerCell, int radius, int amount, Element element)
        {
            for (int x = centerCell.x - radius; x <= centerCell.x + radius; x++)
            {
                for (int z = centerCell.z - radius; z <= centerCell.z + radius; z++)
                {
                    IntVec2 cell = new IntVec2(x, z);
                    if (IsValidCell(cell) && IsWithinCircle(centerCell, radius, cell))
                    {
                        int index = GetIndex(cell, element);
                        cellElements[index] += amount;
                    }
                }
            }
        }

        public void RemoveElementAt(IntVec2 centerCell, int radius, int amount, Element element)
        {
            for (int x = centerCell.x - radius; x <= centerCell.x + radius; x++)
            {
                for (int z = centerCell.z - radius; z <= centerCell.z + radius; z++)
                {
                    IntVec2 cell = new IntVec2(x, z);
                    if (IsValidCell(cell) && IsWithinCircle(centerCell, radius, cell))
                    {
                        int index = GetIndex(cell, element);
                        cellElements[index] = Mathf.Max(0, cellElements[index] - amount);
                    }
                }
            }
        }

        private bool IsWithinCircle(IntVec2 centerCell, int radius, IntVec2 cell)
        {
            return Mathf.Pow(cell.x - centerCell.x, 2) + Mathf.Pow(cell.z - centerCell.z, 2) <= radius * radius;
        }

        public int GetElementAt(IntVec2 cell, Element element)
        {
            if (IsValidCell(cell))
            {
                int index = GetIndex(cell, element);
                return cellElements[index];
            }
            return 0;
        }

        private bool IsValidCell(IntVec2 cell)
        {
            return cell.x >= 0 && cell.x < gridSize && cell.z >= 0 && cell.z < gridSize;
        }

        private bool HasElements(IntVec2 cell)
        {
            int startIndex = (cell.z * gridSize + cell.x) * ElementCount;
            for (int i = startIndex; i < startIndex + ElementCount; i++)
            {
                if (cellElements[i] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private int CountElements(IntVec2 cell)
        {
            int count = 0;
            int startIndex = (cell.z * gridSize + cell.x) * ElementCount;
            for (int i = startIndex; i < startIndex + ElementCount; i++)
            {
                if (cellElements[i] > 0)
                {
                    count++;
                }
            }
            return count;
        }

        private Color GetElementColor(Element element, int amount)
        {
            switch (element)
            {
                case Element.Metal:
                    return Color.gray;
                case Element.Water:
                    return Color.cyan;
                case Element.Wood:
                    return Color.green;
                case Element.Fire:
                    return Color.red;
                case Element.Earth:
                    return new Color(0.7f, 0.4f, 0f);//brown
                default:
                    return Color.white;
            }
        }
        public int CalculateElementValueAt(IntVec2 cell, Element element)
        {
            int baseValue = cellElements[GetIndex(cell, element)];
            int totalValue = baseValue; // Start with the current element value

            foreach (Element sourceElement in Enum.GetValues(typeof(Element)))
            {
                if (sourceElement == element) continue; // Skip self

                int sourceIndex = GetIndex(cell, sourceElement);
                int sourceValue = cellElements[sourceIndex];

                if (sourceValue > 0) // Only affect if the source element value is above zero
                {
                    switch (sourceElement)
                    {
                        case Element.Metal:
                            if (element == Element.Water && baseValue > 0)
                                totalValue += sourceValue;
                            if (element == Element.Wood && baseValue > 0)
                                totalValue -= sourceValue;
                            break;
                        case Element.Water:
                            if (element == Element.Wood && baseValue > 0)
                                totalValue += sourceValue;
                            if (element == Element.Fire && baseValue > 0)
                                totalValue -= sourceValue;
                            break;
                        case Element.Wood:
                            if (element == Element.Fire && baseValue > 0)
                                totalValue += sourceValue;
                            if (element == Element.Earth && baseValue > 0)
                                totalValue -= sourceValue;
                            break;
                        case Element.Fire:
                            if (element == Element.Earth && baseValue > 0)
                                totalValue += sourceValue;
                            if (element == Element.Metal && baseValue > 0)
                                totalValue -= sourceValue;
                            break;
                        case Element.Earth:
                            if (element == Element.Metal && baseValue > 0)
                                totalValue += sourceValue;
                            if (element == Element.Water && baseValue > 0)
                                totalValue -= sourceValue;
                            break;
                        default:
                            break;
                    }
                }
            }

            return totalValue;
        }


    }
}