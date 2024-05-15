
using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class ElementEmitMapComponent : MapComponent
    {
        public int[] qiGrid;

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
            int numGridCells = map.cellIndices.NumGridCells;
            qiGrid = new int[numGridCells * Enum.GetValues(typeof(Element)).Length];
        }



        public override void MapComponentOnGUI()
        {
            base.MapComponentOnGUI();
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(qiGrid.Length / Enum.GetValues(typeof(Element)).Length));

            for (int z = gridSize - 1; z >= 0; z--)
            {
                for (int x = 0; x < gridSize; x++) // for each cell on the grid/map
                {
                    bool hasValue = false;
                    StringBuilder labelText = new StringBuilder();

                    foreach (Element element in Enum.GetValues(typeof(Element)))
                    {
                        int emitAmount = GetElementEmitAt(x, z, element);
                        if (emitAmount != 0)
                        {
                            hasValue = true;
                            labelText.AppendLine($"{element}: {emitAmount}");
                        }
                    }

                    if (hasValue)
                    {
                        Vector3 labelPos = (Vector3)GenMapUI.LabelDrawPosFor(new IntVec3(x, 0, z));
                        GenMapUI.DrawThingLabel(labelPos, labelText.ToString().TrimEnd(), Color.white);
                    }
                }
            }
        }

        public void AddElementEmitAt(int centerX, int centerZ, int radius, int amount, Element element)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / Enum.GetValues(typeof(Element)).Length);
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    int index = (z * gridSize + x) * Enum.GetValues(typeof(Element)).Length;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[index + (int)element] += amount;
                        HandleElementRelations(x, z, amount, element);
                    }
                }
            }
        }

        public void RemoveElementEmitAt(int centerX, int centerZ, int radius, int amount, Element element)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / Enum.GetValues(typeof(Element)).Length);
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    int index = (z * gridSize + x) * Enum.GetValues(typeof(Element)).Length;
                    int dx = x - centerX;
                    int dz = z - centerZ;
                    if (dx * dx + dz * dz <= radius * radius)
                    {
                        qiGrid[index + (int)element] -= amount;
                        HandleElementRelations(x, z, -amount, element);
                    }
                }
            }
        }

        public int GetElementEmitAt(int x, int z, Element element)
        {
            int gridSize = (int)Math.Sqrt(qiGrid.Length / Enum.GetValues(typeof(Element)).Length);
            if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            {
                int index = (z * gridSize + x) * Enum.GetValues(typeof(Element)).Length;
                return qiGrid[index + (int)element];
            }
            return 0;
        }

        private void HandleElementRelations(int x, int z, int amount, Element element)
        {
            switch (element)
            {
                case Element.Metal:
                    ModifyElementAt(x, z, amount, Element.Water);
                    ModifyElementAt(x, z, -amount, Element.Wood);
                    break;
                case Element.Water:
                    ModifyElementAt(x, z, amount, Element.Wood);
                    ModifyElementAt(x, z, -amount, Element.Fire);
                    break;
                case Element.Wood:
                    ModifyElementAt(x, z, amount, Element.Fire);
                    ModifyElementAt(x, z, -amount, Element.Earth);
                    break;
                case Element.Fire:
                    ModifyElementAt(x, z, amount, Element.Earth);
                    ModifyElementAt(x, z, -amount, Element.Metal);
                    break;
                case Element.Earth:
                    ModifyElementAt(x, z, amount, Element.Metal);
                    ModifyElementAt(x, z, -amount, Element.Water);
                    break;
            }
        }

        private void ModifyElementAt(int x, int z, int amount, Element element)
        {
            if (amount > 0)
            {
                AddElementEmitAt(x, z, 1, amount, element);
            }
            else if (amount < 0)
            {
                RemoveElementEmitAt(x, z, 1, -amount, element);
            }
        }

        private Color GetElementColor(Element element, int qiAmount)
        {
            switch (element)
            {
                case Element.Metal:
                    return Color.Lerp(Color.white, Color.gray, (float)qiAmount / 250f);
                case Element.Water:
                    return Color.Lerp(Color.white, Color.blue, (float)qiAmount / 250f);
                case Element.Wood:
                    return Color.Lerp(Color.white, Color.green, (float)qiAmount / 250f);
                case Element.Fire:
                    return Color.Lerp(Color.white, Color.red, (float)qiAmount / 250f);
                case Element.Earth:
                    return Color.Lerp(Color.white, Color.yellow, (float)qiAmount / 250f);
                default:
                    return Color.white;
            }
        }
    }
}