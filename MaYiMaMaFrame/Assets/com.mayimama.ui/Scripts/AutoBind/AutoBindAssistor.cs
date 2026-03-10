using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Sirenix.OdinInspector;
using MaYiMaMa.Unity.Extensions;

public class AutoBindAssistor : SerializedMonoBehaviour
{
    private const string NewLineCharacter = "\r\n";
    private const string IndentCharacter = "\t";

    [SerializeField]
    [LabelText("脚本存放路径")]
    [FolderPath(ParentFolder = "Assets", AbsolutePath = true)]
    private string _ScriptSavePath;

    [SerializeField]
    [LabelText("脚本类名称")]
    private string _ScriptClassName;

    [SerializeField]
    [LabelText("脚本类命名空间")]
    private string _ScriptClassNamespace;

    [SerializeField]
    [LabelText("UI组件后缀")]
    private Dictionary<Type, string> _NameSuffixDict = new Dictionary<Type, string>()
    {
        { typeof(Image), "_Img" },
        { typeof(Button), "_Btn" },
        { typeof(TextMeshProUGUI), "_Text" },
        { typeof(TMP_InputField), "_InputField" },
        { typeof(Toggle), "_Toggle" },
        { typeof(TMP_Dropdown), "_Dropdown" },
        { typeof(Slider), "_Slider" },
        { typeof(ScrollRect), "_ScrollRect" },
    };

    private bool _IsExistNameSpace = false;
    private List<string> _RetractList = new List<string>();
    private List<AutoBindAssistor> _OtherAutoBindAssistorList = new List<AutoBindAssistor>();

    [Button("千万别点，千万别点，用后删除")]
    private void GenerateCode()
    {
        if (!Directory.Exists(_ScriptSavePath))
        {
            Debug.LogError("脚本存放路径不存在!");
            return;
        }

        if (string.IsNullOrEmpty(_ScriptClassName))
        {
            Debug.LogError("脚本类名称不存在!");
            return;
        }

        _IsExistNameSpace = !string.IsNullOrEmpty(_ScriptClassNamespace);
        transform.GetComponentsInChildren(_OtherAutoBindAssistorList);
        _OtherAutoBindAssistorList.Remove(this);

        GenerateMainCodeFile();
        GenerateUICodeFile();
        GenerateUIListenerCode();
    }

