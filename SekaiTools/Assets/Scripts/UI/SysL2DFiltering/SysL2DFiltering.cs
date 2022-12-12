using SekaiTools.SystemLive2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SysL2DFiltering
{
    public class SysL2DFiltering : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtDateTimeRange;
        public CharacterFilterDisplayTypeA characterFilterDisplay;
        public UnitFilterDisplayTypeA unitFilterDisplayType;
        [Header("Prefab")]
        public Window dateTimeSettingPrefab;
        public Window characterMaskSettingPrefab;
        public Window unitMaskSettingPrefab;

        SysL2DFilterSet sysL2DFilterSet;
        Action<SysL2DFilterSet> onApply;

        DateTime defaultStartDateTime = new DateTime(2020, 5, 1);

        public void Initialize(SysL2DFilterSet sysL2DFilterSet,Action<SysL2DFilterSet> onApply)
        {
            this.sysL2DFilterSet = sysL2DFilterSet.Clone();
            this.onApply = onApply;
            Refresh();
        }

        public void Refresh()
        {
            txtDateTimeRange.text = sysL2DFilterSet.filter_DateTime == null?
                "È«²¿":
                $"{sysL2DFilterSet.filter_DateTime.dateTimeStart:D} - {sysL2DFilterSet.filter_DateTime.dateTimeEnd:D}";

            if (sysL2DFilterSet.filter_Character == null)
                characterFilterDisplay.SetAllSelected();
            else
                characterFilterDisplay.SetMask(sysL2DFilterSet.filter_Character.characterIdMask);

            if (sysL2DFilterSet.filter_Unit == null)
                unitFilterDisplayType.SetAllSelected();
            else
                unitFilterDisplayType.SetMask(sysL2DFilterSet.filter_Unit.unitIdMask);
        }

        public void Apply()
        {
            if (onApply != null)
                onApply(sysL2DFilterSet);
            window.Close();
        }

        public void Reset_DateTime()
        {
            sysL2DFilterSet.filter_DateTime = null;
            Refresh();
        }

        public void Reset_Character()
        {
            sysL2DFilterSet.filter_Character = null;
            Refresh();
        }

        public void Reset_Unit()
        {
            sysL2DFilterSet.filter_Unit = null;
            Refresh();
        }

        public void Select_DateTime()
        {
            DateTimeRangeSelect.DateTimeRangeSelect dateTimeRangeSelect
                = window.OpenWindow<DateTimeRangeSelect.DateTimeRangeSelect>(dateTimeSettingPrefab);
            DateTimeRange initRange = sysL2DFilterSet.filter_DateTime == null ?
                new DateTimeRange(defaultStartDateTime, DateTime.Now) :
                new DateTimeRange(sysL2DFilterSet.filter_DateTime.dateTimeStart, sysL2DFilterSet.filter_DateTime.dateTimeEnd);
            dateTimeRangeSelect.Initialize(initRange,
                (dtr) =>
                {
                    sysL2DFilterSet.filter_DateTime = new SysL2DFilter_DateTime(dtr.startTime, dtr.endTime);
                    Refresh();
                });
        }

        public void Select_Character()
        {
            CharIDMaskSelect.CharIDMaskSelect charIDMaskSelect
                = window.OpenWindow<CharIDMaskSelect.CharIDMaskSelect>(characterMaskSettingPrefab);


            Action<bool[]> onApply = (bool[] mask) =>
            {
                sysL2DFilterSet.filter_Character = new SysL2DFilter_Character(mask);
                Refresh();
            };
            if (sysL2DFilterSet.filter_Character == null)
            {
                charIDMaskSelect.Initialize(onApply);
            }
            else
            {
                charIDMaskSelect.Initialize(sysL2DFilterSet.filter_Character.characterIdMask,onApply);
            }
        }

        public void Select_Unit()
        {
            UnitIDMaskSelect.UnitIDMaskSelect unitIDMaskSelect = window.OpenWindow<UnitIDMaskSelect.UnitIDMaskSelect>(unitMaskSettingPrefab);


            Action<bool[]> onApply = (bool[] mask) =>
            {
                sysL2DFilterSet.filter_Unit = new SysL2DFilter_Unit(mask);
                Refresh();
            };
            if (sysL2DFilterSet.filter_Unit == null)
            {
                unitIDMaskSelect.Initialize(onApply);
            }
            else
            {
                unitIDMaskSelect.Initialize(sysL2DFilterSet.filter_Unit.unitIdMask, onApply);
            }
        }
    }
}