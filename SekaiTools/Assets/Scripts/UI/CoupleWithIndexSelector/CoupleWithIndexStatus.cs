using System;
using System.Collections.Generic;

namespace SekaiTools.UI.CoupleWithIndexSelector
{
    public class CoupleWithIndexStatus : CoupleMatrix<SelectStatus[]>
    {
        public CoupleWithIndexStatus(int length) : base(length)
        {
        }

        public CoupleWithIndexStatus(int columnLength, int rowLength) : base(columnLength, rowLength)
        {
        }

        public int[] AppearCharacters
        {
            get
            {
                HashSet<int> appearCharacters = new HashSet<int>();
                for (int i = 0; i < Rows.Length; i++)
                {
                    for (int j = 0; j < Rows[i].Items.Length; j++)
                    {
                        SelectStatus[] selectStatuses = Rows[i].Items[j];
                        if (selectStatuses == null) continue;
                        foreach (var selectStatus in selectStatuses)
                        {
                            if (selectStatus == SelectStatus.Checked)
                            {
                                appearCharacters.Add(i);
                                appearCharacters.Add(j);
                                break;
                            }
                        }
                    }
                }

                List<int> list = new List<int>(appearCharacters);
                list.Sort();
                return list.ToArray();
            }
        }

        public int ClipCount
        {
            get
            {
                int count = 0;
                foreach (var selectStatuses in this)
                {
                    if (selectStatuses == null) continue;
                    foreach (var selectStatus in selectStatuses)
                    {
                        if (selectStatus == SelectStatus.Checked)
                            count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// 将Unavailable替换为Unchecked
        /// </summary>
        public void RemoveMask()
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                for (int j = 0; j < Rows[i].Items.Length; j++)
                {
                    SelectStatus[] selectStatuses = Rows[i].Items[j];
                    if (selectStatuses == null) continue;
                    for (int k = 0; k < selectStatuses.Length; k++)
                    {
                        if (selectStatuses[k] == SelectStatus.Unavailable)
                            selectStatuses[k] = SelectStatus.Unchecked;
                    }
                }
            }
        }

        /// <summary>
        /// 将不存在的片段标记为Unavailable
        /// </summary>
        public void ApplyMask(CoupleWithIndexStatus coupleWithIndexStatus)
        {
            for (int i = 0; i < Rows.Length && i < coupleWithIndexStatus.Rows.Length; i++)
            {
                for (int j = 0; j < Rows[i].Items.Length && i < coupleWithIndexStatus.Rows[i].Items.Length; j++)
                {
                    SelectStatus[] selectStatuses = Rows[i].Items[j];
                    SelectStatus[] selectStatusesMask = coupleWithIndexStatus.Rows[i].Items[j];
                    if (selectStatuses == null || selectStatusesMask == null) continue;
                    for (int k = 0; k < selectStatuses.Length && k < selectStatusesMask.Length; k++)
                    {
                        if (selectStatusesMask[k] == SelectStatus.Unavailable)
                            selectStatuses[k] = SelectStatus.Unavailable;
                    }
                }
            }
        }

        public CoupleWithIndexStatus DeepClone()
        {
            CoupleWithIndexStatus coupleWithIndexStatus = new CoupleWithIndexStatus(ColumnLength, RowLength);
            for (int i = 0; i < Rows.Length && i < coupleWithIndexStatus.Rows.Length; i++)
            {
                for (int j = 0; j < Rows[i].Items.Length && i < coupleWithIndexStatus.Rows[i].Items.Length; j++)
                {
                    SelectStatus[] selectStatuses = Rows[i].Items[j];
                    if (selectStatuses != null)
                        coupleWithIndexStatus.Rows[i].Items[j] = new List<SelectStatus>(selectStatuses).ToArray();
                    else
                        coupleWithIndexStatus.Rows[i].Items[j] = null;
                }
            }
            return coupleWithIndexStatus;
        }

        public void ForEach(Func<CoupleWithIndexStatusWithMeta, SelectStatus> func)
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                CoupleMatrixRow<SelectStatus[]> coupleMatrixRow = Rows[i];
                if (coupleMatrixRow != null)
                {
                    for (int j = 0; j < coupleMatrixRow.Items.Length; j++)
                    {
                        SelectStatus[] selectStatuses = coupleMatrixRow.Items[j];
                        if (selectStatuses != null)
                        {
                            for (int k = 0; k < selectStatuses.Length; k++)
                            {
                                SelectStatus selectStatus = selectStatuses[k];
                                Rows[i].Items[j][k] = func(new CoupleWithIndexStatusWithMeta(i, j, k, selectStatus));
                            }
                        }
                    }
                }
            }
        }
    }
     
    public class CoupleWithIndexStatusWithMeta
    {
        public int charId1;
        public int charId2;
        public int dataId;
        public SelectStatus selectStatus;

        public CoupleWithIndexStatusWithMeta(int charId1, int charId2, int dataId, SelectStatus selectStatus)
        {
            this.charId1 = charId1;
            this.charId2 = charId2;
            this.dataId = dataId;
            this.selectStatus = selectStatus;
        }
    }
}