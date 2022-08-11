using System;

namespace CriandoORM
{
    public class Service 
    {
        private string connectionString;
        private IType iType;

        public Service(IType _iType)
        {
            this.iType = _iType;
            this.connectionString = this.iType.ConnectionString;
        }

        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        public virtual void Destroy()
        {
            throw new NotImplementedException();
        }

        public virtual void Get()
        {
            throw new NotImplementedException();
        }

        public static T All<T>()
        {
            throw new NotImplementedException();
        }
    }
}
