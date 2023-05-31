using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OGIS.UI
{
    public abstract class AOceanEntity
    {
        public EntityType EntityType { get; set; }
        public string Explain { get; set; }
    }

    public enum EntityType
    {
        Mark = 1,
        Tool = 2,
        FeatureFiled = 3,
        Demarcate = 4,
        LineBufferSet = 5,
        DrawGeometry = 6,
        EditGeometry = 7,
        LayerTree = 8,
        LayerTreeCache = 9,
        FeatureQuery = 10,
        LineEdit = 11,
        QueryEntity = 12,
        IElementNode = 13,
        ArcSet = 14,
        SystemSet = 15,
    }
    public delegate void CallBackEvent(AOceanEntity entity);
}
