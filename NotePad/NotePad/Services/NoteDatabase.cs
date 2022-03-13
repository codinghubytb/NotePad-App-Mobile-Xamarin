using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using NotePad.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NotePad.Services
{
    public class NoteDatabase
    {
        readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbPath">path database</param>
        public NoteDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Notes>().Wait();
        }

        /// <summary>
        /// Get list table level async
        /// </summary>
        /// <returns>List level</returns>
        public Task<List<Notes>> GetNoteAsync()
        {
            return _database.Table<Notes>().ToListAsync();
        }

        /// <summary>
        /// Get Level with variable id
        /// </summary>
        /// <param name="id">id Level</param>
        /// <returns>level</returns>
        public Task<Notes> GetNoteAsync(int id)
        {
            return _database.Table<Notes>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Search Note
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Task<List<Notes>> SearchNoteAsync(string a)
        {
            string searchNoSpaces = a.Replace(" ", "%");
            var get_docnumb = _database.QueryAsync<Notes>("SELECT * FROM Notes WHERE Title LIKE ?", "%" + searchNoSpaces + "%");

            return get_docnumb;
        }

        /// <summary>
        /// Update or Insert level
        /// </summary>
        /// <param name="level">level</param>
        /// <returns></returns>
        public Task<int> SaveNoteAsync(Notes notes)
        {
            if (notes.Id != 0)
                return _database.UpdateAsync(notes);
            else
            {
                return _database.InsertAsync(notes);
            }
        }

        /// <summary>
        /// Delete level
        /// </summary>
        /// <param name="level">level</param>
        /// <returns></returns>
        public Task<int> DeleteNotelAsync(Notes notes)
        {
            return _database.DeleteAsync(notes);
        }
    }
}
