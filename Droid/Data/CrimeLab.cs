using System.Collections.Generic;
using Android.Content;
using Android.Database.Sqlite;
using CriminalIntentXamarin.Droid.DataBase;
using Java.Util;
using static CriminalIntentXamarin.Droid.DataBase.CrimeDbSchema;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeLab
    {
        private static CrimeLab crimeLab;

        private Context _context;
        private SQLiteDatabase _database;

        private CrimeLab(Context context)
        {
            _context = context.ApplicationContext;
            _database = new CrimeBaseHelper(_context).WritableDatabase;
        }

        public List<Crime> Crimes
        {
            get
            {
                var crimes = new List<Crime>();
                var cursor = QueryCrimes(null, null);
                try
                {
                    cursor.MoveToFirst();
                    while (!cursor.IsAfterLast)
                    {
                        crimes.Add(cursor.GetCrime());
                        cursor.MoveToNext();
                    }
                }
                finally
                {
                    cursor.Close();
                }

                return crimes;
            }
        } 

        public static CrimeLab Get(Context context)
        {
            if (crimeLab == null)
            {
                crimeLab = new CrimeLab(context);
            }

            return crimeLab;
        }

        public static ContentValues GetContentValues(Crime crime)
        {
            var values = new ContentValues();
            values.Put(CrimeTable.Cols.Uuid, crime.Id.ToString());
            values.Put(CrimeTable.Cols.Title, crime.Title);
            values.Put(CrimeTable.Cols.Date, crime.Date.Time);
            values.Put(CrimeTable.Cols.Solved, crime.Solved ? 1 : 0);
            values.Put(CrimeTable.Cols.Suspect, crime.Suspect);

            return values;
        }

        public Crime GetCrime(UUID id)
        {
            var cursor = QueryCrimes(CrimeTable.Cols.Uuid + " = ?", new string[] { id.ToString() });
            try
            {
                if (cursor.Count == 0)
                {
                    return null;
                }

                cursor.MoveToFirst();
                return cursor.GetCrime();
            }
            finally
            {
                cursor.Close();
            }
        }

        public void AddCrime(Crime crime)
        {
            var values = GetContentValues(crime);
            _database.Insert(CrimeTable.Name, null, values);
        }

        public void UpdateCrime(Crime crime)
        {
            var uuidString = crime.Id.ToString();
            var contentValues = GetContentValues(crime);

            _database.Update(CrimeTable.Name, contentValues, CrimeTable.Cols.Uuid + " = ?", new string[] { uuidString });
        }

        private CrimeCursorWrapper QueryCrimes(string whereClause, string[] whereArgs)
        {
            var cursor = _database.Query(
                CrimeTable.Name,
                null,
                whereClause,
                whereArgs,
                null,
                null,
                null);

            return new CrimeCursorWrapper(cursor);
        }
    }
}
