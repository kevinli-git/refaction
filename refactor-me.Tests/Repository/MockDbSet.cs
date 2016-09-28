﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace refactor_me.Tests.Repository
{
    public class MockDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>
         where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public MockDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public override T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public override IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _data.Add(entity);
            }

            return entities;
        }

        public override T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public override IEnumerable<T> RemoveRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _data.Remove(entity);
            }            

            return entities;
        }


        public override T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<T> Local
        {
            get { return new ObservableCollection<T>(_data); }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
