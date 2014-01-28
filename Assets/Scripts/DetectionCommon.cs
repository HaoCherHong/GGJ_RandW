using UnityEngine;
using System.Collections;

static class DetectionCommon
{
	static public bool ContainmentTest(Bounds container , Bounds obj)
	{

			if(container.center.x + container.extents.x >= obj.center.x + obj.extents.x
			 &&container.center.x - container.extents.x <= obj.center.x - obj.extents.x
			 &&container.center.y + container.extents.y >= obj.center.y + obj.extents.y
			 &&container.center.y - container.extents.y <= obj.center.y - obj.extents.y)
			 return true;
		    else
			 return false;
	
	}

}
