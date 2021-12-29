﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class Mesh2DColliderMaker : MonoBehaviour {

	/// <summary>
	/// 定义一个结构体，用来表示边
	/// </summary>
	struct Edge2D
	{

		public Vector2 a;
		public Vector2 b;

		public override bool Equals(object obj)
		{
			if (obj is Edge2D)
			{
				var edge = (Edge2D)obj;
				//An edge is equal regardless of which order it's points are in
				return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
			}

			return false;

		}

		public override int GetHashCode()
		{
			return a.GetHashCode() ^ b.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[" + a.x + "," + a.y + "->" + b.x + "," + b.y + "]");
		}

	}

	MeshFilter filter;
	PolygonCollider2D polyCollider;


	void Start() 
	{
		filter = GetComponent<MeshFilter>();
		polyCollider = GetComponent<PolygonCollider2D>();
		CreatePolygon2DColliderPoints();
	}

	void Update()
	{		
			
	}


	public void CreatePolygon2DColliderPoints()
	{
		var edges = BuildEdgesFromMesh();
		var paths = BuildColliderPaths(edges);
		ApplyPathsToPolygonCollider(paths);
	}
	
	/// <summary>
	/// 对多边形碰撞体设置路径
	/// </summary>
	/// <param name="paths"></param>
	private void ApplyPathsToPolygonCollider(List<Vector2[]> paths) 
	{
		if (paths == null)
			return;

		polyCollider.pathCount = paths.Count;
		for (int i = 0; i < paths.Count; i++) {
			var path = paths [i];
			polyCollider.SetPath(i, path);
		}
	}

	/// <summary>
	/// 通过mesh去找所有的边，存到一个字典里
	/// 如果 int = 1 则证明是边缘的边，如果 int = 2 则证明则是公用的（不是边缘）
	/// </summary>
	/// <returns>返回存Edge2D的字典</returns>
	private Dictionary<Edge2D, int> BuildEdgesFromMesh() 
	{
		var mesh = filter.sharedMesh;

		if (mesh == null)
			return null;

		var verts = mesh.vertices;
		var tris = mesh.triangles;
		var edges = new Dictionary<Edge2D, int>();

		for (int i = 0; i < tris.Length - 2; i += 3) {

			var faceVert1 = verts[tris[i]];
			var faceVert2 = verts[tris[i + 1]];
			var faceVert3 = verts[tris[i + 2]];

			Edge2D[] faceEdges;
			faceEdges = new Edge2D[] {
				new Edge2D{ a = faceVert1, b = faceVert2 },
				new Edge2D{ a = faceVert2, b = faceVert3 },
				new Edge2D{ a = faceVert3, b = faceVert1 },
			};

			foreach(var edge in faceEdges) {
				if (edges.ContainsKey(edge))
					edges[edge]++;
				else
					edges[edge] = 1;
			}
		}

		return edges;
	}

	private List<Vector2[]> BuildColliderPaths(Dictionary<Edge2D, int> allEdges) 
	{

		if (allEdges == null)
			return null;	

		var outerEdges = GetOuterEdges(allEdges);

		var paths = new List<List<Edge2D>>();
		List<Edge2D> path = null;
		
		while (outerEdges.Count > 0) {
			
			if (path == null) {
				path = new List<Edge2D>();
				path.Add (outerEdges[0]);
				paths.Add (path);

				outerEdges.RemoveAt(0);
			}

			bool foundAtLeastOneEdge = false;

			int i = 0;
			while (i < outerEdges.Count) {
				var edge = outerEdges [i];
				bool removeEdgeFromOuter = false;

				if (edge.b == path[0].a) {
					path.Insert (0, edge);
					removeEdgeFromOuter = true;
				}
				else if (edge.a == path[path.Count - 1].b) {
					path.Add(edge);
					removeEdgeFromOuter = true;
				}

				if (removeEdgeFromOuter) {
					foundAtLeastOneEdge = true;
					outerEdges.RemoveAt(i);
				} else
					i++;
			}

			if (!foundAtLeastOneEdge)
				path = null;
			
		}
		
		var cleanedPaths = new List<Vector2[]>();
		
		foreach(var builtPath in paths) {
			var coords = new List<Vector2>();
			
			foreach(var edge in builtPath)
				coords.Add (edge.a);
			
			cleanedPaths.Add (CoordinatesCleaned(coords));
		}		
		
		return cleanedPaths;
	}

	/// <summary>
	/// 取到边缘的边（即字典中int = 1的值）
	/// </summary>
	/// <param name="allEdges"></param>
	/// <returns>边缘的边</returns>
	private List<Edge2D> GetOuterEdges(Dictionary<Edge2D, int> allEdges)
	{
		var outerEdges = new List<Edge2D>();

		foreach (var edge in allEdges.Keys)
		{
			var numSharedFaces = allEdges[edge];
			if (numSharedFaces == 1)
				outerEdges.Add(edge);
		}

		return outerEdges;
	}

	private bool CoordinatesFormLine(Vector2 a, Vector2 b, Vector2 c)
	{
		//If the area of a triangle created from three points is zero, they must be in a line.
		float area = a.x * ( b.y - c.y ) + 
			b.x * ( c.y - a.y ) + 
				c.x * ( a.y - b.y );
		
		return Mathf.Approximately(area, 0f);
		
	}
	
	private Vector2[] CoordinatesCleaned(List<Vector2> coordinates) 
	{
		List<Vector2> coordinatesCleaned = new List<Vector2> ();
		coordinatesCleaned.Add (coordinates [0]);
		
		var lastAddedIndex = 0;
		
		for (int i = 1; i < coordinates.Count; i++) {
			
			var coordinate = coordinates [i];
			
			Vector2 lastAddedCoordinate = coordinates [lastAddedIndex];
			Vector2 nextCoordinate = (i + 1 >= coordinates.Count) ? coordinates[0] : coordinates [i + 1];
			
			if (!CoordinatesFormLine(lastAddedCoordinate, coordinate, nextCoordinate)) {
				
				coordinatesCleaned.Add (coordinate);
				lastAddedIndex = i;							
			}
			
		}
		
		return coordinatesCleaned.ToArray ();
		
	}

}
