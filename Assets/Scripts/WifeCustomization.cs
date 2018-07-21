using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WifeCustomization : Singleton<WifeCustomization>
{
    public string suffixMin = "Min";
    public string suffixMax = "Max";
    public SkinnedMeshRenderer target;
    SkinnedMeshRenderer skm;
    Mesh mesh;
    Dictionary<string, BlendShape> BlendShapeDatabase = new Dictionary<string, BlendShape>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        skm = target;
        mesh = skm.sharedMesh;
        SaveBlendShapeDatabase();
    }

    #region 存储数据

    void SaveBlendShapeDatabase()
    {
        List<string> BlendShapeNames = Enumerable.Range(0, mesh.blendShapeCount).Select(x => mesh.GetBlendShapeName(x)).ToList();
        for (int i = 0; BlendShapeNames.Count > 0;)
        {
            string noSuffix, altSuffix;
            noSuffix = BlendShapeNames[i].TrimEnd(suffixMax.ToCharArray()).TrimEnd(suffixMin.ToCharArray()).Trim();
            string positiveName = string.Empty;
            string negativeName = string.Empty;
            int postiveIndex = -1;
            int negativeIndex = -1;
            bool exist = false;

            //后缀是max
            if (BlendShapeNames[i].EndsWith(suffixMax))
            {
                positiveName = BlendShapeNames[i];
                altSuffix = noSuffix + "" + suffixMin;

                if (BlendShapeNames.Contains(altSuffix))
                    exist = true;

                postiveIndex = mesh.GetBlendShapeIndex(positiveName);

                if (exist)
                {
                    negativeName = altSuffix;
                    negativeIndex = mesh.GetBlendShapeIndex(negativeName);
                }
            }
            //后缀是min结尾
            else if (BlendShapeNames[i].EndsWith(suffixMin))
            {
                negativeName = BlendShapeNames[i];
                altSuffix = noSuffix + "" + suffixMax;

                if (BlendShapeNames.Contains(altSuffix))
                    exist = true;

                negativeIndex = mesh.GetBlendShapeIndex(negativeName);

                if (exist)
                {
                    positiveName = altSuffix;
                    postiveIndex = mesh.GetBlendShapeIndex(positiveName);
                }
            }

            if (BlendShapeDatabase.ContainsKey(noSuffix))
                Debug.LogError(noSuffix + "已经存在");

            BlendShapeDatabase.Add(noSuffix, new BlendShape(postiveIndex, negativeIndex));

            //移除操作
            if (positiveName != string.Empty)
                BlendShapeNames.Remove(positiveName);
            if (negativeName != string.Empty)
                BlendShapeNames.Remove(negativeName);
        }

    }

    #endregion
    public void ChangeBlendShapeValue(string blendShapeName, float value)
    {
        if (!BlendShapeDatabase.ContainsKey(blendShapeName))
        {
            Debug.LogError(blendShapeName + "不存在");
            return;
        }

        BlendShape blendshape = BlendShapeDatabase[blendShapeName];
        value = Mathf.Clamp(value, -100, 100);
        if (value > 0)
        {
            if (blendshape.postiveIndex == -1)
                return;
            skm.SetBlendShapeWeight(blendshape.postiveIndex, value);
            if (blendshape.negativeIndex == -1)
                return;
            skm.SetBlendShapeWeight(blendshape.negativeIndex, 0);
        }
        else
        {
            if (blendshape.negativeIndex == -1)
                return;
            skm.SetBlendShapeWeight(blendshape.negativeIndex, -value);
            if (blendshape.postiveIndex == -1)
                return;
            skm.SetBlendShapeWeight(blendshape.postiveIndex, 0);
        }
    }


    public bool DoesTargetMatchSkm()
    {
        return (target == skm) ? true : false;
    }

    public void ClearDatabase()
    {
        BlendShapeDatabase.Clear();
    }

    public string[] GetBlendshapeNames()
    {
        return BlendShapeDatabase.Keys.ToArray();
    }

    public int GetNumber()
    {
        return BlendShapeDatabase.Count;
    }

    public BlendShape GetBlendShape(string name)
    {
        return BlendShapeDatabase[name];
    }
}


