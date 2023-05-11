using Hanssens.Net;
using View.Contracts;

namespace View.Services
{
    public class LocalStorageService:ILocalStorageService
    {
        private LocalStorage _storage;

        public LocalStorageService()
        {
            var config = new LocalStorageConfiguration
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "LeaveManagement"
            };
            _storage = new LocalStorage(config);
        }
        public void ClearStorage(List<string> keys)
        {
            foreach (var key in keys)
            {
                _storage.Remove(key);
            }
        }

        public bool Exists(string key)
        {
            return _storage.Exists(key);
        }

        public T GetStorageValue<T>(string key)
        {
            var value1 = _storage.Get(key);
            var value = _storage.Get<T>(key);
            return value;
        }

        public void SetStorageValue<T>(T storageValue, string key)
        {
            _storage.Store(key, storageValue);
            _storage.Persist();

        }
    }
}
