using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaYiMaMa.Unity.Extensions
{
    public static class MMTransformExtensions
    {
        #region 基础 
        public static int GetDepth(this Transform transform)
        {
            int count = 1;
            while (transform = transform.parent)
            {
                count++;
            }
            return count;
        }

        public static string GetFullName(this Transform transform)
        {
            string fullName = transform.name;
            while (transform = transform.parent)
            {
                fullName = $"{transform.name}/{fullName}";
            }
            return fullName;
        }

        public static string GetFullName2RootTransform(this Transform transform, Transform rootTransform)
        {
            string fullName = transform.name;
            while ((transform = transform.parent) && (rootTransform != transform))
            {
                fullName = $"{transform.name}/{fullName}";
            }
            return fullName;
        }

        public static string GetHierarchyPath(this Transform transform)
        {
            if (transform == null) return string.Empty;
            if (transform.parent == null) return transform.name;
            return GetHierarchyPath(transform.parent) + "/" + transform.name;
        }

        public static bool PathMatch(this Transform transform, string requiredPath)
        {
            if (transform == null) return false;
            if (transform.name.Contains("/"))
            {
                var fullPath = GetHierarchyPath(transform);
                return fullPath.EndsWith(requiredPath);
            }
            else
            {
                return string.Equals(transform.name, requiredPath);
            }
        }

        public static void DestroyChilds(this Transform transform)
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public static void HideChilds(this Transform transform)
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        #endregion

        #region 获取组件
        public static void GetComponentAtPath<T>(
            this Transform transform,
            string path,
            out T foundComponent) where T : Component
        {
            Transform t = null;
            if (path == null)
            {
                foreach (Transform child in transform)
                {
                    T comp = child.GetComponent<T>();
                    if (comp != null)
                    {
                        foundComponent = comp;
                        return;
                    }
                }
            }
            else
                t = transform.Find(path);

            if (t == null)
                foundComponent = default(T);
            else
                foundComponent = t.GetComponent<T>();
        }

        public static T GetComponentAtPath<T>(
            this Transform transform,
            string path) where T : Component
        {
            T foundComponent;
            transform.GetComponentAtPath(path, out foundComponent);

            return foundComponent;
        }
        #endregion

        #region 查找
        // 根据name和tag在指定的Transform下查找对应的GameObject
        public static GameObject SearchHierarchy(this Transform transform, string goName, string tag)
        {
            if (transform == null) 
                return null;
            if (string.Equals(transform.name, goName) && 
                (string.IsNullOrEmpty(tag) || string.Equals(transform.tag, tag))) 
                return transform.gameObject;
            foreach (Transform child in transform)
            {
                var result = SearchHierarchy(child, goName, tag);
                if (result != null) return result;
            }
            return null;
        }

        // 根据name和tag在指定的Transform集合列表下查找对应的GameObject
        public static GameObject SearchHierarchy(string goName, string tag, Transform[] rootTransforms)
        {
            if (rootTransforms == null) return null;
            for (int i = 0; i < rootTransforms.Length; i++)
            {
                var result = SearchHierarchy(rootTransforms[i].transform, goName, tag);
                if (result != null) return result;
            }
            return null;
        }

        public static T GetComponentInParent<T>(this Transform transform, bool includeInactive = true) where T : Component
        {
            var here = transform;
            T result = null;
            while (here && !result)
            {
                if (includeInactive || here.gameObject.activeSelf)
                {
                    result = here.GetComponent<T>();
                }
                here = here.parent;
            }
            return result;
        }

        public static T GetComponentInParentsOrChildren<T>(this Transform transform, bool includeInactive = true)
            where T : Component
        {
            T result = transform.GetComponentInParent<T>(includeInactive);
            if (result == null)
            {
                result = transform.GetComponentInChildren<T>(includeInactive);
            }

            return result;
        }
        #endregion

        #region Descendant, 后裔，子孙 
        // 判定otherTransform是否是transform的子孙
        public static bool IsDescendantOf(this Transform transform, Transform otherTransform)
        {
            IList<Transform> children = transform.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child == otherTransform)
                {
                    return true;
                }
            }

            int childCount = children.Count;
            for (int i = 0; i < childCount; ++i)
            {
                var child = children[i];
                if (child.IsDescendantOf(otherTransform))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            int childCount = transform.childCount;
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
            return children;
        }

        public static List<Transform> GetChildrenExcept(this Transform transform, Func<Transform, bool> except)
        {
            int childCount = transform.childCount;
            var children = new List<Transform>();
            for (int i = 0; i < childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (except == null || !except(child))
                {
                    children.Add(child);
                }
            }
            return children;
        }

        public static List<Transform> GetDescendants(this Transform transform)
        {
            return transform.GetDescendantsExcept(null);
        }

        public static List<Transform> GetDescendantsExcept(this Transform transform, Func<Transform, bool> except)
        {
            IList<Transform> children = except == null ?
                transform.GetChildren() : transform.GetChildrenExcept(except);
            List<Transform> descendants = new List<Transform>();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (except == null || !except(child))
                {
                    descendants.Add(child);
                }
            }

            int childCount = children.Count;
            for (int i = 0; i < childCount; ++i)
            {
                var child = children[i];
                var cDescendants = child.GetDescendantsExcept(except);
                descendants.AddRange(cDescendants);
            }

            return descendants;
        }

        public static void GetDescendantsAndRelativePaths(this Transform transform,
            ref Dictionary<Transform, string> mapDescendantToPath)
        {
            transform.GetDescendantsAndRelativePaths("", ref mapDescendantToPath);
        }

        public static void GetDescendantsAndRelativePaths(this Transform transform, string currentPath,
            ref Dictionary<Transform, string> mapDescendantToPath)
        {
            List<Transform> children = transform.GetChildren();
            int childCount = children.Count;
            string path;
            for (int i = 0; i < childCount; ++i)
            {
                var ch = children[i];
                path = currentPath + "/" + ch.name;
                mapDescendantToPath[ch] = path;
                ch.GetDescendantsAndRelativePaths(path, ref mapDescendantToPath);
            }
        }
        #endregion

        #region Ancestor，祖先，祖宗
        // 获取Hierarchy层级中祖先层级数量
        public static int GetNumberOfAncestors(this Transform transform)
        {
            int num = 0;
            while (transform = transform.parent)
            {
                ++num;
            }
            return num;
        }

        // 判定transform是否是otherTransform的祖先
        public static bool IsAncestorOf(this Transform transform, Transform otherTransform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException("ParentTransform");
            }
            if (otherTransform == null)
            {
                throw new ArgumentNullException("ChildTransform");
            }
            while (otherTransform = otherTransform.parent)
            {
                if (transform == otherTransform)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Position处理
        public static void ResetLocal(this Transform transform)
        {
            transform.ResetLocalPosition();
            transform.ResetLocalRotation();
            transform.ResetLocalScale();
        }

        public static void ResetLocalPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void ResetLocalRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        public static void ResetLocalScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }

        public static void SetLocalPositionX(this Transform transform, float x)
        {
            var pos = transform.localPosition;
            pos.x = x;
            transform.localPosition = pos;
        }

        public static void SetLocalPositionY(this Transform transform, float y)
        {
            var pos = transform.localPosition;
            pos.y = y;
            transform.localPosition = pos;
        }

        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;
        }
        #endregion

        #region Layer
        public static void ChangeLayersRecursively(this Transform trans, string name)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);
            foreach (Transform child in trans)
            {
                child.ChangeLayersRecursively(name);
            }
        }
        #endregion

    }
}

