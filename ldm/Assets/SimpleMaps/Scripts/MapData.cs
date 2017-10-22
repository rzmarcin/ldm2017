using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleMaps {
    [System.Serializable]
    public class MapData : MonoBehaviour {
        [SerializeField] Sprite data, view;
        [SerializeField] float unitsPerPixel = 10;

        SpriteRenderer rendererData;
        SpriteRenderer rendererView;

        Texture2D mapData { get { return data.texture; } }

        public int Width { get { return mapData.width; } }
        public int Height { get { return mapData.height; } }

        public Color this[Vector3 v] { get { return this[v.x, v.y]; } }
        public Color this[float x, float y] {
            get {
                return PositionToPixelColor(x, y);
            }
        }

        Color PositionToPixelColor(float px, float py) {
            int x = Mathf.FloorToInt(px / unitsPerPixel),
                y = Mathf.FloorToInt(py / unitsPerPixel);

            if (x < 0 || x >= Width || y < 0 || y >= Height) {
                return Color.clear;
            } else {
                return mapData.GetPixel(x, y);
            }
        }

        public void OnValidate() {

            if (rendererData == null) {
                IEnumerable<SpriteRenderer> notView = GetComponentsInChildren<SpriteRenderer>().Where(sr => sr != rendererView);
                if (notView.Count() > 0) {
                    rendererData = notView.First();
                    rendererData.gameObject.name = "map-data-sprite";
                } else {
                    GameObject go = new GameObject("map-data-sprite");
                    go.transform.SetParent(transform);
                    rendererData = go.AddComponent<SpriteRenderer>();
                }
            }
            if (rendererView == null) {
                IEnumerable<SpriteRenderer> notData = GetComponentsInChildren<SpriteRenderer>().Where(sr => sr != rendererView);
                if (notData.Count() > 0) {
                    rendererView = notData.First();
                } else {
                    GameObject go = new GameObject("map-view-sprite");
                    rendererView.gameObject.name = "map-view-sprite";
                    go.transform.SetParent(transform);
                    rendererView = go.AddComponent<SpriteRenderer>();
                }
            }

            rendererData.sprite = data;
            rendererData.sortingOrder = 1;
            if (data != null) {
                rendererData.transform.localScale = Vector3.one * unitsPerPixel;// / (float)map.pixelsPerUnit;
                rendererData.transform.localPosition = new Vector3(Width / 2f, Height / 2f) * rendererData.transform.localScale.x;
            }

            rendererView.sprite = view;
            rendererView.sortingOrder = 0;
            if (view != null) {
                rendererView.transform.localScale = Vector3.one * unitsPerPixel;// / (float)map.pixelsPerUnit;
                rendererView.transform.localPosition = new Vector3(view.texture.width / 2f, view.texture.height / 2f) * rendererData.transform.localScale.x;
            }
        }
    }

    public static class Extension {
        public static float GetValue(this Vector4 c, Channel channel) {
            switch (channel) {
                case Channel.R:
                    return c.x;
                case Channel.G:
                    return c.y;
                case Channel.B:
                    return c.z;
                default:
                    return c.w;
            }
        }
    }

    public enum Channel {
        R,
        G,
        B,
        A,
    }
}