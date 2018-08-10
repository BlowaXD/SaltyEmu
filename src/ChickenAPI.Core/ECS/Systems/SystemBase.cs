using System;
using System.Linq;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.ECS.Systems
{
    public abstract class SystemBase : ISystem
    {
        protected static readonly Logger Log = Logger.GetLogger<SystemBase>();
        private Func<IEntity, bool> _filter;

        private DateTime _lastUpdate;

        protected SystemBase(IEntityManager entityManager) => EntityManager = entityManager;


        protected virtual short RefreshRate { get; } = 2;
        protected int RefreshDelay => 1000 / RefreshRate;

        /// <summary>
        ///     Gets filter of the system.
        /// </summary>
        /// <remarks>
        ///     This filter is used to check if the entities needs to be updated by this system.
        /// </remarks>
        protected virtual Expression<Func<IEntity, bool>> Filter { get; }

        protected IEntity[] Entities { get; private set; }

        // todo
        protected bool UpdateCacheRequest { get; set; }

        public IEntityManager EntityManager { get; }

        public virtual void Update(DateTime time)
        {
            if (_lastUpdate.AddMilliseconds(RefreshDelay) > time)
            {
                return;
            }

            if (Entities == null || UpdateCacheRequest)
            {
                UpdateCache();
            }


            try
            {
                foreach (IEntity entity in Entities)
                {
                    Execute(entity);
                }
            }
            catch (Exception e)
            {
                Log.Error("Update()", e);
            }

            _lastUpdate = DateTime.UtcNow;
        }

        public bool Match(IEntity entity)
        {
            _filter = _filter ?? Filter.Compile();

            return (bool)_filter?.Invoke(entity);
        }

        private void UpdateCache()
        {
            UpdateCacheRequest = false;
            Entities = EntityManager.Entities.Where(Match).ToArray();
        }

        protected virtual void Execute(IEntity entity)
        {
        }
    }
}