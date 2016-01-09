using System;

namespace GameCore.FSM
{
    public class GameEntity<TEntityInfo, TEntityConfig, TDirector> : BaseGameEntity
    {
        protected TEntityInfo mEntityInfo;

        protected TEntityConfig mEntityConfig;

        private TDirector mDirector;

        public TEntityInfo info
        {
            get
            {
                return this.mEntityInfo;
            }
        }

        public TEntityConfig config
        {
            get
            {
                return this.mEntityConfig;
            }
        }

        public TDirector director
        {
            get
            {
                return this.mDirector;
            }
        }

        public GameEntity(TEntityInfo entityInfo, TEntityConfig entityConfig, TDirector entityDirector, int entityId)
        {
            this.mEntityInfo = entityInfo;
            this.mEntityConfig = entityConfig;
            this.mDirector = entityDirector;
            base.SetID(entityId);
        }
    }
}
