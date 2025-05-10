using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace WasmScripting.Enums
{
	[PublicAPI]
	public static class UnityEventsUtils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasEvent(long flags, UnityEvents @event) => (flags & (long)@event) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long AddEvent(long flags, UnityEvents @event) => flags | (long)@event;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long RemoveEvent(long flags, UnityEvents @event) => flags & ~(long)@event;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToggleEvent(long flags, UnityEvents @event) => flags ^ (long)@event;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long SetEvent(long flags, UnityEvents @event, bool value) => value ? AddEvent(flags, @event) : RemoveEvent(flags, @event);
	}

	[PublicAPI, Flags]
	public enum UnityEvents : long
	{
		Awake = 1L << 0,
		Start = 1L << 1,
		Update = 1L << 2,
		LateUpdate = 1L << 3,
		FixedUpdate = 1L << 4,
		OnEnable = 1L << 5,
		OnDisable = 1L << 6,
		OnDestroy = 1L << 7,
		OnPreCull = 1L << 8,
		OnPreRender = 1L << 9,
		OnPostRender = 1L << 10,
		OnRenderImage = 1L << 11,
		OnRenderObject = 1L << 12,
		OnWillRenderObject = 1L << 13,
		OnBecameVisible = 1L << 14,
		OnBecameInvisible = 1L << 15,
		OnTriggerEnter = 1L << 16,
		OnTriggerEnter2D = 1L << 17,
		OnTriggerStay = 1L << 18,
		OnTriggerStay2D = 1L << 19,
		OnTriggerExit = 1L << 20,
		OnTriggerExit2D = 1L << 21,
		OnParticleTrigger = 1L << 22,
		OnCollisionEnter = 1L << 23,
		OnCollisionEnter2D = 1L << 24,
		OnCollisionStay = 1L << 25,
		OnCollisionStay2D = 1L << 26,
		OnCollisionExit = 1L << 27,
		OnCollisionExit2D = 1L << 28,
		OnControllerColliderHit = 1L << 29,
		OnTransformChildrenChanged = 1L << 30,
		OnTransformParentChanged = 1L << 31,
		OnJointBreak = 1L << 32,
		OnJointBreak2D = 1L << 33,
		OnParticleCollision = 1L << 34,
		OnMouseEnter = 1L << 35,
		OnMouseOver = 1L << 36,
		OnMouseExit = 1L << 37,
		OnMouseDown = 1L << 38,
		OnMouseUp = 1L << 39,
		OnMouseUpAsButton = 1L << 40,
		OnMouseDrag = 1L << 41,
		OnAnimatorMove = 1L << 42,
		OnAnimatorIK = 1L << 43,
		OnAudioFilterRead = 1L << 44,
		// Up to 63: 1L << 63 is max
	}
}
