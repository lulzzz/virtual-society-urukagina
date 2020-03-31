﻿using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Enum;
using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Objects.FormElements.Interfaces;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Shared.Components.FormElements
{
    public partial class DateRange
    {
        private IDateRangeFormElementData _data => Data as IDateRangeFormElementData;

        private string YearStart
        {
            get => _data.GetYear(DateRangeType.Start).ToString();
            set => _data.SetYear(DateRangeType.Start, value);
        }

        private string MonthStart
        {
            get => _data.GetMonth(DateRangeType.Start).ToString();
            set => _data.SetMonth(DateRangeType.Start, value);
        }

        private string DayStart
        {
            get => _data.GetDay(DateRangeType.Start).ToString();
            set => _data.SetDay(DateRangeType.Start, value);
        }

        private string YearEnd
        {
            get => _data.GetYear(DateRangeType.End).ToString();
            set => _data.SetYear(DateRangeType.End, value);
        }

        private string MonthEnd
        {
            get => _data.GetMonth(DateRangeType.End).ToString();
            set => _data.SetMonth(DateRangeType.End, value);
        }

        private string DayEnd
        {
            get => _data.GetDay(DateRangeType.End).ToString();
            set => _data.SetDay(DateRangeType.End, value);
        }
    }
}