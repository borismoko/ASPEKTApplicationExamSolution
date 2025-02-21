using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public T Get(int? id)
        {
            if (typeof(T).IsAssignableFrom(typeof(Contact)))
            {
                return entities
                    .Include("Country")
                    .Include("Company")
                    .Include(c => ((Contact)(object)c).Country)
                    .Include(c => ((Contact)(object)c).Company) 
                    .First(s => s.Id == id);
            }
            else
            {
                return entities.First(s => s.Id == id);
            }
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T).IsAssignableFrom(typeof(Contact)))
            {
                return entities
                    .Include("Country")
                    .Include("Company")
                    .Include(c => ((Contact)(object)c).Country)
                    .Include(c => ((Contact)(object)c).Company)
                    .AsEnumerable();
            }
            else
            {
                return entities.AsEnumerable();
            }
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public List<T> InsertMany(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            entities.AddRange(entities);
            context.SaveChanges();
            return entities;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
            return entity;
        }

    }

}