﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoZ.PA
{
    /// <summary>
    /// Base class for all tools
    /// </summary>
    internal class PATool : ImmediateModeElement
    {
        public PACanvas Canvas { get; private set; }

        public float DrawThreshold = 0.0f;

        public PATool(PACanvas canvas)
        {
            Canvas = canvas;

            visible = false;
            pickingMode = PickingMode.Ignore;

            this.StretchToParentSize();
        }

        public bool IsEnabledDuringPlay { get; protected set; }

        public bool IsDrawing => Canvas.IsDrawing;

        public Vector2Int CanvasToTexture(Vector2Int v) =>
            new Vector2Int(v.x, Canvas.ImageHeight - 1 - v.y);

        public RectInt CanvasToTexture(RectInt r) =>
            new RectInt(r.xMin, Canvas.ImageHeight - r.yMin - r.height, r.width, r.height);

        /// <summary>
        /// Load the icon for the tool
        /// </summary>
        /// <returns></returns>
        public Texture2D LoadIcon () =>
            AssetDatabase.LoadAssetAtPath<Texture2D>($"{GetType().Name}.psd");

        /// <summary>
        /// Called when a mouse button is pressed with the tool active
        /// </summary>
        public virtual void OnMouseDown(PAMouseEvent evt) { }

        /// <summary>
        /// Called when a mouse button is released with the tool active
        /// </summary>
        public virtual void OnMouseUp(PAMouseEvent evt) { }

        /// <summary>
        /// Called when the mouse moves while the tool is active
        /// </summary>
        public virtual void OnMouseMove(PAMouseEvent evt) { }

        /// <summary>
        /// Set the cursor to the appropriate cursor for the given position
        /// </summary>
        public virtual void SetCursor (Vector2 canvasPosition)
        {
            Canvas.SetCursor(MouseCursor.Arrow);
        }

        /// <summary>
        /// Called when the tool becomes the active tool
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Called when the tool is no longer the active tool
        /// </summary>
        public virtual void OnDisable() { }

        public virtual void OnDrawStart (PADrawEvent evt) { }

        public virtual void OnDrawContinue (PADrawEvent evt) { }

        public virtual void OnDrawEnd (PADrawEvent evt, bool cancelled) { }

        sealed protected override void ImmediateRepaint()
        {
            GUI.BeginClip(contentRect);
            OnRepaint();
            GUI.EndClip();            
        }

        protected virtual void OnRepaint() { }

        public virtual bool OnKeyDown(PAKeyEvent evt) => true;
    }
}
