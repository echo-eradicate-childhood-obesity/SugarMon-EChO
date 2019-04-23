using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProductCollection 
{
    public List<ProductInfo> products = new List<ProductInfo>();
    
    int[] ints = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
    public Dictionary<Category, List<ProductInfo>> greenDic=new Dictionary<Category, List<ProductInfo>>();
    private List<ProductInfo> curDic = new List<ProductInfo>();
    public List<ProductInfo> CurDic { get { return curDic; } set { curDic = value; } }
    public void AddProduct(string name)
    {
        if (products == null)
        {
            products = new List<ProductInfo>();
        }
        var prod = new ProductInfo(name);
        products.Add(prod);
    }

    public int GetCount(List<Category> cates)
    {
        if (cates.Count > 0)
        {
            var output = 0;
            for (int i = 0; i < cates.Count; i++)
            {
                output += GetCount(cates[i]);
            }
            return output;
        }
        else { return GetCount(); }
    }

    private int GetCount(Category cate = Category.uncate)
    {
        if (products != null)
        {
            if (cate != Category.uncate)
            {
                return greenDic[cate].Count;
            }
            return this.products.Count;
        }
        return 0;
    }

#if UNITY_EDITOR
    internal void Reset()
    {
        greenDic = new Dictionary<Category, List<ProductInfo>>();
        products = new List<ProductInfo>();
    }
#endif
    public void PCSave()
    {
        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "test.txt"))
        {
            foreach (ProductInfo pi in products)
            {
                writer.WriteLine(pi.Name +";"+ pi.Type);
            }
        }
        Debug.Log("save done");
    }

    public List<ProductInfo> Load()
    {
        string line = "";
        using (StreamReader reader=new StreamReader(Application.persistentDataPath + "test.txt"))
        {
            if (products == null)
                {
                    products = new List<ProductInfo>();
                }
            while ((line = reader.ReadLine()) != null)
            {
                var arr = line.Split(';');
                var prod = new ProductInfo(arr[0], Converter.StringEnumConverter<Category, string>(arr[1]));
                products.Add(prod);
            }
        }
        return products;
    }

    public string PrintInfo(int i, Category cate = Category.uncate)
    {
        if (products != null)
        {
            return products[i].PrintInfo();
        }
        return "No Product";
    }

    public ProductInfo GetProduct(int i, List<Category> cates)
    {
        if (cates.Count==0)
        {
            return GetProduct(i);
        }
        else
        {
            return curDic[i];
        }
    }

    private ProductInfo GetProduct(int i)
    {
        if ( i>0&&i < products.Count)
        {
            return products[i];
        }
        string str = "No More Product";
        Func<ProductInfo> returnNoProd = () => {
            var pi = new ProductInfo(str);
            return pi;
        };
        return returnNoProd();
    }
}
