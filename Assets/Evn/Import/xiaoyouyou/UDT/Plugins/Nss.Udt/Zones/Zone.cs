using UnityEngine;

namespace Nss.Udt.Zones {
    public class Zone : MonoBehaviour {
		public enum ZoneTriggerTypes {
			Local,			// Let events fire on resident gameObject
			SendMessage		// Send messages to target gameObject
		}
		
		/// <summary>
		/// The type of the trigger.
		/// </summary>
		public ZoneTriggerTypes triggerType;
		
		/// <summary>
		/// The color.
		/// </summary>
        public Color color = new Color(0f, 255f, 255f, 0.5f);
		
		/// <summary>
		/// The message receiver.
		/// </summary>
        public GameObject messageReceiver;
		
		/// <summary>
		/// Should SendMessage require receivers?
		/// </summary>
		public bool RequireReceivers = false;
		
		/// <summary>
        /// Method name to use in SendMessage
        /// </summary>
        public string MessageEnterHandler;

        /// <summary>
        /// Method name to use in SendMessage
        /// </summary>
        public string MessageStayHandler;

        /// <summary>
        /// Method name to use in SendMessage
        /// </summary>
        public string MessageExitHandler;
		
		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		private void OnTriggerEnter(Collider other) {
			if(triggerType == ZoneTriggerTypes.SendMessage) {
				if(messageReceiver == null) {
					Debug.LogError(
						string.Format("UDT: Zone configured to send messages must have a receiver linked. [Zone: '{0}']", gameObject.name)
					);
					
					return;
				}
				
				if(!string.IsNullOrEmpty(MessageEnterHandler)) {
					messageReceiver.SendMessage(
						MessageEnterHandler,
						other,
						(RequireReceivers) ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver
					);
				}
			}
		}
		
		/// <summary>
		/// Raises the trigger stay event.
		/// </summary>
		private void OnTriggerStay(Collider other) {
			if(triggerType == ZoneTriggerTypes.SendMessage) {
				if(messageReceiver == null) {
					Debug.LogError(
						string.Format("UDT: Zone configured to send messages must have a receiver linked. [Zone: '{0}']", gameObject.name)
					);
					
					return;
				}
				
				if(!string.IsNullOrEmpty(MessageStayHandler)) {
					messageReceiver.SendMessage(
						MessageStayHandler,
						other,
						(RequireReceivers) ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver
					);
				}
			}
		}
		
		/// <summary>
		/// Raises the trigger exit event.
		/// </summary>
		private void OnTriggerExit(Collider other) {
			if(triggerType == ZoneTriggerTypes.SendMessage) {
				if(messageReceiver == null) {
					Debug.LogError(
						string.Format("UDT: Zone configured to send messages must have a receiver linked. [Zone: '{0}']", gameObject.name)
					);
					
					return;
				}
				
				if(!string.IsNullOrEmpty(MessageExitHandler)) {
					messageReceiver.SendMessage(
						MessageExitHandler,
						other,
						(RequireReceivers) ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver
					);
				}
			}
		}
    }
}