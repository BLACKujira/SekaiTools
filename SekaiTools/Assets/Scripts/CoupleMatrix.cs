using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    public class CoupleMatrix<T>
    {
        CoupleMatrixRow<T>[] rows;
        public CoupleMatrixRow<T>[] Rows => rows;
        public int RowLength => rows.Length <= 1 ? 0 : rows[1].Items.Length;
        public int ColumnLength => rows.Length <= 1 ? 0 : rows.Length;
        public CoupleMatrixRow<T> this[int index] => rows[index];

        public CoupleMatrix(int length) : this(length, length) { }

        public CoupleMatrix(int columnLength, int rowLength)
        {
            rows = new CoupleMatrixRow<T>[columnLength];
            for (int i = 0; i < columnLength; i++)
            {
                rows[i] = new CoupleMatrixRow<T>(rowLength);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var row in rows)
            {
                foreach (var item in row.Items)
                {
                    yield return item;
                }
            }
        }

        public void ForEach(Func<CoupleMatrixItemWithMeta<T>,T> func)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                CoupleMatrixRow<T> coupleMatrixRow = rows[i];
                if(coupleMatrixRow!=null)
                {
                    for (int j = 0; j < coupleMatrixRow.Items.Length; j++)
                    {
                        T t = coupleMatrixRow.Items[j];
                        if(t!=null)
                        {
                            rows[i].Items[j] = func(new CoupleMatrixItemWithMeta<T>(i, j, t));
                        }
                    }
                }
            }
        }
    }

    public class CoupleMatrixRow<T>
    {
        T[] items;
        public T[] Items => items;
        public T this[int index] => items[index];

        public CoupleMatrixRow(int length)
        {
            items = new T[length];
        }
    }

    public class CoupleMatrixItemWithMeta<T>
    {
        public int charId1;
        public int charId2;
        public T item;

        public CoupleMatrixItemWithMeta(int charId1, int charId2, T item)
        {
            this.charId1 = charId1;
            this.charId2 = charId2;
            this.item = item;
        }
    }
}