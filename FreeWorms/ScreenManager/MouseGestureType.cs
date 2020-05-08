using System;

namespace FreeWorms
{
	[Flags]
	public enum MouseGestureType
	{
		None = 0,
		LeftClick = 1,
		DragComplete = 2,		        
		FreeDrag = 4,
		Move = 8,
	}
}