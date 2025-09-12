using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class AtlosMain : MonoBehaviour
    {
        /*Atlos stands for:
            Artificial
            Target
            Locating
            Operating
            System
         * */

        public static AtlosMain atlos;

        public bool FieldGenerated = false;

        public Vector2 size;
        public AtlosSingle unit;

        private Vector3 unitSize;
        private Vector2 startPos;

        private AtlosSingle[,] field;
        private AtlosSingle scoutingUnit;

        private bool foundTarget = false;

        private void Awake()
        {
            if (atlos == null)
                atlos = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
        }

        [ContextMenu("Create Field")]
        private void CreateField()
        {

            if (unit == null)
            {
                Debug.LogError("A.T.L.O.S. has no assgined singles");
                return;
            }

            FieldGenerated = true;

            unitSize = unit.GetSize();
            startPos.y = -(size.y - 1) / 2f;
            startPos.x = -(size.x - 1) / 2f;
            field = new AtlosSingle[(int)size.x, (int)size.y];

            for (int x = 0; x < size.x; x++)
            {
                 for (int y = 0; y < size.y; y++)
                {
                    //field[x, y] = Instantiate(unit, new Vector3((x + startPos.x) * unitSize.x, 0, (y + startPos.y) * unitSize.z), Quaternion.identity, transform);
                    field[x, y] = Instantiate(unit, new Vector3(0,0,0), Quaternion.identity, transform);
                    field[x, y].gameObject.name = "Atlos[" + x.ToString() + "-" + y.ToString() + "]";
                    field[x, y].transform.localPosition = new Vector3((x + startPos.x) * unitSize.x, 0, (y + startPos.y) * unitSize.z);
                    field[x, y].gameObject.layer = LayerMask.NameToLayer("A.T.L.O.S.");

                }
            }

        }

        [ContextMenu("Scan All Units")]
        private void ScanAllUnits()
        {
            Vector3 unitPos = Vector3.zero;

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    field[x, y].ScanUnit(out foundTarget, out unitPos);
                    if (foundTarget)
                    {
                        scoutingUnit = field[x, y];
                        return;
                    }

                }
            }
        }

        public (bool _foundTarget, Vector3 _scoutUnitPos) getTargetFieldPosition()
        {
            if (!FieldGenerated)
                CreateField();

            ScanAllUnits();
            return (_foundTarget: foundTarget, _scoutUnitPos: scoutingUnit.transform.position);

        }

    }
}