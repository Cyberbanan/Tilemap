using UnityEngine;
using System.Collections.Generic;

namespace toinfiniityandbeyond.Tilemapping
{
	
	[System.Serializable]
	public class Fill : ScriptableTool
	{
		//An empty constructor
		public Fill () : base()
		{

		}
		//Sets the shortcut key to 'F'
		public override KeyCode Shortcut
		{
			get { return KeyCode.F; }
		}
		//Sets the tooltip description
		public override string Description
		{
			get { return "A flood fill tool"; }
		}
		//Called when the left mouse button is held down
		public override bool OnClick (Point point, ScriptableTile tile, TileMap map)
		{
			return false;
		}
		//Called when the left mouse button is initially held down
		public override bool OnClickDown (Point point, ScriptableTile tile, TileMap map)
		{
			//Return if the tilemap is null/empty
			if (map == null)
				return false;
			//Gets the tile where you clicked
			ScriptableTile start = map.GetTileAt(point);
			//Return if ther tile specified is null
			if (tile == null)
				return false;
			//The queue of points that need to be changed to the specified tile
			Queue<Point> open = new Queue<Point> ();
			//The list of points already changed to the specified tile
			List<Point> closed = new List<Point> ();
			//A number larger than the amount of tiles available
			int maxLoops = map.Width * map.Height * 10;
			
			//Add the specified point to the open queue
			open.Enqueue (point);
			//As long as there are items in the queue, keep this running
			while(open.Count > 0) 
			{
				//Decrement the max loops value
				maxLoops--;
				//If we've executed this code more than the max loops then we've done something wrong :/
				if (maxLoops <= 0) {
					Debug.LogError ("Fill tool, max loops reached!");
					return false;
				}

				Point p = open.Dequeue ();
				if (closed.Contains (p))
					continue;

				closed.Add (p);
				ScriptableTile t = map.GetTileAt(p);

				if (!map.IsInBounds (p) || t != start)
					continue;
			
				open.Enqueue (p.Up);
				open.Enqueue (p.Right);
				open.Enqueue (p.Down);
				open.Enqueue (p.Left);

				map.SetTileAt (p, tile);
			}

			return true;
		}

		public override bool OnClickUp (Point point, ScriptableTile tile, TileMap map)
		{
			map.UpdateTileMap ();
			return false;
		}
	}
}
