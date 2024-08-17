using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AP4Activity3
{
       public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Person>().Wait();
        }

        //Insert and Update new record  
        public Task<int> SaveItemAsync(Person person)
        {
            if (person.MeterNo != 0)
            {
                return db.UpdateAsync(person);
            }
            else
            {
                return db.InsertAsync(person);
            }
        }
        public Task<Person> GetItemByMeterNoAsync(string meterNo)
        {
            // Parse meterNo to an int
            int meterNumber;
            if (!int.TryParse(meterNo, out meterNumber))
            {
                // Handle invalid meter number format
                return null;
            }

            return db.Table<Person>().Where(p => p.MeterNo == meterNumber).FirstOrDefaultAsync();
        }

        //Delete  
        public Task<int> DeleteItemAsync(Person person)
        {
            return db.DeleteAsync(person);
        }

        //Read All Items  
        public Task<List<Person>> GetItemsAsync()
        {
            return db.Table<Person>().ToListAsync();
        }

        //Read Item  
        public Task<Person> GetItemAsync(int meterNo)
        {
            return db.Table<Person>().Where(i => i.MeterNo == meterNo).FirstOrDefaultAsync();
        }
        public async Task<int> DeleteItemByMeterNoAsync(string meterNo)
        {
            var itemToDelete = await GetItemByMeterNoAsync(meterNo);
            if (itemToDelete != null)
            {
                return await db.DeleteAsync<Person>(itemToDelete.MeterNo);
            }
            return 0;
        }
    }
}
