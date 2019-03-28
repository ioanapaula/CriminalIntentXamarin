using System;
using Android.Content;
using Android.Database.Sqlite;
using static CriminalIntentXamarin.Droid.DataBase.CrimeDbSchema;

namespace CriminalIntentXamarin.Droid.DataBase
{
    public class CrimeBaseHelper : SQLiteOpenHelper
    {
        private const int Version = 1;
        private const string DbName = "crimeBase.db";

        public CrimeBaseHelper(Context context) : base(context, DbName, null, Version)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL("create table " + CrimeTable.Name + "(" +
                " _id integer primary key autoincrement, " +
                CrimeTable.Cols.Uuid + ", " +
                CrimeTable.Cols.Title + ", " +
                CrimeTable.Cols.Date + ", " +
                CrimeTable.Cols.Solved + ", " +
                CrimeTable.Cols.SuspectName + ", " +
                CrimeTable.Cols.SuspectNumber +
                ")");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
        }
    }
}
