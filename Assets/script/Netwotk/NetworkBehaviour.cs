    using UnityEngine;
    using Mirror;

    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkBehaviour : MonoBehaviour
    {
        protected NetworkIdentity networkIdentity;
        private NetworkWorker _networkWorker;
        public string sessionIdOwner => networkIdentity.sessionIdOwner;
        public string id => networkIdentity.id;
        public NetworkWorker networkWorker
        {
            get
            {
                if (_networkWorker == null) _networkWorker = new NetworkWorker(this);
                return _networkWorker;
            }
        }

        // check if local is owner of the object
        protected bool isOwn => networkIdentity.IsOwner();

        protected virtual void Awake()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            if (networkIdentity == null)
            {
                Debug.LogError($"{gameObject.name} does not have a NetworkIdentity component.");
                return;
            }

            // Gọi hàm khởi tạo cho các class kế thừa
            OnStartNetwork();
        }

        void Start()
        {
            _networkWorker.Invoke("Greeting", "hello");
        }

        /// <summary>
        /// Khởi tạo thông tin mạng của đối tượng
        /// </summary>
        public void InitializeNetworkObject(string id, string sessionIdOwner)
        {
            if (networkIdentity == null)
            {
                Debug.LogError($"Cannot initialize NetworkObject for {gameObject.name}: NetworkIdentity is null.");
                return;
            }

            networkIdentity.Init(id, sessionIdOwner);
        }

        [Command]
        public void Hello()
        {
            Debug.Log("Hello method executed.");
        }

        [Command]
        public void Greeting(string message)
        {
            Debug.Log("Hello method executed.");
        }

        /// <summary>
        /// Hàm này sẽ được override trong các lớp kế thừa để thực thi logic mạng tùy chỉnh
        /// </summary>
        public virtual void OnStartNetwork()
        {
            // Để trống, dành cho lớp con.
        }


    }
