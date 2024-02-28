using System;
using System.Collections.Generic;
using Cagri.Scripts.GenericSystems.Pathfinding;
using UnityEngine;

namespace Cagri.Scripts.GenericSystems.GridMap
{
   public class Grid<TGridObject>
   {
      
      public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanced;
      public class OnGridValueChangedEventArgs:EventArgs
      {
         public int x;
         public int y;
      }
   
      
      private int _widht;
      private int _height;
      private float _cellSize;
      private Vector3 _originPos;
      private TGridObject[,] _gridArray;
      private TextMesh[,] debugTextArray;
     
      public Grid(int width, int height,float cellSize,Vector3 originPos,Func<Grid<TGridObject>,int,int,TGridObject> createGridObject)
      {
         _height = height;
         _widht = width;
         _cellSize = cellSize;
         _originPos = originPos;
         
         _gridArray = new TGridObject[width, height];

         for (int i = 0; i < _gridArray.GetLength(0); i++)
         {
            for (int j = 0; j < _gridArray.GetLength(1); j++)
            {
               _gridArray[i, j] = createGridObject(this,i,j);
            }
         }
         
         bool showDebug = GameManager.instance.showGrid;
         if (showDebug)
         {
            debugTextArray = new TextMesh[width, height];
      
            for (int i = 0; i < _gridArray.GetLength(0); i++)
            {
               for (int j = 0; j < _gridArray.GetLength(1); j++)
               {
                  /*debugTextArray[i,j]=Utils.Utils.CreateWorldText(_gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j)+new Vector3(cellSize,cellSize)*.5f, 4, Color.white,
                     TextAnchor.MiddleCenter);*/
                  Debug.DrawLine(GetWorldPosition(i,j),GetWorldPosition(i,j+1),Color.white,100f);
                  Debug.DrawLine(GetWorldPosition(i,j),GetWorldPosition(i+1,j),Color.white,100f);
               }
            }
            Debug.DrawLine(GetWorldPosition(0,this._height),GetWorldPosition(width,height),Color.white,100f);
            Debug.DrawLine(GetWorldPosition(width,0),GetWorldPosition(width,height),Color.white,100f);
            OnGridValueChanced += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
               //debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
         }
         
      }
      
      public int GetWidth()
      {
         return _widht;
      }

      public int GetHeight()
      {
         return _height;
      }

      public float GetCellSize()
      {
         return _cellSize;
      }

      public Vector3 GetWorldPosition(int x, int y)
      {
         return new Vector3(x, y) * _cellSize+_originPos;
      }

      public void GetXY(Vector3 worldPosition, out int x, out int y)
      {
         x = Mathf.FloorToInt((worldPosition-_originPos).x / _cellSize);
         y = Mathf.FloorToInt((worldPosition-_originPos).y / _cellSize);
      }

      public void SetGridObject(int x, int y, TGridObject targetValue)
      {
         if (x >= 0 && y >= 0 && x < _widht && y < _height)
         {
            debugTextArray[x, y].text = targetValue==null?"X":_gridArray[x, y]?.ToString();
            if (OnGridValueChanced != null) OnGridValueChanced(this, new OnGridValueChangedEventArgs() { x = targetValue==null?0:x, y = targetValue==null?0:y });
         }
      }

      public void SetGridObject(Vector3 worldPosition, TGridObject value)
      {
         int x, y;
         GetXY(worldPosition, out x, out y);
         SetGridObject(x, y, value);
      }

      public void TriggerGridObjectChanged(int x , int y)
      {
         if (OnGridValueChanced != null) OnGridValueChanced(this, new OnGridValueChangedEventArgs() { x = x, y = y });
      }

      public Vector3 PositionInTheMiddleOfGrid(int x , int y)
      {
         return GetWorldPosition(x, y) + Vector3.up * .5f +Vector3.right * .5f;
      }

      public TGridObject GetGridObject(int x, int y)
      {
         if (x >= 0 && y >= 0 && x < _widht && y < _height)
         {
            return _gridArray[x, y];
         }
         else
         {
            return default(TGridObject);
         }
      }

      public TGridObject GetGridObject(Vector3 worldPosition)
      {
         int x, y;
         GetXY(worldPosition,out x,out y);
         return GetGridObject(x, y);
      }
      
   }
}
