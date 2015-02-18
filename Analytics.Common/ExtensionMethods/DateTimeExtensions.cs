using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static bool Between(this DateTime date, DateTime? fromDate, DateTime? toDate)
        {
            DateTime frmDate = fromDate.HasValue ? fromDate.Value.Date : DateTime.MinValue.Date;
            DateTime tillDate = fromDate.HasValue ? fromDate.Value.Date : DateTime.MaxValue.Date;

            return date.Date >= frmDate && date.Date <= tillDate;
        }

        public static bool DateEquals(this DateTime? date, DateTime? compareDate)
        {
            DateTime thisDate = date.HasValue ? date.Value.Date : DateTime.MinValue.Date;
            DateTime otherDate = compareDate.HasValue ? compareDate.Value.Date : DateTime.MinValue.Date;

            return thisDate == otherDate;
        }
    }
}
