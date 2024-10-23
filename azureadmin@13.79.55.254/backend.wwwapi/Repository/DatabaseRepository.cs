using backend.wwwapi.DataContext;
using Microsoft.EntityFrameworkCore;

namespace backend.wwwapi.Repository
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : class
    {
        private CatContext _db;
        private DbSet<T> _table = null;
        public DatabaseRepository(CatContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public void Delete(int id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
            this.Save();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(int id)
        {
            return _table.Find(id);
        }

        public void Insert(T obj)
        {
            _table.Add(obj);
            this.Save();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
            this.Save();
        }
    }
}
