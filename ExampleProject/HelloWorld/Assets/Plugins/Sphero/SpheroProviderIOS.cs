using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

#if UNITY_IPHONE

public class SpheroProviderIOS : SpheroProvider {
	
	/*
	 * Get the Robot Provider for Android 
	 */
	public SpheroProviderIOS() : base() {
		m_PairedSpheros = new Sphero[1];
		m_PairedSpheros[0] = new SpheroIOS();
		// DO NOT CHANGE THIS UNTIL MULTIPLE ROBOTS ARE USED ON iOS (if ever)
		m_PairedSpheros[0].DeviceInfo.UniqueId = "Robot";
		m_PairedSpheros[0].DeviceInfo.Name = "Robot";
		// Sign up for notifications on Sphero connections
		SpheroDeviceMessenger.SharedInstance.NotificationReceived += ReceiveNotificationMessage;
	}
	
	/*
	 * Callback to receive connection notifications 
	 */
	private void ReceiveNotificationMessage(object sender, SpheroDeviceMessenger.MessengerEventArgs eventArgs)
	{
		SpheroDeviceNotification message = (SpheroDeviceNotification)eventArgs.Message;
		Sphero notifiedSphero = m_PairedSpheros[0];
		if( message.NotificationType == SpheroDeviceNotification.SpheroNotificationType.CONNECTED ) {
			notifiedSphero.ConnectionState = Sphero.Connection_State.Connected;
			// Consider setting bluetooth device info here
		}
		else if( message.NotificationType == SpheroDeviceNotification.SpheroNotificationType.DISCONNECTED ) {
			notifiedSphero.ConnectionState = Sphero.Connection_State.Disconnected;
		}
		else if( message.NotificationType == SpheroDeviceNotification.SpheroNotificationType.CONNECTION_FAILED ) {
			notifiedSphero.ConnectionState = Sphero.Connection_State.Failed;
		}
	}
	
	/*
	 * Call to properly disconnect Spheros.  Call in OnApplicationPause 
	 */
	override public void DisconnectSpheros() {
		SpheroBridge.DisconnectRobots();
	}
	
	/* Connect to a robot at index */
	override public void Connect(int index) {
		// Don't try to connect to multiple Spheros at once
		SpheroBridge.SetupRobotConnection();
	}	
	
	/*
	 * Get a Sphero object from the unique Sphero id 
	 * Returns nulls if no Spheros were found with that particular id
	 */
	override public Sphero GetSphero(string spheroId) {
		if( m_PairedSpheros.Length > 0 ) return m_PairedSpheros[0];
		return null; 
	}
	
	/* Need to call this to get the robot objects that are paired from Android */
	override public void FindRobots() {}
	/* Check if bluetooth is on */
	override public bool IsAdapterEnabled() { return true; }
}

#endif