    #region Main
    private void GenerateMainCodeFile()
    {
        string mainFileFullName = $"{_ScriptSavePath}/{_ScriptClassName}.cs";
        if (File.Exists(mainFileFullName))
        {
            return;
        }
        FileStream fileStream = File.Open(mainFileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

        _RetractList.Clear();

        streamWriter.Write(GetNamespaceSourceCode());

        if (_IsExistNameSpace)
        {
            streamWriter.Write($"namespace {_ScriptClassNamespace}{NewLineCharacter}");
            streamWriter.Write($"{{{NewLineCharacter}");
            _RetractList.Add(IndentCharacter);
        }

        streamWriter.Write($"{string.Join("", _RetractList)}public partial class {_ScriptClassName} : MonoBehaviour\r\n");
        streamWriter.Write($"{string.Join("", _RetractList)}{{\r\n");
        _RetractList.Add("\t");

        GenerateNoParamFunction(streamWriter, "Awake", _RetractList);
        GenerateNoParamFunction(streamWriter, "OnEnable", _RetractList);
        GenerateNoParamFunction(streamWriter, "Start", _RetractList);
        // GenerateNoParamFunction(streamWriter, "FixedUpdate", _RetractList);
        GenerateNoParamFunction(streamWriter, "Update", _RetractList);
        // GenerateNoParamFunction(streamWriter, "LateUpdate", _RetractList);
        // GenerateNoParamFunction(streamWriter, "OnGUI", _RetractList);
        GenerateNoParamFunction(streamWriter, "OnDisable", _RetractList);
        GenerateNoParamFunction(streamWriter, "OnDestroy", _RetractList);
        // GenerateNoParamFunction(streamWriter, "OnApplicationQuit", _RetractList);

        _RetractList.Remove("\t");
        streamWriter.Write($"{string.Join("", _RetractList)}}}\r\n");

        if (_IsExistNameSpace)
        {
            streamWriter.Write("}\r\n");
        }

        streamWriter.Flush(); // 确保所有内容都写入文件
        streamWriter.Close();
        fileStream.Dispose(); // 释放文件流资源

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    #endregion

    #region UI
    private void GenerateUICodeFile()
    {
        string uiFileFullName = $"{_ScriptSavePath}/{_ScriptClassName}.UI.cs";
        if (File.Exists(uiFileFullName))
        {
            File.Delete(uiFileFullName);
        }
        FileStream fileStream = File.Open(uiFileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

        _RetractList.Clear();

        streamWriter.Write(GetNamespaceSourceCode());

        if (_IsExistNameSpace)
        {
            streamWriter.Write($"namespace {_ScriptClassNamespace}\r\n");
            streamWriter.Write("{\r\n");
            _RetractList.Add("\t");
        }

        streamWriter.Write($"{string.Join("", _RetractList)}public partial class {_ScriptClassName} : MonoBehaviour\r\n");
        streamWriter.Write($"{string.Join("", _RetractList)}{{\r\n");

        _RetractList.Add("\t");

        // UI组件变量源代码
        streamWriter.WriteLine();
        StringBuilder uiVariableNameSB = new StringBuilder();
        foreach (var pair in _NameSuffixDict)
        {
            StringBuilder content = GetUIVariableNameInfo(pair.Key);
            if (content != null && content.Length > 0)
            {
                uiVariableNameSB.Append(content.ToString());
            }
        }
        streamWriter.Write(uiVariableNameSB.ToString());

        // UI组件查找源代码
        streamWriter.WriteLine();
        StringBuilder uiVariablePathSB = new StringBuilder();
        foreach (var pair in _NameSuffixDict)
        {
            StringBuilder content = GetUIVariablePathInfo(pair.Key, _RetractList.Count + 1);
            if (content != null && content.Length > 0)
            {
                uiVariablePathSB.Append(content.ToString());
            }
        }
        GenerateNoParamFunction(streamWriter, "BindUI", uiVariablePathSB.ToString(), _RetractList);

        // UI事件绑定源代码
        streamWriter.WriteLine();
        StringBuilder uiListenerSB = new StringBuilder();
        foreach (var pair in _NameSuffixDict)
        {
            StringBuilder content = GetUIListenerInfo(pair.Key, _RetractList.Count + 1);
            if (content != null && content.Length > 0)
            {
                uiListenerSB.Append(content.ToString());
            }
        }
        GenerateNoParamFunction(streamWriter, "BindUIListener", uiListenerSB.ToString(), _RetractList);

        streamWriter.Write(GetResetFunctionSourceCode());

        streamWriter.Write($"{string.Join("", _RetractList)}\r\n");

        _RetractList.Remove("\t");
        streamWriter.Write($"{string.Join("", _RetractList)}}}\r\n");

        if (_IsExistNameSpace)
        {
            streamWriter.Write("}\r\n");
        }

        streamWriter.Dispose();
        streamWriter.Close();

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    #endregion

    #region Listener
    private void GenerateUIListenerCode()
    {
        string listenerFileFullName = $"{_ScriptSavePath}/{_ScriptClassName}.Listener.cs";
        if (File.Exists(listenerFileFullName))
        {
            return;
        }
        FileStream fileStream = File.Open(listenerFileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

        _RetractList.Clear();

        streamWriter.Write(GetNamespaceSourceCode());

        if (_IsExistNameSpace)
        {
            streamWriter.Write($"namespace {_ScriptClassNamespace}\r\n");
            streamWriter.Write("{\r\n");
            _RetractList.Add("\t");
        }

        streamWriter.Write($"{string.Join("", _RetractList)}public partial class {_ScriptClassName} : MonoBehaviour\r\n");
        streamWriter.Write($"{string.Join("", _RetractList)}{{\r\n");

        _RetractList.Add("\t");

        // UI事件绑定源代码
        streamWriter.WriteLine();
        StringBuilder uiListenerFuncSB = new StringBuilder();
        foreach (var pair in _NameSuffixDict)
        {
            StringBuilder content = GetUIListenerFuncInfo(pair.Key, _RetractList.Count);
            if (content != null && content.Length > 0)
            {
                uiListenerFuncSB.Append(content.ToString());
            }
        }

        streamWriter.Write(uiListenerFuncSB.ToString());

        _RetractList.Remove("\t");
        streamWriter.Write($"{string.Join("", _RetractList)}}}\r\n");

        if (_IsExistNameSpace)
        {
            streamWriter.Write("}\r\n");
        }

        streamWriter.Dispose();
        streamWriter.Close();

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    #endregion

    #region 获取源码字符串
    private StringBuilder GetUIVariableInfo<T>() where T : MonoBehaviour
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(T), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }
        string retract = string.Join("", _RetractList);
        T[] children = transform.GetComponentsInChildren<T>();
        foreach (T child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (childName.EndsWith(nameSuffix))
            {
                sb.AppendLine($"{retract}private {typeof(T).Name} _{childName};");
            }
        }
        return sb;
    }

    private StringBuilder GetUIVariableNameInfo(Type uiType)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(uiType, out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }
        string retract = string.Join("", _RetractList);
        var children = transform.GetComponentsInChildren(uiType);
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (childName.EndsWith(nameSuffix))
            {
                sb.AppendLine($"{retract}private {uiType.Name} _{childName};");
            }
        }
        return sb;
    }

    private StringBuilder GetUIVariablePathInfo(Type uiType, int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(uiType, out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }
        var children = transform.GetComponentsInChildren(uiType);
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (childName.EndsWith(nameSuffix))
            {
                string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
                string relativePath = GetRelativeTransformPath(transform, child.transform);
                sb.AppendLine($"{indent}_{childName} = transform.Find(\"{relativePath}\")?.GetComponent<{uiType.Name}>();");
                //sb.AppendLine($"{indent}if (_{childName} == null)");
                //sb.AppendLine($"{indent}{{");
                //sb.AppendLine($"{indent}\tDebug.LogError($\"找不到UI组件：{uiType.Name} -> 路径：{relativePath}\", this);");
                //sb.AppendLine($"{indent}}}");
            }
        }
        return sb;
    }

    private StringBuilder GetUIListenerInfo(Type uiType, int indentCount)
    {
        if (uiType == typeof(Button))
        {
            return GetUIListenerInfo4Button(indentCount); 
        }
        else if (uiType == typeof(Toggle))
        {
            return GetUIListenerInfo4Toggle(indentCount);
        }
        else if (uiType == typeof(TMP_Dropdown))
        {
            return GetUIListenerInfo4TMP_Dropdown(indentCount);
        }
        else if (uiType == typeof(TMP_InputField))
        {
            return GetUIListenerInfo4TMP_InputField(indentCount);
        }
        else if (uiType == typeof(Slider))
        {
            return GetUIListenerInfo4Slider(indentCount);
        }
        return null;
    }

    private StringBuilder GetUIListenerFuncInfo(Type uiType, int indentCount)
    {
        if (uiType == typeof(Button))
        {
            return GetUIListenerFuncInfo4Button(indentCount); 
        }
        else if (uiType == typeof(Toggle))
        {
            return GetUIListenerFuncInfo4Toggle(indentCount);
        }
        else if (uiType == typeof(TMP_Dropdown))
        {
            return GetUIListenerFuncInfo4TMP_Dropdown(indentCount);
        }
        else if (uiType == typeof(TMP_InputField))
        {
            return GetUIListenerFuncInfo4TMP_InputField(indentCount);
        }
        else if (uiType == typeof(Slider))
        {
            return GetUIListenerFuncInfo4Slider(indentCount);
        }
        return null;
    }

    private StringBuilder GetUIListenerInfo4Button(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Button), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Button[] children = transform.GetComponentsInChildren<Button>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}_{childName}.AddListener(OnClick_{childName});");
         }
        return sb;
    }

    private StringBuilder GetUIListenerFuncInfo4Button(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Button), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Button[] children = transform.GetComponentsInChildren<Button>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}private void OnClick_{childName}(Button button)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
        }
        return sb;
    }

    private StringBuilder GetUIListenerInfo4Toggle(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Toggle), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Toggle[] children = transform.GetComponentsInChildren<Toggle>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}_{childName}.AddListener(OnValueChanged_{childName});");
        }
        return sb;
    }

    private StringBuilder GetUIListenerFuncInfo4Toggle(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Button), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Toggle [] children = transform.GetComponentsInChildren<Toggle>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}private void OnValueChanged_{childName}(Toggle toggle, bool isOn)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
        }
        return sb;
    }

    private StringBuilder GetUIListenerInfo4TMP_Dropdown(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(TMP_Dropdown), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        TMP_Dropdown[] children = transform.GetComponentsInChildren<TMP_Dropdown>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}_{childName}.AddListener(OnValueChanged_{childName});");
        }
        return sb;
    }

    private StringBuilder GetUIListenerFuncInfo4TMP_Dropdown(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(TMP_Dropdown), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        TMP_Dropdown [] children = transform.GetComponentsInChildren<TMP_Dropdown>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}private void OnValueChanged_{childName}(TMP_Dropdown dropdown, int optionIndex)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
        }
        return sb;
    }

    private StringBuilder GetUIListenerInfo4TMP_InputField(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(TMP_InputField), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        TMP_InputField[] children = transform.GetComponentsInChildren<TMP_InputField>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}_{childName}.AddListener4EndEdit(OnEndEdit_{childName});");
            sb.AppendLine($"{indent}_{childName}.AddListener4ValueChanged(OnValueChanged_{childName});");
            sb.AppendLine($"{indent}_{childName}.AddListener4OnSelect(OnSelect_{childName});");
            sb.AppendLine($"{indent}_{childName}.AddListener4OnDeselect(OnDeselect_{childName});");
        }
        return sb;
    }

    private StringBuilder GetUIListenerFuncInfo4TMP_InputField(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(TMP_InputField), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        TMP_InputField[] children = transform.GetComponentsInChildren<TMP_InputField>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}private void OnEndEdit_{childName}(TMP_InputField inputField, string content)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
            sb.AppendLine($"{indent}private void OnValueChanged_{childName}(TMP_InputField inputField, string content)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
            sb.AppendLine($"{indent}private void OnSelect_{childName}(TMP_InputField inputField, string content)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
            sb.AppendLine($"{indent}private void OnDeselect_{childName}(TMP_InputField inputField, string content)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
        }
        return sb;
    }

    private StringBuilder GetUIListenerInfo4Slider(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Slider), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Slider[] children = transform.GetComponentsInChildren<Slider>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}{childName}.AddListener(OnValueChanged_{childName});");
        }
        return sb;
    }

    private StringBuilder GetUIListenerFuncInfo4Slider(int indentCount)
    {
        StringBuilder sb = new StringBuilder();
        _NameSuffixDict.TryGetValue(typeof(Slider), out string nameSuffix);
        if (string.IsNullOrEmpty(nameSuffix))
        {
            return sb;
        }

        string indent = string.Join("", Enumerable.Repeat(IndentCharacter, indentCount));
        Slider [] children = transform.GetComponentsInChildren<Slider>();
        foreach (var child in children)
        {
            if (CheckDescendant4OtherAutoBindAssistor(child.transform))
            {
                continue;
            }
            string childName = child.name.Trim();
            if (!childName.EndsWith(nameSuffix))
            {
                continue;
            }
            sb.AppendLine($"{indent}private void OnValueChanged_{childName}(Slider slider, float value)\r\n{indent}{{\r\n\r\n{indent}}}\r\n");
        }
        return sb;
    }
    #endregion

    private void GenerateNoParamFunction(StreamWriter streamWriter, string functionName, List<string> retractList)
    {
        streamWriter.Write($"{string.Join("", _RetractList)}private void {functionName}(){NewLineCharacter}");
        streamWriter.Write($"{string.Join("", _RetractList)}{{{NewLineCharacter}");
        streamWriter.Write($"{NewLineCharacter}");
        streamWriter.Write($"{string.Join("", _RetractList)}}}{NewLineCharacter}");
        streamWriter.Write($"{NewLineCharacter}");
    }

    private void GenerateNoParamFunction(StreamWriter streamWriter, string functionName, string functionCode, List<string> retractList)
    {
        streamWriter.Write($"{string.Join("", _RetractList)}private void {functionName}(){NewLineCharacter}");
        streamWriter.Write($"{string.Join("", _RetractList)}{{{NewLineCharacter}");
        streamWriter.Write($"{functionCode}");
        streamWriter.Write($"{NewLineCharacter}");
        streamWriter.Write($"{string.Join("", _RetractList)}}}{NewLineCharacter}");
    }

    private bool CheckDescendant4OtherAutoBindAssistor(Transform child)
    {
        for(int i = 0; i < _OtherAutoBindAssistorList.Count; i++)
        {
            if(_OtherAutoBindAssistorList[i].transform.IsDescendantOf(child))
            {
                return true;
            }
        }
        return false;
    }

    private string GetNamespaceSourceCode()
    {
        string namespaceInfo = "using System; \r\n" +
            "using System.Collections; \r\n" +
            "using System.Collections.Generic; \r\n" +
            "using UnityEngine; \r\n" +
            "using UnityEngine.UI; \r\n" +
            "using TMPro; \r\n" +
            "using Sirenix.OdinInspector; \r\n\r\n";
        return namespaceInfo;
    }

    private string GetRelativeTransformPath(Transform parentTransform, Transform childTransform)
    {
        if (parentTransform == childTransform)
        {
            return string.Empty;
        }

        StringBuilder path = new StringBuilder();
        Transform current = childTransform;
        while (current != null && current != parentTransform)
        {
            if (path.Length > 0)
            {
                path.Insert(0, $"{current.name}/");
            }
            else
            {
                path.Append(current.name);
            }
            current = current.parent;
        }

        // 如果遍历到根物体还没找到父物体，说明不是直接子层级，返回完整路径（或空）
        if (current == null)
        {
            Debug.LogWarning($"子物体 {childTransform.name} 不是 {parentTransform.name} 的后代", parentTransform);
            return childTransform.name; // 降级返回直接名称（避免路径错误）
        }

        // 移除末尾可能的斜杠
        return path.ToString().TrimEnd('/');
    }

    private string GetResetFunctionSourceCode()
    {
        return @"
        #if UNITY_EDITOR
        private void Reset()
        {
            System.Type type = this.GetType();
            System.Reflection.FieldInfo[] fieldInfos = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if(fieldInfos == null || fieldInfos.Length <= 0)
            {
                return;
            }
            for(int i = 0; i < fieldInfos.Length; i++)
            {
                object[] attributes = fieldInfos[i].GetCustomAttributes(typeof(FoldoutGroupAttribute), false);
                if(attributes == null || attributes.Length <= 0)
                {
                    continue;
                }
                FoldoutGroupAttribute attribute = attributes[0] as FoldoutGroupAttribute; 
                if(attribute.GroupName != ""自动绑定"")
                {
                    continue;
                }
                System.Type componentType = fieldInfos[i].FieldType;
                if (!componentType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    continue;
                }
                string childName = fieldInfos[i].Name.TrimStart('_');
                Component[] components = transform.GetComponentsInChildren(componentType, true);
                for (int j = 0; j < components.Length; j++)
                {
                    if (components[j].name == childName)
                    {
                        fieldInfos[i].SetValue(this, components[j]);
                        break;
                    }
                }
            }
        }
        #endif";
    }
}
