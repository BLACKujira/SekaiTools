using SekaiTools.DecompiledClass;
using SekaiTools.SystemLive2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DSelect
{
    public class SysL2DSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator ugAddable;
        public UniversalGenerator ugInData;
        public Image imgFilterBtnIcon;
        public MasterRefUpdateItem masterRefUpdateItem;
        [Header("Settings")]
        public Sprite spriteNoFilter;
        public Sprite spriteHasFilter;
        [Header("Prefab")]
        public Window filterSettingPrefab;

        List<MergedSystemLive2D> mergedSystemLive2Ds;
        List<MergedSystemLive2D> filteredSystemLive2Ds;
        SysL2DFilterSet filterSet = new SysL2DFilterSet();
        List<SysL2DShow> selectSystemLive2Ds = new List<SysL2DShow>();

        Action<SysL2DShowData> onApply = null; 

        public void Initialize(Action<SysL2DShowData> onApply)
        {
            this.onApply = onApply;
            RefreshTable();
            filteredSystemLive2Ds = mergedSystemLive2Ds;

            masterRefUpdateItem.OnTableUpdated += () =>
            {
                RefreshTable();
                Refresh_Addable();
            };

            Refresh_Addable();
        }

        private void RefreshTable()
        {
            try
            {
                MasterSystemLive2D[] masterSystemLive2Ds = EnvPath.GetTable<MasterSystemLive2D>("systemLive2ds");
                mergedSystemLive2Ds = MergedSystemLive2D.GetMergedSystemLive2Ds(masterSystemLive2Ds);
            }
            catch
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, "获取系统Live2D数据表失败，请尝试更新数据表");
                mergedSystemLive2Ds = new List<MergedSystemLive2D>();
            }
        }

        public void EditFilter()
        {
            SysL2DFiltering.SysL2DFiltering sysL2DFiltering
                = window.OpenWindow<SysL2DFiltering.SysL2DFiltering>(filterSettingPrefab);
            sysL2DFiltering.Initialize(filterSet,
                (sysFS) =>
                {
                    filterSet = sysFS;
                    Refresh_Addable();
                });
        }

        public void Refresh_Addable()
        {
            HashSet<int> usedSysL2Ds =
                new HashSet<int>(selectSystemLive2Ds.Select((msl2d) => msl2d.systemLive2D.FirstId));
            filteredSystemLive2Ds = filterSet.ApplyFilters(mergedSystemLive2Ds);
            imgFilterBtnIcon.sprite = filterSet.IsEmpty ? spriteNoFilter : spriteHasFilter;
            ugAddable.ClearItems();
            ugAddable.Generate(filteredSystemLive2Ds.Count,
                (gobj, id) =>
                {
                    SysL2DItemAddable sysL2DItemAddable = gobj.GetComponent<SysL2DItemAddable>();
                    sysL2DItemAddable.Initialize(filteredSystemLive2Ds[id]);
                    sysL2DItemAddable.btnAddItem.onClick.AddListener(() =>
                    {
                        selectSystemLive2Ds.Add(new SysL2DShow(filteredSystemLive2Ds[id]));
                        sysL2DItemAddable.SetUsedMode();
                        Refresh_InData();
                    });
                    if (usedSysL2Ds.Contains(filteredSystemLive2Ds[id].FirstId))
                        sysL2DItemAddable.SetUsedMode();
                });
            Debug.Log(filteredSystemLive2Ds.Count);
        }

        public void Refresh_InData()
        {
            ugInData.ClearItems();
            ugInData.Generate(selectSystemLive2Ds.Count,
                (gobj,id)=>
                {
                    SysL2DItemInData sysL2DItemInData = gobj.GetComponent<SysL2DItemInData>();
                    SysL2DShow sysL2DShow = selectSystemLive2Ds[id];
                    sysL2DItemInData.Initialize(sysL2DShow.systemLive2D);
                    sysL2DItemInData.btnRemove.onClick.AddListener(() =>
                    {
                        if(selectSystemLive2Ds.Remove(selectSystemLive2Ds[id]))
                        {
                            foreach (var item in ugAddable.items)
                            {
                                SysL2DItemAddable sysL2DItemAddable = item.GetComponent<SysL2DItemAddable>();
                                if (sysL2DItemAddable.MergedSystemLive2D == sysL2DShow.systemLive2D)
                                    sysL2DItemAddable.SetUnusedMode();
                            }
                        }
                        Refresh_InData();
                    });

                    if(id==0)
                    {
                        sysL2DItemInData.btnMoveUp.interactable = false;
                    }
                    else
                    {
                        sysL2DItemInData.btnMoveUp.onClick.AddListener(() =>
                        {
                            (selectSystemLive2Ds[id], selectSystemLive2Ds[id - 1])
                            = (selectSystemLive2Ds[id - 1], selectSystemLive2Ds[id]);
                            Refresh_InData();
                        });
                    }

                    if (id == selectSystemLive2Ds.Count-1)
                    {
                        sysL2DItemInData.btnMoveDown.interactable = false;
                    }
                    else
                    {
                        sysL2DItemInData.btnMoveDown.onClick.AddListener(() =>
                        {
                            (selectSystemLive2Ds[id], selectSystemLive2Ds[id + 1])
                            = (selectSystemLive2Ds[id + 1], selectSystemLive2Ds[id]);
                            Refresh_InData();
                        });
                    }
                }
            );
        }

        public void AddAll()
        {
            HashSet<int> usedSysL2Ds =
                new HashSet<int>(selectSystemLive2Ds.Select((msl2d) => msl2d.systemLive2D.FirstId));
            foreach (var mergedSystemLive2D in filteredSystemLive2Ds)
            {
                if(!usedSysL2Ds.Contains(mergedSystemLive2D.FirstId))
                    selectSystemLive2Ds.Add(new SysL2DShow(mergedSystemLive2D));
            }
            Refresh_InData();

            foreach (var item in ugAddable.items)
            {
                SysL2DItemAddable sysL2DItemAddable = item.GetComponent<SysL2DItemAddable>();
                sysL2DItemAddable.SetUsedMode();
            }
        }

        public void Apply()
        {
            if (onApply != null)
                onApply(new SysL2DShowData(selectSystemLive2Ds));
            window.Close();
        }
    }
}