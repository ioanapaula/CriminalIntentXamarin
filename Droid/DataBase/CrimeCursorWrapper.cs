﻿using System;
using Android.Database;
using CriminalIntentXamarin.Droid.Data;
using Java.Util;
using static CriminalIntentXamarin.Droid.DataBase.CrimeDbSchema;

namespace CriminalIntentXamarin.Droid.DataBase
{
    public class CrimeCursorWrapper : CursorWrapper
    {
        public CrimeCursorWrapper(ICursor cursor) : base(cursor)
        {
        }

        public Crime GetCrime()
        {
            var uuidString = GetString(GetColumnIndex(CrimeTable.Cols.Uuid));
            var title = GetString(GetColumnIndex(CrimeTable.Cols.Title));
            var date = GetLong(GetColumnIndex(CrimeTable.Cols.Date));
            var isSolved = GetInt(GetColumnIndex(CrimeTable.Cols.Solved));
            var suspectName = GetString(GetColumnIndex(CrimeTable.Cols.SuspectName));
            var suspectNumber = GetString(GetColumnIndex(CrimeTable.Cols.SuspectNumber));

            var crime = new Crime(UUID.FromString(uuidString));
            crime.Title = title;
            crime.Date = new Date(date);
            crime.Solved = isSolved != 0;
            crime.SuspectName = suspectName;
            crime.SuspectNumber = suspectNumber;

            return crime;
        }
    }
}
